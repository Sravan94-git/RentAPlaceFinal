import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, NgZone, OnInit } from '@angular/core';
import { BookingItem } from '../../core/models/booking.model';
import { BookingService } from '../../core/services/booking.service';

@Component({
  selector: 'app-owner-bookings',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './owner-bookings.html'
})
export class OwnerBookings implements OnInit {
  bookings: BookingItem[] = [];
  loading = true;
  error = '';

  constructor(
    private readonly bookingService: BookingService,
    private readonly cdr: ChangeDetectorRef,
    private readonly ngZone: NgZone
  ) {}

  ngOnInit(): void {
    this.fetch();
  }

  fetch() {
    this.loading = true;
    this.error = '';
    this.bookingService.ownerBookings().subscribe({
      next: (res) => {
        this.ngZone.run(() => {
          this.bookings = res ?? [];
          this.loading = false;
          this.cdr.detectChanges();
        });
      },
      error: (err) => {
        this.ngZone.run(() => {
          console.error('[OwnerBookings] Error:', err);
          this.error = 'Unable to load booking requests.';
          this.loading = false;
          this.cdr.detectChanges();
        });
      }
    });
  }

  updateStatus(id: number, status: 'Confirmed' | 'Rejected') {
    this.bookingService.updateStatus(id, status).subscribe({
      next: () => {
        this.ngZone.run(() => {
          this.bookings = this.bookings.map((x) => x.id === id ? { ...x, status } : x);
          this.cdr.detectChanges();
        });
      }
    });
  }

  trackByBookingId(_: number, booking: BookingItem) {
    return booking.id;
  }
}
