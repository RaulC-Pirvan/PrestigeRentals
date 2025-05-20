import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

interface UpdateUserDetailsRequest {
  firstName: string;
  lastName: string;
}

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly baseUrl = 'https://localhost:7093/api/auth';

  constructor(private http: HttpClient) {}

  updateUserDetails(
    userId: number,
    data: UpdateUserDetailsRequest
  ): Observable<any> {
    const token =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
    if (!token) {
      throw new Error('No auth token found');
    }

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
    return this.http.patch(`${this.baseUrl}/${userId}`, data, { headers });
  }

  changeEmail(userId: number, newEmail: string): Observable<any> {
    const token =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
    if (!token) {
      throw new Error('No auth token found');
    }

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });

    return this.http.patch(`${this.baseUrl}/${userId}/change-email`, null, {
      params: { newEmail },
      headers,
      responseType: 'text' 
    });
  }

  changePassword(
    userId: number,
    oldPassword: string,
    newPassword: string
  ): Observable<any> {
    const token =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
    if (!token) {
      throw new Error('No auth token found');
    }

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });

    return this.http.patch(`${this.baseUrl}/${userId}/change-password`, null, {
      params: {
        oldPassword,
        newPassword,
      },
      headers,
    });
  }

  setInactive(userId: number) {
    const token =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
    if (!token) {
      throw new Error('No auth token found');
    }

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });

    return this.http.patch(`${this.baseUrl}/${userId}/set-inactive`, null, { headers });
  }
}
