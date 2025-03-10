import { Component } from '@angular/core';
import { RouterModule } from '@angular/router'; // Import RouterModule to use RouterOutlet

@Component({
  selector: 'app-root',
  standalone: true, // Mark the component as standalone
  imports: [RouterModule], // Use RouterModule here
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'PrestigeRentals';
}
