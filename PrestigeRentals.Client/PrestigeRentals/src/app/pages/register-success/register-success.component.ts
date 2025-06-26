import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { TitleComponent } from '../../shared/title/title.component';
import { ButtonComponent } from '../../shared/button/button.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-success',
  imports: [NavbarComponent, FooterComponent, TitleComponent, ButtonComponent],
  templateUrl: './register-success.component.html',
  styleUrl: './register-success.component.scss',
})
export class RegisterSuccessComponent {
  constructor(private router: Router) {}

  goToLogin() {
    this.router.navigate(['/login']);
  }
}
