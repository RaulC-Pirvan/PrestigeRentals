import { Injectable } from "@angular/core";

interface CheckoutData {
    startTime: string;
    endTime: string;
    totalCost: number;
    makeModel: string;
    vehicleId: number;
}

const STORAGE_KEY = 'checkoutData';

@Injectable({
    providedIn: 'root'
})

export class CheckoutDataService {
    private data: CheckoutData | null = null;

    constructor() {
        const stored = localStorage.getItem(STORAGE_KEY);
        if (stored)
            this.data = JSON.parse(stored);
    }

    setCheckoutData(data: CheckoutData)
    {
        this.data = data;
        localStorage.setItem(STORAGE_KEY, JSON.stringify(data));
    }

    getCheckoutData(): CheckoutData | null {
        if(!this.data) {
            const stored = localStorage.getItem(STORAGE_KEY);
            if(stored)
                this.data = JSON.parse(stored);
        }
        return this.data;
    }

    clearCheckoutData() {
        this.data = null;
        localStorage.removeItem(STORAGE_KEY);
    }
}