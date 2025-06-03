import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Review } from '../models/review.model';
import { isPlatformBrowser } from '@angular/common';

export interface Vehicle {
  id: number;
  make: string;
  model: string;
  chassis: string;
  horsepower: number;
  pricePerDay: number;
  engineSize: number;
  fuelType: string;
  transmission: string;
  active: boolean;
  deleted: boolean;
  available: boolean;
}

export interface VehicleOptions {
  id: number;
  vehicleId: number;
  navigation: boolean;
  headsUpDisplay: boolean;
  hillAssist: boolean;
  cruiseControl: boolean;
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
  baseUrl = 'https://localhost:7093/api';

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  private getToken(): string | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }
    const token =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
    console.log('Retrieved token:', token);
    return token;
  }

  private getAuthHeaders(): HttpHeaders {
    const token = this.getToken();
    if (!token) throw new Error('No auth token found');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }

  getVehicleById(vehicleId: number): Observable<Vehicle> {
    const headers = this.getAuthHeaders();
    return this.http.get<Vehicle>(`${this.baseUrl}/vehicle/${vehicleId}`, {
      headers,
    });
  }

  getVehicleImage(vehicleId: number): Observable<Blob> {
    const headers = this.getAuthHeaders();
    return this.http.get(`${this.baseUrl}/image/vehicle/${vehicleId}/main`, {
      headers,
      responseType: 'blob',
    });
  }

  getAdditionalVehicleImages(vehiclId: number): Observable<string[]> {
    const headers = this.getAuthHeaders();
    return this.http.get<string[]>(
      `${this.baseUrl}/image/vehicle/${vehiclId}`,
      { headers }
    );
  }

  getAllVehicles(onlyActive: boolean = true): Observable<Vehicle[]> {
    const headers = this.getAuthHeaders();
    return this.http.get<Vehicle[]>(
      `${this.baseUrl}/vehicle?onlyActive=${onlyActive}`,
      { headers }
    );
  }

  getFilterOptions(): Observable<VehicleFilterOptions> {
    const headers = this.getAuthHeaders();
    return this.http.get<VehicleFilterOptions>(
      `${this.baseUrl}/vehicle/filter-options`,
      { headers }
    );
  }

  getVehicleOptions(vehicleId: number) {
    return this.http.get<VehicleOptions>(
      `${this.baseUrl}/vehicle/${vehicleId}/options`
    );
  }

  getReviewsByVehicleId(vehicleId: number): Observable<Review[]> {
    const token =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');

    if (!token) {
      throw new Error('No auth token found');
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http.get<Review[]>(
      `${this.baseUrl}/review/vehicle/${vehicleId}`,
      { headers }
    );
  }

  getSimilarVehicleIds(
    id: number,
    chassis: string,
    transmission: string
  ): Observable<number[]> {
    return this.http.get<number[]>(`${this.baseUrl}/vehicle/similar`, {
      params: { excludeId: id, chassis, transmission },
    });
  }
}
