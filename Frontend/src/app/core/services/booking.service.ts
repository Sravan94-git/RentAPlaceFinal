import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BookingCreatePayload, BookingItem } from '../models/booking.model';

@Injectable({ providedIn: 'root' })
export class BookingService {
  private readonly url = 'http://localhost:5255/api/booking';

  constructor(private readonly http: HttpClient) {}

  create(payload: BookingCreatePayload) {
    return this.http.post<BookingItem>(this.url, payload);
  }

  myBookings() {
    return this.http.get<BookingItem[]>(`${this.url}/my`);
  }

  ownerBookings() {
    return this.http.get<BookingItem[]>(`${this.url}/owner`);
  }

  cancel(id: number) {
    return this.http.patch(`${this.url}/${id}/cancel`, {});
  }

  updateStatus(id: number, status: 'Confirmed' | 'Rejected' | 'Cancelled') {
    return this.http.patch(`${this.url}/${id}/status`, { status });
  }
}
