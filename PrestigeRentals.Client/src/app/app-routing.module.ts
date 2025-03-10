import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VehiclesComponent } from './components/vehicles/vehicles.component'; // Import your component

const routes: Routes = [
  {
    path: '',
    redirectTo: '/vehicles',
    pathMatch: 'full'
  },
  {
    path: 'vehicles',
    component: VehiclesComponent
  },
  // Add more routes as needed
];

@NgModule({
  imports: [RouterModule.forRoot(routes)], // Import RouterModule and set up routes
  exports: [RouterModule]
})
export class AppRoutingModule { }
