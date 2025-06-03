import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthDialogComponent } from '../components/auth-dialog/auth-dialog.component';
import { AuthDialogService } from './auth-dialog.service';

export interface UserDetailsRequest {
  id: number;
  name: string;
  lastName: string;
  photo: string;
  email: string;
  role: 'User' | 'Admin';
}

export interface UserEmailRequest {
  id: number;
  email: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  public loggedInSubject = new BehaviorSubject<boolean>(this.hasToken());
  isLoggedIn$ = this.loggedInSubject.asObservable();

  private userLoadedSubject = new BehaviorSubject<boolean>(false);
  userLoaded$ = this.userLoadedSubject.asObservable();

  private userSubject = new BehaviorSubject<UserDetailsRequest | null>(null);
  userDetails$: Observable<UserDetailsRequest | null> =
    this.userSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router,
    private authDialog: AuthDialogService
  ) {
    if (this.hasToken()) {
      this.loadUserProfile();
    }
  }

  get currentUser(): UserDetailsRequest | null {
    return this.userSubject.value;
  }

  public loadUserProfile() {
    const token =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
    if (!token) {
      this.userLoadedSubject.next(true); // fără token, marchez ca încărcat ca să nu blocheze
      return;
    }

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });

    this.http
      .get<any>('https://localhost:7093/api/auth/profile', { headers })
      .subscribe({
        next: (res) => {
          const user: UserDetailsRequest = {
            id: res.userId,
            name: `${res.firstName}`,
            lastName: `${res.lastName}`,
            photo: `https://localhost:7093/api/image/user/${res.userId}`,
            email: res.email,
            role: res.role,
          };
          this.userSubject.next(user);
          this.loggedInSubject.next(true);
          this.userLoadedSubject.next(true); // marchez ca încărcat
        },
        error: (err) => {
          console.error('Failed to fetch user profile:', err);
          if (err.status === 401) {
            this.logout();
            this.authDialog.open();
          }
          this.userLoadedSubject.next(true); // marchez ca încărcat chiar dacă e eroare
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
      this.router.navigate(['/']);
    }
  }

  isLoggedIn(): boolean {
    return this.loggedInSubject.value;
  }

  register(data: any): Observable<any> {
    return this.http.post('https://localhost:7093/register', data);
  }
}
