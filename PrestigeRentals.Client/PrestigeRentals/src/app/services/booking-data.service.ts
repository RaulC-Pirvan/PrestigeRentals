import { Injectable } from '@angular/core';

interface BookingData {
  bookingReference: string;
  qrCodeData: string;
}

const STORAGE_KEY = 'bookingData';

@Injectable({
  providedIn: 'root',
})
export class BookingDataService {
  private data: BookingData | null = null;

  constructor() {
    const stored = sessionStorage.getItem(STORAGE_KEY);
    if (stored) this.data = JSON.parse(stored);
  }

  setBookingData(data: BookingData) {
    this.data = data;
    sessionStorage.setItem(STORAGE_KEY, JSON.stringify(data));
  }

  getBookingData(): BookingData | null {
    if (!this.data) {
      const stored = sessionStorage.getItem(STORAGE_KEY);
      if (stored) this.data = JSON.parse(stored);
    }
    return this.data;
  }

  clearBookingData() {
    this.data = null;
    sessionStorage.removeItem(STORAGE_KEY);
  }
}
