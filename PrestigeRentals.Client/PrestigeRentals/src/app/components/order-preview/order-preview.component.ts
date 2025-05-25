import { Component, Input, OnInit } from "@angular/core";
import { VehicleService, Vehicle } from "../../services/vehicle.service";
import { Order } from "../../models/order.model";
import { CommonModule } from "@angular/common";

@Component({
  selector: 'app-order-preview',
  templateUrl: './order-preview.component.html',
  imports: [CommonModule],
  styleUrls: ['./order-preview.component.scss'],
})
export class OrderPreviewComponent implements OnInit {
  @Input() order!: Order;
  @Input() orderNumber!: number;

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

    // Load make & model
    this.vehicleService.getVehicleById(this.order.vehicleId).subscribe({
      next: (vehicle: Vehicle) => {
        this.vehicleMakeModel = `${vehicle.make} ${vehicle.model}`;
      },
      error: () => {
        this.vehicleMakeModel = 'Unknown Vehicle';
      },
    });

    // Load image
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
