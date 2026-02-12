import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private apiUrl = 'https://localhost:7093/api/auth';

  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/users`);
  }

  promoteUser(userId: number): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${userId}/set-admin`, {});
  }

  demoteUser(userId: number): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${userId}/set-user`, {});
  }

  banUser(userId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${userId}/ban`, {});
  }

  unbanUser(userId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${userId}/unban`, {});
  }
}
