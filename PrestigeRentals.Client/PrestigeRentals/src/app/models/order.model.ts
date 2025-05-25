export interface Order {
    id: number;
    userId: number;
    vehicleId: number;
    startTime: string;
    endTime: string;
    isCancelled: boolean;
}