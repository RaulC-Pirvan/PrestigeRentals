export interface VehicleDto {
    id: number;
    make: string;
    model: string;
    chassis: string;
    horsepower: number;
    engineSize?: number;     
    fuelType: string;
    transmission: string;
    active?: boolean;
    deleted?: boolean;
    available?: boolean;
    photos?: string[];        
  }