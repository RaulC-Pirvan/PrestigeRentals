import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

export interface UserProfile {
  firstName: string;
  lastName: string;
  userId: number;
}

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  private getToken(): string | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null; 
    }
    return (
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken')
    );
  }

getProfile(): Observable<UserProfile> {
  const token = this.getToken();
  if (!token) {
    return of(null as unknown as UserProfile); 
  }

  const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
  return this.http.get<UserProfile>('https://localhost:7093/api/auth/profile', { headers });
}

  getUserImageUrl(userId: number): Observable<Blob> {
    const token = this.getToken();
    return this.http.get(`https://localhost:7093/api/image/user/${userId}`, {
      responseType: 'blob',
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  getUserProfileById(userId: number): Observable<UserProfile> {
    const token = this.getToken();
    if (!token) throw new Error('No auth token found');

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
    return this.http.get<UserProfile>(
      `https://localhost:7093/api/auth/profile/${userId}`,
      { headers }
    );
  }
}
