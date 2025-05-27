import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { Vehicle, VehicleService } from '../../services/vehicle.service';
import { Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { ButtonComponent } from "../../shared/button/button.component";

@Component({
  selector: 'app-vehicle-suggestion',
  imports: [ButtonComponent],
  templateUrl: './vehicle-suggestion.component.html',
  styleUrl: './vehicle-suggestion.component.scss',
})
export class VehicleSuggestionComponent implements OnInit {
  @Input() vehicleId!: number;

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
    if (!this.vehicleId) return;

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
        this.vehicleImageUrl = URL.createObjectURL(blob);
      },
      error: () => {
        console.error('Failed to load vehicle image');
      },
    });
  }

  goToDetails(id?: number) {
    if (id) this.router.navigate(['/vehicle', id]);
  }
}