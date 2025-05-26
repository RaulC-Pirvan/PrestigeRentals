import { Component, Input, OnInit } from '@angular/core';
import { VehicleService, Vehicle } from '../../services/vehicle.service';
import { DomSanitizer } from '@angular/platform-browser';
import { ButtonComponent } from '../../shared/button/button.component';
import { StringifyOptions } from 'querystring';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vehicle-card',
  imports: [ButtonComponent],
  templateUrl: './vehicle-card.component.html',
  styleUrl: './vehicle-card.component.scss',
})
export class VehicleCardComponent implements OnInit {
  @Input() vehicleId: number = 1;

  vehicleData?: Vehicle;
  vehicleImageUrl: string = 'assets/vehicle-placeholder.png';

  constructor(
    private vehicleService: VehicleService,
    private sanitizer: DomSanitizer,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadVehicleData();
  }

  private loadVehicleData(): void {
    this.vehicleService.getVehicleById(this.vehicleId).subscribe({
      next: (vehicle) => {
        this.vehicleData = vehicle;
      },
      error: () => {
        console.error('Failed to load vehicle data');
      },
    });

    this.vehicleService.getVehicleImage(this.vehicleId).subscribe({
      next: (blob: Blob) => {
        const reader = new FileReader();
        reader.onload = () => {
          this.vehicleImageUrl = reader.result as string;
        };
        reader.readAsDataURL(blob);
      },
      error: () => {
        console.error('Failed to load vehicle image');
      },
    });
  }

  goToDetails(id?: number) {
    this.router.navigate(['/vehicle', id]);
  }
}
