import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { QRCodeComponent } from 'angularx-qrcode';
import { BookingDataService } from '../../services/booking-data.service';
import { NavbarComponent } from "../../components/navbar/navbar.component";
import { FooterComponent } from "../../components/footer/footer.component";
import { TitleComponent } from "../../shared/title/title.component";
import { ButtonComponent } from "../../shared/button/button.component";

@Component({
  selector: 'app-order-confirmation',
  standalone: true,
  imports: [CommonModule, QRCodeComponent, NavbarComponent, FooterComponent, TitleComponent, ButtonComponent],
  templateUrl: './order-confirmation.component.html',
  styleUrl: './order-confirmation.component.scss',
})
export class OrderConfirmationComponent implements OnInit {
  bookingReference: string = '';
  qrCodeData: string = '';

  constructor(
    private route: ActivatedRoute,
    public router: Router,
    private bookingDataService: BookingDataService
  ) {}

  ngOnInit(): void {
    const nav = this.router.getCurrentNavigation();
    const state = nav?.extras?.state as {
      bookingReference: string;
      qrCodeData: string;
    };

    if (state) {
      this.bookingReference = state.bookingReference;
      this.qrCodeData = state.qrCodeData;

      // De asemenea actualizează serviciul, dacă vrei să sincronizezi
      this.bookingDataService.setBookingData(state);

      console.log('QR Code Data (from navigation state):', this.qrCodeData);
    } else {
      // Încearcă să iei datele din serviciu
      const storedData = this.bookingDataService.getBookingData();

      if (storedData) {
        this.bookingReference = storedData.bookingReference;
        this.qrCodeData = storedData.qrCodeData;
        console.log('QR Code Data (from stored data):', this.qrCodeData);
      } else {
        console.warn('No navigation state found, no stored booking data.');
      }
    }
  }

  goHome() {
    this.router.navigate(['/']);
  }
}
