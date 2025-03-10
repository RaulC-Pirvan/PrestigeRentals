import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Vehicle {
  id: number;
  make: string;
  model: string;
  engineSize: number;
  fuelType: string;
  transmission: string;
  active: boolean;
  deleted: boolean;
}

export interface VehicleOptions {
  vehicleId: number;
  navigation: boolean;
  headsUpDisplay: boolean;
  hillAssist: boolean;
  cruiseControl: boolean;
}

export interface VehiclePhotos {
  imageBase64: string;
}

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  private apiUrl = 'https://localhost:7093/api'; 
  constructor(private http: HttpClient) {}

  // Fetch list of vehicles
  getVehicles(): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(`${this.apiUrl}/vehicle?onlyActive=false`);
  }

  // Fetch options for a specific vehicle
  getVehicleOptions(vehicleId: number): Observable<VehicleOptions> {
    return this.http.get<VehicleOptions>(`${this.apiUrl}/vehicle/${vehicleId}/options`);
  }

  // Fetch photos for a specific vehicle
  getVehiclePhotos(vehicleId: number): Observable<VehiclePhotos[]> {
    return this.http.get<VehiclePhotos[]>(`${this.apiUrl}/vehicle/${vehicleId}/photos`);
  }
}
