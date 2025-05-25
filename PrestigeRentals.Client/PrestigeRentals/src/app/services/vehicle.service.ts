import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Vehicle {
  id: number;
  make: string;
  model: string;
  chassis: string;
  horsepower: number;
  engineSize: number;
  fuelType: string;
  transmission: string;
  active: boolean;
  deleted: boolean;
  available: boolean;
}

export interface VehicleFilterOptions {
  makes: string[];
  models: string[];
  fuelTypes: string[];
  transmissions: string[];
  chassis: string[];
}

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  private baseUrl = 'https://localhost:7093/api';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
    if (!token) throw new Error('No auth token found');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }

  getVehicleById(vehicleId: number): Observable<Vehicle> {
    const headers = this.getAuthHeaders();
    return this.http.get<Vehicle>(`${this.baseUrl}/vehicle/${vehicleId}`, { headers });
  }

  getVehicleImage(vehicleId: number): Observable<Blob> {
    const headers = this.getAuthHeaders();
    return this.http.get(`${this.baseUrl}/image/vehicle/${vehicleId}/main`, {
      headers,
      responseType: 'blob',
    });
  }

  getAllVehicles(onlyActive: boolean = true): Observable<Vehicle[]> {
    const headers = this.getAuthHeaders();
    return this.http.get<Vehicle[]>(`${this.baseUrl}/vehicle?onlyActive=${onlyActive}`, { headers });
  }

  getFilterOptions(): Observable<VehicleFilterOptions> {
    const headers = this.getAuthHeaders();
    return this.http.get<VehicleFilterOptions>(`${this.baseUrl}/vehicle/filter-options`, { headers });
  }
}
