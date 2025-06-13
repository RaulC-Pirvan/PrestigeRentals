export interface Order {
    qrCodeData: string;
    bookingReference: string;
    id: number;
    userId: number;
    vehicleId: number;
    startTime: string;
    endTime: string;
    isCancelled: boolean;
}