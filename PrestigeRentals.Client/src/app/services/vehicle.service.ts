import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, forkJoin } from 'rxjs';

export interface Vehicle {
  id: number;
  make: string;
  model: string;
  engineSize: number;
  fuelType: string;
  transmission: string;
  active: boolean;
  deleted: boolean;
  options: VehicleOptions;
}

export interface VehicleOptions {
  navigation: boolean;
  headsUpDisplay: boolean;
  hillAssist: boolean;
  cruiseControl: boolean;
}

export type VehicleForPOST = Omit<Vehicle, 'id'>;

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  private apiUrl = 'https://localhost:7093/api';
  constructor(private http: HttpClient) {}

  getVehicles(onlyActive: boolean): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(
      `${this.apiUrl}/vehicle?onlyActive=${onlyActive}`
    );
  }

  getVehicleById(id: number): Observable<Vehicle> {
    return this.http.get<Vehicle>(this.apiUrl + `/Vehicle/${id}`);
  }

  addVehicle(vehicle: VehicleForPOST) {
    return this.http.post<{ success: boolean }>('/api/vehicles', vehicle);
  }

  updateVehicle(id: number, vehicle: Vehicle): Observable<Vehicle> {
    return this.http.put<Vehicle>(this.apiUrl + '/Vehicle/${id}', vehicle);
  }

  deleteVehicle(id: number): Observable<string> {
    return this.http
      .delete(this.apiUrl + `/Vehicle/${id}`, { responseType: 'text' })
      .pipe(
        catchError((error) => {
          console.error('Error deleting vehicle:', error);
          throw error;
        })
      );
  }

  getVehicleOptions(vehicleId: number): Observable<VehicleOptions> {
    return this.http.get<VehicleOptions>(
      `${this.apiUrl}/vehicle/${vehicleId}/options`
    );
  }

  getVehiclesWithOptions(onlyActive: boolean): Observable<{
    vehicles: Vehicle[];
    options: Record<number, VehicleOptions>;
  }> {
    return new Observable((observer) => {
      this.getVehicles(onlyActive).subscribe((vehicles) => {
        if (vehicles.length === 0) {
          observer.next({ vehicles, options: {} });
          observer.complete();
          return;
        }

        const optionsPromises = vehicles.map((vehicle) =>
          this.getVehicleOptions(vehicle.id)
        );

        forkJoin(optionsPromises).subscribe((optionsArray) => {
          const optionsMap = vehicles.reduce((acc, vehicle, index) => {
            acc[vehicle.id] = optionsArray[index];
            return acc;
          }, {} as Record<number, VehicleOptions>);

          observer.next({ vehicles, options: optionsMap });
          observer.complete();
        });
      });
    });
  }

  getVehiclePhotos(vehicleId: number): Observable<any> {
    return this.http.get<any[]>(`${this.apiUrl}/vehicle/${vehicleId}/photos`);
  }

}
