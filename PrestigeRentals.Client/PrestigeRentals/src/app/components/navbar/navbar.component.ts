import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Observable } from 'rxjs';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-navbar',
  imports: [CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent implements OnInit {
  isLoggedIn$!: Observable<boolean>;
  notificationMessage: string | null = null;
  notificationType: 'success' | 'error' = 'success';

  constructor(private router: Router, private authService: AuthService, private notificationService: NotificationService) {
    this.notificationService.message$.subscribe(message => this.notificationMessage = message);
    this.notificationService.type$.subscribe(type => this.notificationType = type);
  }

  ngOnInit() {
    this.isLoggedIn$ = this.authService.isLoggedIn$; 
  }

  navigateTo(path: string) {
    this.router.navigate([path]);
  }

  logout() {
    this.authService.logout();
    this.notificationService.show('Logged out successfully', 'success');
    setTimeout(() => {
      location.reload();
    }, 1000);
    this.router.navigate(['/']);
  }
}
