import { Component, Inject, Input, PLATFORM_ID } from '@angular/core';
import { Vehicle } from '../../../../models/vehicle.model';
import { ButtonComponent } from '../../../../shared/button/button.component';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-admin-vehicle-card',
  imports: [ButtonComponent],
  templateUrl: './admin-vehicle-card.component.html',
  styleUrl: './admin-vehicle-card.component.scss',
})
export class AdminVehicleCardComponent {
  @Input() vehicle!: Vehicle;
  private cachedToken: string | null = null;

  constructor(
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object,
    private http: HttpClient,
  ) {}

  goToVehicle(vehicleId: number) {
    this.router.navigate(['/vehicle', vehicleId]);
  }

  deleteVehicle(vehicleId: number): void {
    const token = this.getToken();
    if (!token) {
      console.error('No auth token found');
      return;
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    this.http
      .delete(`https://localhost:7093/api/vehicle/${vehicleId}`, { headers })
      .subscribe({
        next: () => {
          console.log('Vehicul șters cu succes.');
          window.location.reload();
        },
        error: (err) => {
          console.error('Eroare la ștergerea vehiculului:', err);
        },
      });
  }

  private getToken(): string | null {
    if (this.cachedToken) {
      return this.cachedToken;
    }
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }
    this.cachedToken =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
    console.log('Retrieved token once:', this.cachedToken);
    return this.cachedToken;
  }
}
