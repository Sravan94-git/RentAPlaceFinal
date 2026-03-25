import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, NgZone, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BookingItem } from '../../core/models/booking.model';
import { BookingService } from '../../core/services/booking.service';

@Component({
  selector: 'app-renter-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './renter-dashboard.html'
})
export class RenterDashboard implements OnInit {
  bookings: BookingItem[] = [];
  loading = true;
  error = '';

  constructor(
    private readonly bookingService: BookingService,
    private readonly cdr: ChangeDetectorRef,
    private readonly ngZone: NgZone
  ) {}

  ngOnInit() {
    this.fetch();
  }

  fetch() {
    this.loading = true;
    this.error = '';
    this.bookingService.myBookings().subscribe({
      next: (res) => {
        this.ngZone.run(() => {
          this.bookings = res ?? [];
          this.loading = false;
          this.cdr.detectChanges();
        });
      },
      error: (err) => {
        this.ngZone.run(() => {
          console.error('[MyBookings] Error:', err);
          this.error = 'Unable to load your bookings.';
          this.loading = false;
          this.cdr.detectChanges();
        });
      }
    });
  }

  cancel(bookingId: number) {
    this.bookingService.cancel(bookingId).subscribe({
      next: () => {
        this.ngZone.run(() => {
          this.bookings = this.bookings.map((x) =>
            x.id === bookingId ? { ...x, status: 'Cancelled' } : x
          );
          this.cdr.detectChanges();
        });
      }
    });
  }

  trackByBookingId(_: number, booking: BookingItem) {
    return booking.id;
  }
}
