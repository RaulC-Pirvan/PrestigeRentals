import { Component, Input, OnInit } from '@angular/core';
import { Order } from '../../../../models/order.model';
import {  VehicleService } from '../../../../services/vehicle.service';
import { CommonModule } from '@angular/common';
import { Vehicle } from '../../../../models/vehicle.model';

@Component({
  selector: 'app-admin-order-overview',
  imports: [CommonModule],
  templateUrl: './admin-order-overview.component.html',
  styleUrl: './admin-order-overview.component.scss',
})
export class AdminOrderOverviewComponent implements OnInit{
  @Input() order!: Order;
  @Input() orderNumber?: number;

  vehicleMakeModel: string = 'Unknown Vehicle';
  vehicleImageUrl: string = 'assets/vehicle-placeholder.png';

  constructor(private vehicleService: VehicleService) {}

  ngOnInit(): void {
    this.loadVehicleData();
  }

  private loadVehicleData() {
    if (!this.order || !this.order.vehicleId) {
      this.vehicleMakeModel = 'No Vehicle';
      return;
    }

    this.vehicleService.getVehicleById(this.order.vehicleId).subscribe({
      next: (vehicle: Vehicle) => {
        this.vehicleMakeModel = `${vehicle.make} ${vehicle.model}`;
      },
      error: () => {
        this.vehicleMakeModel = 'Unknown Vehicle';
      },
    });

    this.vehicleService.getVehicleImage(this.order.vehicleId).subscribe({
      next: (blob) => {
        const reader = new FileReader();
        reader.onload = () => {
          this.vehicleImageUrl = reader.result as string;
        };
        reader.readAsDataURL(blob);
      },
      error: () => {
        this.vehicleImageUrl = 'assets/vehicle-placeholder.png';
      },
    });
  }
}
