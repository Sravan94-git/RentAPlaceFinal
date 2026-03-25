import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { PropertyCreatePayload } from '../../core/models/property.model';
import { PropertyService } from '../../core/services/property.service';

@Component({
  selector: 'app-add-property',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './add-property.html'
})
export class AddProperty {
  model: PropertyCreatePayload = {
    title: '',
    location: '',
    propertyType: 'Apartment',
    price: 0,
    maxGuests: 2,
    description: '',
    imageUrl: '',
    hasPool: false,
    isBeachFacing: false,
    hasGarden: false
  };

  loading = false;
  uploadingImage = false;
  error = '';

  constructor(
    private readonly service: PropertyService,
    private readonly router: Router
  ) {}

  submit() {
    if (!this.model.title || !this.model.location || this.model.price <= 0) {
      this.error = 'Title, location and a valid price are required.';
      return;
    }

    this.loading = true;
    this.error = '';

    this.service.add(this.model).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/owner']);
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.error?.message || 'Only owner accounts can add properties.';
      }
    });
  }

  onFileSelected(event: Event) {
    const target = event.target as HTMLInputElement;
    const file = target.files?.[0];
    if (!file) {
      return;
    }

    this.uploadingImage = true;
    this.service.uploadImage(file).subscribe({
      next: (res) => {
        this.model.imageUrl = `http://localhost:5255${res.imageUrl}`;
        this.uploadingImage = false;
      },
      error: () => {
        this.uploadingImage = false;
        this.error = 'Image upload failed.';
      }
    });
  }
}
