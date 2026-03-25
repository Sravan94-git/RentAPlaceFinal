import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { PropertyItem } from '../../../core/models/property.model';

@Component({
  selector: 'app-property-card',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './property-card.html',
  styleUrl: './property-card.css'
})
export class PropertyCard {
  @Input({ required: true }) property!: PropertyItem;
  @Input() showBookButton = false;
  @Input() showMessageButton = false;
  @Input() showDeleteButton = false;
  @Output() book = new EventEmitter<PropertyItem>();
  @Output() message = new EventEmitter<PropertyItem>();
  @Output() edit = new EventEmitter<PropertyItem>();
  @Output() remove = new EventEmitter<PropertyItem>();

  constructor(private readonly router: Router) {}

  /** Navigate to the detail page when the card body is clicked */
  navigateToDetail() {
    if (!this.showDeleteButton) {
      this.router.navigate(['/properties', this.property.id]);
    }
  }

  onBook() { this.book.emit(this.property); }
  onEdit() { this.edit.emit(this.property); }
  onMessage() { this.message.emit(this.property); }
  onRemove() { this.remove.emit(this.property); }
}
