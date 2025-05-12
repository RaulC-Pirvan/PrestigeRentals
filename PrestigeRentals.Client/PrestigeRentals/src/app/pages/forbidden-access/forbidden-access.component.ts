import { Component } from '@angular/core';
import { ButtonComponent } from '../../shared/button/button.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forbidden-access',
  imports: [ButtonComponent, CommonModule, RouterModule],
  standalone: true,
  templateUrl: './forbidden-access.component.html',
  styleUrl: './forbidden-access.component.scss'
})
export class ForbiddenAccessComponent {
  constructor(private router: Router) {}

  goToHome() {
    this.router.navigate(['']);
  }

}
