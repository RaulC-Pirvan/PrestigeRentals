export interface Review {
  id: number;
  userId: number;
  vehicleId: number;
  rating: number;
  description: string;
  createdAt: string;

  userFirstName?: string;
  userLastName?: string;
  userProfilePhotoUrl?: string;
}