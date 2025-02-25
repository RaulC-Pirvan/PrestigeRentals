import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { observableToBeFn } from 'rxjs/internal/testing/TestScheduler';

export interface Vehicle {
  id: number;
  make: string;
  model: string;
  engineSize: number;
  fuelType: string;
  transmission: string;
}

export type VehicleForPOST = Omit<Vehicle, 'id'>;

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private apiUrl = 'https://localhost:7093/api/Vehicle'
  constructor(private http: HttpClient) { }

  getVehicles(): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(this.apiUrl+'/Vehicles');
  }

  getVehicleById(id: number): Observable<Vehicle> {
    return this.http.get<Vehicle>(this.apiUrl+`/Vehicle/${id}`)
  }

  addVehicle(vehicle: VehicleForPOST): Observable<VehicleForPOST> {
    return this.http.post<VehicleForPOST>(this.apiUrl+'/Vehicle', vehicle);
  }

  updateVehicle(id: number, vehicle: Vehicle): Observable<Vehicle> {
    return this.http.put<Vehicle>(this.apiUrl+'/Vehicle/${id}', vehicle);
  }

  deleteVehicle(id: number): Observable<string> {
    return this.http.delete(this.apiUrl + `/Vehicle/${id}`, { responseType: 'text' }).pipe(
      catchError((error) => {
        console.error('Error deleting vehicle:', error);
        throw error;
      })
    );
  }
  
}
