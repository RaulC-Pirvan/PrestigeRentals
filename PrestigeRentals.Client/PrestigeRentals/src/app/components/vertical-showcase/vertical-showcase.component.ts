import { Component } from '@angular/core';
import { ButtonComponent } from '../../shared/button/button.component';
import { Router } from '@angular/router';
import { TitleComponent } from '../../shared/title/title.component';

@Component({
  selector: 'app-vertical-showcase',
  imports: [ButtonComponent, TitleComponent],
  templateUrl: './vertical-showcase.component.html',
  styleUrl: './vertical-showcase.component.scss'
})
export class VerticalShowcaseComponent {

  constructor(private router: Router) {}

  goToInventory() {
    this.router.navigate(['/login']);
  }

}
