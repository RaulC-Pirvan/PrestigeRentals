import { Component } from '@angular/core';
import { ButtonComponent } from '../../shared/button/button.component';
import { Router } from '@angular/router';


@Component({
  selector: 'app-banner-with-button',
  imports: [ButtonComponent],
  templateUrl: './banner-with-button.component.html',
  styleUrl: './banner-with-button.component.scss'
})
export class BannerWithButtonComponent {

  constructor(private router: Router) {}

  goToLogin() {
    this.router.navigate(['/login']);
  }
}
