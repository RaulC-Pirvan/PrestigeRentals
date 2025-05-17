import { Component } from '@angular/core';
import { ButtonComponent } from '../../shared/button/button.component';
import { Router } from '@angular/router';
import { TitleComponent } from '../../shared/title/title.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-half-photo-showcase',
  imports: [ButtonComponent, TitleComponent],
  templateUrl: './half-photo-showcase.component.html',
  styleUrl: './half-photo-showcase.component.scss'
})
export class HalfPhotoShowcaseComponent {
  constructor(private router: Router, private authService: AuthService) {}

  goToInventoryOrLogin() {
    if (this.authService.isLoggedIn())
    {
      this.router.navigate(['/Inventory']);
    } else {
      this.router.navigate(['/login']);
    }
  }

  goToInventory() {
    this.router.navigate(['/login']);
  }
}
