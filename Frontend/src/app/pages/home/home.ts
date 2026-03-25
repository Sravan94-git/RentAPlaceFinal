import { ChangeDetectorRef, Component, NgZone, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PropertyCard } from '../../shared/components/property-card/property-card';
import { PropertyItem } from '../../core/models/property.model';
import { PropertyService } from '../../core/services/property.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule, PropertyCard],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {
  featuredProperties: PropertyItem[] = [];
  loading = true;

  constructor(
    private readonly propertyService: PropertyService,
    private readonly ngZone: NgZone,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.propertyService.getAll({ page: 1, pageSize: 4, sortBy: '' }).subscribe({
      next: (res) => {
        this.ngZone.run(() => {
          this.featuredProperties = res?.items ?? [];
          this.loading = false;
          this.cdr.detectChanges();
        });
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}