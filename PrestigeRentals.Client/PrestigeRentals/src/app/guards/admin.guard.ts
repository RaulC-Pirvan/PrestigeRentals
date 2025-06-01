import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class AdminGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean | UrlTree | Observable<boolean | UrlTree> {
    console.log('[AdminGuard] Checking if user can activate route...');

    return this.authService.userDetails$.pipe(
      map((user) => {
        console.log('[AdminGuard] User received:', user);

        if (user?.role === 'Admin') {
          console.log('[AdminGuard] User is Admin -> access granted ✅');
          return true;
        }

        console.warn('[AdminGuard] Access denied ❌ - Redirecting to /');
        return this.router.parseUrl('/');
      })
    );
  }
}