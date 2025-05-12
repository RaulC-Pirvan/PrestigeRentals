import { Component } from '@angular/core';
import { LandingPageComponent } from './pages/landing-page/landing-page.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';

@Component({
  selector: 'app-root',
  imports: [LandingPageComponent, NotFoundComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'PrestigeRentals';
}
