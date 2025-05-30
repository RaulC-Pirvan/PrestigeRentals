import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CheckoutDataService } from '../../services/checkout-data.service';
import { VehicleService } from '../../services/vehicle.service';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService, UserDetailsRequest } from '../../services/auth.service';
import { HttpClient } from '@angular/common/http';

import { CommonModule } from '@angular/common';
import { NavbarComponent } from "../../components/navbar/navbar.component";
import { FooterComponent } from "../../components/footer/footer.component";
import { TitleComponent } from "../../shared/title/title.component";
import { ButtonComponent } from "../../shared/button/button.component";
import { BookingDataService } from '../../services/booking-data.service';

@Component({
  selector: 'app-order-checkout',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NavbarComponent,
    FooterComponent,
    TitleComponent,
    ButtonComponent
  ],
  templateUrl: './order-checkout.component.html',
  styleUrls: ['./order-checkout.component.scss'],
})
export class OrderCheckoutComponent implements OnInit {
  startTime!: string;
  endTime!: string;
  totalCost!: number;
  makeModel!: string;
  vehicleId!: number;
  mainPhotoUrl: string = '';
  checkoutForm!: FormGroup;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private authService: AuthService,
    private checkoutDataService: CheckoutDataService,
    private vehicleService: VehicleService,
    private http: HttpClient,
    private bookingDataService: BookingDataService
  ) {}

  ngOnInit() {
    this.checkoutForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      cardHolder: ['', Validators.required],
      cardNumber: ['', Validators.required],
      expireDate: ['', Validators.required],
      cvv: ['', Validators.required]
    });

    this.authService.userDetails$.subscribe((user: UserDetailsRequest | null) => {
      if (user) {
        const [firstName, ...lastNameParts] = user.name.split(' ');
        const lastName = lastNameParts.join(' ') || '';
        this.checkoutForm.patchValue({
          firstName: firstName,
          lastName: lastName,
          email: user.email
        });
      }
    });

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

  onCheckoutClick() {
    if (this.checkoutForm.invalid) {
      this.checkoutForm.markAllAsTouched();
      return;
    }

    const payload = {
      orderId: 1,
      totalCost: this.totalCost,
      userId: 1,
      vehicleId: this.vehicleId,
      firstName: this.checkoutForm.value.firstName,
      lastName: this.checkoutForm.value.lastName,
      email: this.checkoutForm.value.email,
      cardHolder: this.checkoutForm.value.cardHolder,
      cardNumber: this.checkoutForm.value.cardNumber,
      expireDate: this.checkoutForm.value.expireDate,
      cvv: this.checkoutForm.value.cvv
    };

    this.http.post<any>('https://localhost:7093/api/payment/mockpay', payload).subscribe({
      next: (res) => {
        if (res.success) {
          console.log(res);

          this.bookingDataService.setBookingData({
            bookingReference: res.bookingReference,
            qrCodeData: res.qrCodeData
          });

          window.location.href = "/order-confirmation";
        } else {
          alert('Payment failed: ' + res.errorMessage);
        }
      },
      error: (err) => {
        console.error('Payment error:', err);
        alert('Payment error. Please try again.');
      }
    });
  }
}
