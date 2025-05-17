import { Component } from '@angular/core';
import { ButtonComponent } from '../../shared/button/button.component';
import { Router } from '@angular/router';
import { TitleComponent } from '../../shared/title/title.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-vertical-showcase',
  imports: [ButtonComponent, TitleComponent],
  templateUrl: './vertical-showcase.component.html',
  styleUrl: './vertical-showcase.component.scss'
})
export class VerticalShowcaseComponent {

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
