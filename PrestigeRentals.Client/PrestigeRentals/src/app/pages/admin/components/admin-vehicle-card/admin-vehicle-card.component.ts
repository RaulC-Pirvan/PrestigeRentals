import { Component, Input } from '@angular/core';
import { Vehicle } from '../../../../models/vehicle.model';
import { ButtonComponent } from "../../../../shared/button/button.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-vehicle-card',
  imports: [ButtonComponent],
  templateUrl: './admin-vehicle-card.component.html',
  styleUrl: './admin-vehicle-card.component.scss',
})
export class AdminVehicleCardComponent {
  @Input() vehicle!: Vehicle;

  constructor(private router: Router) {}

  goToVehicle(vehicleId: number)
  {
    this.router.navigate(['/vehicle', vehicleId]);
  }
}
