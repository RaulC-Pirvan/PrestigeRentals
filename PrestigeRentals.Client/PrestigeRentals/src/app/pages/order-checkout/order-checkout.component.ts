import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CheckoutDataService } from '../../services/checkout-data.service';
import { VehicleService } from '../../services/vehicle.service';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { AuthService, UserDetailsRequest } from '../../services/auth.service';
import { HttpClient } from '@angular/common/http';

import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { TitleComponent } from '../../shared/title/title.component';
import { ButtonComponent } from '../../shared/button/button.component';
import { BookingDataService } from '../../services/booking-data.service';
import { switchMap } from 'rxjs/operators';
import { NotificationService } from '../../services/notification.service';
import { Order } from '../../models/order.model';
import { of } from 'rxjs';

@Component({
  selector: 'app-order-checkout',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NavbarComponent,
    TitleComponent,
    ButtonComponent,
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
  userId!: number;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private authService: AuthService,
    private checkoutDataService: CheckoutDataService,
    private vehicleService: VehicleService,
    private http: HttpClient,
    private bookingDataService: BookingDataService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.checkoutForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      cardHolder: ['', [Validators.required, Validators.minLength(5)]],
      cardNumber: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\d{4}\s?\d{4}\s?\d{4}\s?\d{4}$/), // accepts with or without spaces
        ],
      ],
      expireDate: [
        '',
        [
          Validators.required,
          Validators.pattern(/^(0[1-9]|1[0-2])\/\d{2}$/), // MM/YY format
        ],
      ],
      cvv: ['', [Validators.required, Validators.pattern(/^\d{3}$/)]],
    });

    this.authService.userDetails$.subscribe(
      (user: UserDetailsRequest | null) => {
        if (user) {
          this.userId = user.id;
        }
      }
    );

    this.authService.userDetails$.subscribe(
      (user: UserDetailsRequest | null) => {
        if (user) {
          const [firstName, ...lastNameParts] = user.name.split(' ');
          const lastName = lastNameParts.join(' ') || '';
          this.checkoutForm.patchValue({
            firstName: firstName,
            lastName: lastName,
          });
        }
      }
    );

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

  formatToISOString = (dateStr: string): string => {
    const d = new Date(dateStr);
    return d.toISOString();
  };

  onCheckoutClick() {
    if (this.checkoutForm.invalid) {
      this.notificationService.show(
        'Please make sure the fields are completed correctly.',
        'error'
      );
      this.checkoutForm.markAllAsTouched();
      return;
    }

    const orderPayload = {
      userId: this.userId,
      vehicleId: this.vehicleId,
      startTime: this.formatToISOString(this.startTime),
      endTime: this.formatToISOString(this.endTime),
      email: this.checkoutForm.value.email,
    };

    this.http
      .post<{
        id: number;
        bookingReference: string;
        qrCodeData: string;
        qrCodeBase64Image: string;
      }>('https://localhost:7093/api/order', orderPayload)
      .pipe(
        switchMap((orderRes) => {
          const orderId = orderRes.id;

          const paymentPayload = {
            orderId,
            totalCost: this.totalCost,
            userId: this.userId,
            vehicleId: this.vehicleId,
            firstName: this.checkoutForm.value.firstName,
            lastName: this.checkoutForm.value.lastName,
            email: this.checkoutForm.value.email,
            cardHolder: this.checkoutForm.value.cardHolder,
            cardNumber: this.checkoutForm.value.cardNumber,
            expireDate: this.checkoutForm.value.expireDate,
            cvv: this.checkoutForm.value.cvv,
          };

          console.log('📧 Email from form:', this.checkoutForm.value.email);

          this.bookingDataService.setBookingData({
            bookingReference: orderRes.bookingReference,
            qrCodeData: orderRes.qrCodeData,
            qrCodeBase64Image: orderRes.qrCodeBase64Image,
          });

          return this.http.post<any>(
            'https://localhost:7093/api/payment/mockpay',
            paymentPayload
          );
        })
      )
      .subscribe({
        next: (paymentRes) => {
          console.log('[FRONTEND] QR Data:', paymentRes.qrCodeData);

          if (paymentRes.success) {
            const data = this.bookingDataService.getBookingData();
            this.router.navigate(['/order-confirmation'], {
              state: {
                bookingReference: data?.bookingReference,
                qrCodeData: data?.qrCodeData,
                qrCodeBase64Image: data?.qrCodeBase64Image,
              },
            });
          } else {
            this.notificationService.show('Payment failed', 'error');
          }
        },
        error: () => {
          this.notificationService.show('Order or payment error', 'error');
        },
      });
  }
}
