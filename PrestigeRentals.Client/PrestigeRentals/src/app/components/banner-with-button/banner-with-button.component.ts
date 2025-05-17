import { Component } from '@angular/core';
import { ButtonComponent } from '../../shared/button/button.component';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';


@Component({
  selector: 'app-banner-with-button',
  imports: [ButtonComponent],
  templateUrl: './banner-with-button.component.html',
  styleUrl: './banner-with-button.component.scss'
})
export class BannerWithButtonComponent {

  constructor(private router: Router, private authService: AuthService) {}

  goToInventoryOrLogin() {
    if (this.authService.isLoggedIn())
    {
      this.router.navigate(['/Inventory']);
    } else {
      this.router.navigate(['/login']);
    }
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }
}
