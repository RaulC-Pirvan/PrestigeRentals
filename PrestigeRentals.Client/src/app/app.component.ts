import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router'; // Make sure to import RouterOutlet

@Component({
  selector: 'app-root',
  standalone: true, // Define as a standalone component
  imports: [RouterOutlet],  // Include RouterOutlet to enable routing
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'PrestigeRentals';
}