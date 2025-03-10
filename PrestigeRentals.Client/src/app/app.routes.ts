import { Routes } from '@angular/router';
import { VehiclesComponent } from './components/vehicles/vehicles.component'; // Make sure the path is correct

export const routes: Routes = [
  { path: '', redirectTo: '/vehicles', pathMatch: 'full' },
  { path: 'vehicles', component: VehiclesComponent },
  // Other routes can go here
];