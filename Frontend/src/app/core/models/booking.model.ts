export interface BookingItem {
  id: number;
  propertyId: number;
  propertyTitle: string;
  propertyImageUrl: string;
  location: string;
  renterId: number;
  renterName: string;
  checkInDate: string;
  checkOutDate: string;
  guests: number;
  totalPrice: number;
  status: 'Pending' | 'Confirmed' | 'Rejected' | 'Cancelled';
  createdAt: string;
}

export interface BookingCreatePayload {
  propertyId: number;
  checkInDate: string;
  checkOutDate: string;
  guests: number;
}
