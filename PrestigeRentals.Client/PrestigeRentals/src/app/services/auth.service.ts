import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

export interface User {
  name: string;
  photo: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  public loggedInSubject = new BehaviorSubject<boolean>(this.hasToken());
  isLoggedIn$ = this.loggedInSubject.asObservable();

  private userSubject = new BehaviorSubject<User | null>(null);
  user$: Observable<User | null> = this.userSubject.asObservable();

  constructor(private http: HttpClient) {
    if (this.hasToken()) {
      this.loadUserProfile();
    }
  }

  public loadUserProfile() {
    const token =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
    if (!token) return;

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });

    this.http
      .get<any>('https://localhost:7093/api/auth/profile', { headers })
      .subscribe({
        next: (res) => {
          const user: User = {
            name: `${res.firstName}`,
            photo: res.imageData,
          };
          this.userSubject.next(user);
        },
        error: (err) => {
          console.error('Failed to fetch user profile:', err);
        },
      });
  }

  private hasToken(): boolean {
    const isBrowser = typeof window !== 'undefined';
    if (!isBrowser) return false;

    return (
      !!localStorage.getItem('authToken') ||
      !!sessionStorage.getItem('authToken')
    );
  }

  login(token: string, rememberMe: boolean): void {
    const isBrowser = typeof window !== 'undefined';

    if (!isBrowser) return;

    if (rememberMe) {
      localStorage.setItem('authToken', token);
      sessionStorage.removeItem('authToken');
    } else {
      sessionStorage.setItem('authToken', token);
      localStorage.removeItem('authToken');
    }
  }

  logout(): void {
    const isBrowser = typeof window !== 'undefined';
    if (isBrowser) {
      localStorage.removeItem('authToken');
      sessionStorage.removeItem('authToken');
      this.loggedInSubject.next(false);
      this.userSubject.next(null);
    }
  }

  isLoggedIn(): boolean {
    return this.loggedInSubject.value;
  }
}
