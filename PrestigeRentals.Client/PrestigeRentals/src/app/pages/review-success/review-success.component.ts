import { Component } from '@angular/core';
import { ButtonComponent } from '../../shared/button/button.component';
import { TitleComponent } from '../../shared/title/title.component';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-review-success',
  imports: [ButtonComponent, TitleComponent, NavbarComponent, CommonModule],
  templateUrl: './review-success.component.html',
  styleUrl: './review-success.component.scss',
})
export class ReviewSuccessComponent {

    constructor(
    public router: Router,
  ) {}
  goHome() {
    this.router.navigate(['/']);
  }
}
