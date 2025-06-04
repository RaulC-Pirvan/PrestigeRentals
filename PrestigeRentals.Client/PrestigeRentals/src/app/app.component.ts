import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import {
  Router,
  NavigationStart,
  NavigationEnd,
  NavigationCancel,
  NavigationError,
} from '@angular/router';
import { NotificationService } from './services/notification.service';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'PrestigeRentals';

  notificationMessage: string | null = null;
  notificationType: 'success' | 'error' = 'success';

  private hasReloaded = false;

  constructor(
    private notificationService: NotificationService,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit() {
    this.notificationService.message$.subscribe((message) => {
      this.notificationMessage = message;
    });

    this.notificationService.type$.subscribe((type) => {
      this.notificationType = type;
    });

    this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        if (isPlatformBrowser(this.platformId)) {
          const currentUrl = window.location.pathname;
          // Comentăm/refacem această parte
          // if (!event.restoredState && event.url !== currentUrl && !this.hasReloaded) {
          //   this.hasReloaded = true;
          //   window.location.href = event.url;
          // }
        }
      }
      if (
        event instanceof NavigationEnd ||
        event instanceof NavigationCancel ||
        event instanceof NavigationError
      ) {
        this.hasReloaded = false;
      }
    });
  }
}
