import { Component } from '@angular/core';
import { ForbiddenAccessComponent } from './pages/forbidden-access/forbidden-access.component';

@Component({
  selector: 'app-root',
  imports: [ForbiddenAccessComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'PrestigeRentals';
}
