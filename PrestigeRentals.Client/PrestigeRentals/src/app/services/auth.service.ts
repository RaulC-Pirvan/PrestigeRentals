import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private loggedInSubject = new BehaviorSubject<boolean>(this.hasToken());

  isLoggedIn$ = this.loggedInSubject.asObservable();

  private hasToken(): boolean {
    const isBrowser = typeof window !== 'undefined';
    if(!isBrowser) return false;

    return !!localStorage.getItem('authToken') || !!sessionStorage.getItem('authToken');
  }

  login(token: string): void {
    const isBrowser = typeof window !== 'undefined';
    if (isBrowser) {
      localStorage.setItem('authToken', token);
      this.loggedInSubject.next(true);
    }
  }

  logout(): void {
    const isBrowser = typeof window !== 'undefined';
    if (isBrowser) {
      localStorage.removeItem('authToken');
      this.loggedInSubject.next(false);
    }
  }

  isLoggedIn(): boolean {
    return this.loggedInSubject.value;
  }
}
