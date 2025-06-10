import { Component, Input, OnInit } from '@angular/core';
import { VehicleService } from '../../services/vehicle.service';
import { DomSanitizer } from '@angular/platform-browser';
import { ButtonComponent } from '../../shared/button/button.component';
import { Router } from '@angular/router';
import { Vehicle } from '../../models/vehicle.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vehicle-card',
  imports: [ButtonComponent, CommonModule],
  templateUrl: './vehicle-card.component.html',
  styleUrls: ['./vehicle-card.component.scss'],
})
export class VehicleCardComponent implements OnInit {
  @Input() vehicleId: number = 1;
  @Input() vehicle?: Vehicle;
  @Input() orderEndTime?: string;

  vehicleData?: Vehicle;
  vehicleImageUrl: string = 'assets/vehicle-placeholder.png';

  remainingTime: string | null = null;
  private timerInterval: any;

  constructor(
    private vehicleService: VehicleService,
    private sanitizer: DomSanitizer,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (this.vehicle) {
      this.vehicleData = this.vehicle;
    }

    if (!this.vehicleData?.available && this.orderEndTime) {
      this.startCountdown(this.orderEndTime);
    }

    this.loadVehicleData();
  }

  startCountdown(endTime: string) {
    const end = new Date(endTime).getTime();

    this.timerInterval = setInterval(() => {
      const now = new Date().getTime();
      const diff = end - now;

      if (diff <= 0) {
        clearInterval(this.timerInterval);
        this.remainingTime = null;
        return;
      }

      const hours = Math.floor(diff / (1000 * 60 * 60));
      const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
      const seconds = Math.floor((diff % (1000 * 60)) / 1000);

      this.remainingTime = `${this.pad(hours)}:${this.pad(minutes)}:${this.pad(
        seconds
      )}`;
    }, 1000);
  }

  pad(n: number): string {
    return n < 10 ? '0' + n : n.toString();
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

  ngOnDestroy(): void {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }
  }

  goToDetails(id?: number) {
    this.router.navigate(['/vehicle', id]);
  }
}
