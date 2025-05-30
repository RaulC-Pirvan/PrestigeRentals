import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { QRCodeComponent } from 'angularx-qrcode';

@Component({
  selector: 'app-order-confirmation',
  standalone: true,
  imports: [CommonModule, QRCodeComponent],
  templateUrl: './order-confirmation.component.html',
  styleUrl: './order-confirmation.component.scss',
})
export class OrderConfirmationComponent implements OnInit {
  bookingReference: string = '';
  qrCodeData: string = '';

   constructor(private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    const nav = this.router.getCurrentNavigation();
    const state = nav?.extras?.state as {
      bookingReference: string;
      qrCodeData: string;
    };

    if (state) {
      this.bookingReference = state.bookingReference;
      this.qrCodeData = state.qrCodeData;
    } else {
      window.location.href = "/order-checkout";
    }
  }

}
