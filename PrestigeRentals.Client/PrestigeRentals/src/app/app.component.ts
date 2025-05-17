import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NotificationService } from './services/notification.service';
import { CommonModule } from '@angular/common';

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

  constructor(private notificationService: NotificationService) {}

  ngOnInit() {
    this.notificationService.message$.subscribe(message => {
      this.notificationMessage = message;
    });

    this.notificationService.type$.subscribe(type => {
      this.notificationType = type;
    });
  }

}
