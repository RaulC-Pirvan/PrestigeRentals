export interface Vehicle {
    id: number;
    make: string;
    model: string;
    engineSize: number;
    fuelType: string;
    pricePerDay: number;
    transmission: string;
    chassis: string;
    horsepower: number;
    navigation: boolean;
    headsupDisplay: boolean;
    hillAssist: boolean;
    cruiseControl: boolean;
    imageUrl: string;
    active: boolean;
    deleted: boolean;
    available: boolean;
  }