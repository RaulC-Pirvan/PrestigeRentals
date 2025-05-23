import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface UserProfile {
  firstName: string;
  lastName: string;
  userId: number;
}

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  constructor(private http: HttpClient) {}

  getProfile(): Observable<UserProfile> {
    const token =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
    if (!token) {
      throw new Error('No auth token found');
    }

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
    return this.http.get<UserProfile>(
      'https://localhost:7093/api/auth/profile', { headers }
    );
  }

  getUserImageUrl(userId: number): Observable<Blob> {
    const token = localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
    return this.http.get(`https://localhost:7093/api/image/user/${userId}`, {
      responseType: 'blob',
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  }
}
