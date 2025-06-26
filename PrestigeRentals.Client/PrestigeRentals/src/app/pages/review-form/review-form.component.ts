import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NotificationService } from '../../services/notification.service';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NavbarComponent } from "../../components/navbar/navbar.component";
import { TitleComponent } from "../../shared/title/title.component";
import { ButtonComponent } from "../../shared/button/button.component";
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-review-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavbarComponent, TitleComponent, ButtonComponent],
  templateUrl: './review-form.component.html',
  styleUrls: ['./review-form.component.scss'],
})
export class ReviewFormComponent implements OnInit {
  reviewForm: FormGroup;
  orderId: number | null = null;
  userId: number | null = null;
  vehicleId: number | null = null;
  vehicleName: string = '';
  vehicleImageUrl: string = '';

  constructor(
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router,
    private authService: AuthService
  ) {
    this.reviewForm = this.fb.group({
      rating: [
        null,
        [Validators.required, Validators.min(1), Validators.max(5)],
      ],
      description: ['', [Validators.required, Validators.minLength(5)]],
    });
  }

  ngOnInit(): void {
    const param = this.route.snapshot.queryParamMap.get('orderId');
    this.orderId = param ? +param : null;

    if (this.orderId) {
      this.fetchOrderAndVehicle(this.orderId);
    }
  }

  fetchOrderAndVehicle(orderId: number) {
    this.http.get<any>(`https://localhost:7093/api/order/${orderId}`).subscribe({
      next: order => {
        this.vehicleId = order.vehicleId;
        this.userId = order.userId;

        const loggedInUserId = this.authService.currentUser?.id;
        if (!loggedInUserId || this.userId !== loggedInUserId) {
          this.notificationService.show("You are not authorized to review this order.", "error");
          this.router.navigate(['/forbidden']);
          return;
        }

        // Check if review already exists
        this.http.get<boolean>(`https://localhost:7093/api/review/exists/${orderId}`).subscribe({
          next: exists => {
            if (exists) {
              this.notificationService.show("You’ve already submitted a review for this order.", "error");
              this.router.navigate(['/']);
              return;
            }

            // Proceed with loading vehicle info if allowed
            this.loadVehicleInfo(this.vehicleId!);
          },
          error: () => {
            this.notificationService.show("Failed to verify review existence.", "error");
          }
        });
      },
      error: () => {
        this.notificationService.show("Failed to load order information.", "error");
        this.router.navigate(['/not-found']);
      }
    });
  }

  loadVehicleInfo(vehicleId: number) {
    this.http.get<any>(`https://localhost:7093/api/vehicle/${vehicleId}`).subscribe(vehicle => {
      this.vehicleName = `${vehicle.make} ${vehicle.model}`;
    });

    this.http.get(`https://localhost:7093/api/image/vehicle/${vehicleId}/main`, { responseType: 'blob' })
      .subscribe(blob => {
        this.vehicleImageUrl = URL.createObjectURL(blob);
      });
  }

  onSubmit() {
    const currentUserId = this.authService.currentUser?.id;
    if (this.reviewForm.invalid || !this.vehicleId || !this.orderId || !currentUserId) return;

    const payload = {
      userId: currentUserId,
      vehicleId: this.vehicleId,
      orderId: this.orderId,
      rating: this.reviewForm.value.rating,
      description: this.reviewForm.value.description,
    };

    this.http.post('https://localhost:7093/api/review', payload).subscribe({
      next: () => {
        this.notificationService.show('Review submitted!', 'success');
        this.router.navigate(['/review-success']);
      },
      error: (err) => {
        const message = err?.error?.message || "Failed to submit review.";
        this.notificationService.show(message, 'error');
      }
    });
  }
}
