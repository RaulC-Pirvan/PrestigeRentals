import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { TitleComponent } from '../../shared/title/title.component';
import { Router, RouterLink } from '@angular/router';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-settings',
  imports: [NavbarComponent, FooterComponent, TitleComponent, RouterOutlet, CommonModule, RouterLink],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss',
})
export class SettingsComponent {
  constructor(private router: Router) {}



  navigateTo(path: string) {
    this.router.navigate([path]);
  }
}
