import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CheckoutDataService } from '../../services/checkout-data.service';
import { VehicleService } from '../../services/vehicle.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-order-checkout',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-checkout.component.html',
  styleUrl: './order-checkout.component.scss',
})
export class OrderCheckoutComponent implements OnInit {
  startTime!: string;
  endTime!: string;
  totalCost!: number;
  makeModel!: string;
  vehicleId!: number;
  mainPhotoUrl: string = '';

  constructor(
    private router: Router,
    private checkoutDataService: CheckoutDataService,
    private vehicleService: VehicleService
  ) {}

  ngOnInit() {
    const nav = this.router.getCurrentNavigation();
    const state = nav?.extras?.state as {
      startTime: string;
      endTime: string;
      totalCost: number;
      makeModel: string;
      vehicleId: number;
    };

    if (state) {
      this.startTime = state.startTime;
      this.endTime = state.endTime;
      this.totalCost = state.totalCost;
      this.makeModel = state.makeModel;
      this.vehicleId = state.vehicleId;

      this.checkoutDataService.setCheckoutData(state);
    } else {
      const saved = this.checkoutDataService.getCheckoutData();
      if (saved) {
        this.startTime = saved.startTime;
        this.endTime = saved.endTime;
        this.totalCost = saved.totalCost;
        this.makeModel = saved.makeModel;
        this.vehicleId = saved.vehicleId;
      } else {
        console.error('No checkout data found!');
        return;
      }
    }

    this.loadMainPhoto();
  }

  private loadMainPhoto() {
    this.vehicleService.getVehicleImage(this.vehicleId).subscribe({
      next: (blob) => {
        this.mainPhotoUrl = URL.createObjectURL(blob);
      },
      error: (err) => {
        console.error('Error loading main photo:', err);
      },
    });
  }
}
