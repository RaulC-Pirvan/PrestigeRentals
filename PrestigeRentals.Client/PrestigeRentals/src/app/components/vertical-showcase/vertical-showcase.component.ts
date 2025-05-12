import { Component } from '@angular/core';
import { ButtonComponent } from '../../shared/button/button.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vertical-showcase',
  imports: [ButtonComponent],
  templateUrl: './vertical-showcase.component.html',
  styleUrl: './vertical-showcase.component.scss'
})
export class VerticalShowcaseComponent {

  constructor(private router: Router) {}

  goToInventory() {
    this.router.navigate(['/login']);
  }

}
