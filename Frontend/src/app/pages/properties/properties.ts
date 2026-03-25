import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, NgZone, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BookingCreatePayload } from '../../core/models/booking.model';
import { PropertyItem, PropertySearchParams } from '../../core/models/property.model';
import { AuthService } from '../../core/services/auth.service';
import { BookingService } from '../../core/services/booking.service';
import { MessageService } from '../../core/services/message.service';
import { PropertyService } from '../../core/services/property.service';
import { PropertyCard } from '../../shared/components/property-card/property-card';

@Component({
  selector: 'app-properties',
  standalone: true,
  imports: [CommonModule, FormsModule, PropertyCard],
  templateUrl: './properties.html'
})
export class Properties implements OnInit {
  properties: PropertyItem[] = [];
  loading = true;
  error = '';
  success = '';

  page = 1;
  pageSize = 15;
  totalCount = 0;
  villaTotal = 0;
  apartmentTotal = 0;
  bungalowTotal = 0;

  search: PropertySearchParams = {
    location: '',
    propertyType: '',
    minPrice: undefined,
    maxPrice: undefined,
    guests: undefined,
    sortBy: '',
    hasPool: undefined,
    isBeachFacing: undefined,
    hasGarden: undefined
  };

  bookingModalOpen = false;
  selectedProperty: PropertyItem | null = null;
  bookingForm: BookingCreatePayload = {
    propertyId: 0,
    checkInDate: '',
    checkOutDate: '',
    guests: 1
  };

  constructor(
    private readonly propertyService: PropertyService,
    private readonly bookingService: BookingService,
    private readonly authService: AuthService,
    private readonly messageService: MessageService,
    private readonly cdr: ChangeDetectorRef,
    private readonly ngZone: NgZone
  ) {}

  ngOnInit() {
    this.fetchProperties();
    this.loadCategoryCounts();
  }

  get canBook(): boolean {
    return this.authService.isRenter();
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.totalCount / this.pageSize));
  }

  get villaCount(): number {
    return this.villaTotal;
  }

  get apartmentCount(): number {
    return this.apartmentTotal;
  }

  get bungalowCount(): number {
    return this.bungalowTotal;
  }

  loadCategoryCounts() {
    this.propertyService.getAll({ propertyType: 'Villa', page: 1, pageSize: 1 })
      .subscribe(res => { this.villaTotal = res?.totalCount ?? 0; this.cdr.detectChanges(); });
    this.propertyService.getAll({ propertyType: 'Apartment', page: 1, pageSize: 1 })
      .subscribe(res => { this.apartmentTotal = res?.totalCount ?? 0; this.cdr.detectChanges(); });
    this.propertyService.getAll({ propertyType: 'Bungalow', page: 1, pageSize: 1 })
      .subscribe(res => { this.bungalowTotal = res?.totalCount ?? 0; this.cdr.detectChanges(); });
  }

  fetchProperties() {
    this.loading = true;
    this.error = '';
    this.success = '';
    this.propertyService
      .getAll({
        ...this.search,
        page: this.page,
        pageSize: this.pageSize
      })
      .subscribe({
        next: (res) => {
          this.ngZone.run(() => {
            this.properties = res?.items ?? [];
            this.totalCount = res?.totalCount ?? 0;
            this.loading = false;
            this.cdr.detectChanges();
          });
        },
        error: (err) => {
          this.ngZone.run(() => {
            console.error('[Properties] API Error:', err);
            this.error = 'Failed to load properties.';
            this.loading = false;
            this.cdr.detectChanges();
          });
        }
      });
  }

  applyFilters() {
    this.page = 1;
    this.fetchProperties();
  }

  resetFilters() {
    this.search = {
      location: '',
      propertyType: '',
      minPrice: undefined,
      maxPrice: undefined,
      guests: undefined,
      sortBy: '',
      hasPool: undefined,
      isBeachFacing: undefined,
      hasGarden: undefined
    };
    this.applyFilters();
  }

  openBookModal(property: PropertyItem) {
    if (!this.canBook) {
      this.error = 'Please login as a renter to book properties.';
      return;
    }

    this.selectedProperty = property;
    this.bookingForm = {
      propertyId: property.id,
      checkInDate: '',
      checkOutDate: '',
      guests: 1
    };
    this.bookingModalOpen = true;
  }

  messageOwner(property: PropertyItem) {
    if (!this.authService.isRenter()) {
      this.error = 'Please login as a renter to message owners.';
      return;
    }

    const content = window.prompt(`Send message to owner of "${property.title}"`);
    if (!content || !content.trim()) {
      return;
    }

    this.messageService
      .send({
        propertyId: property.id,
        receiverId: property.ownerId,
        content: content.trim()
      })
      .subscribe({
        next: () => {
          this.success = 'Message sent to owner.';
        },
        error: (err) => {
          this.error = err?.error?.message || 'Failed to send message.';
        }
      });
  }

  closeBookModal() {
    this.bookingModalOpen = false;
    this.selectedProperty = null;
  }

  submitBooking() {
    if (!this.selectedProperty) {
      return;
    }
    this.bookingService.create(this.bookingForm).subscribe({
      next: () => {
        this.success = 'Booking request submitted successfully.';
        this.closeBookModal();
      },
      error: (err) => {
        this.error = err?.error?.message || 'Booking failed.';
      }
    });
  }

  nextPage() {
    if (this.page < this.totalPages) {
      this.page += 1;
      this.fetchProperties();
    }
  }

  prevPage() {
    if (this.page > 1) {
      this.page -= 1;
      this.fetchProperties();
    }
  }
}