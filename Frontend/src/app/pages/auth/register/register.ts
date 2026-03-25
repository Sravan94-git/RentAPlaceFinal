import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.html'
})
export class Register {
  private router = inject(Router);
  private authService = inject(AuthService);

  fullName = '';
  email = '';
  password = '';
  role = 'Renter'; 
  
  loading = false;
  error = '';
  success = '';

  register() {
    this.error = '';
    this.loading = true;

    if (!this.fullName || !this.email || !this.password) {
      this.error = 'All fields are required.';
      this.loading = false;
      return;
    }

    this.authService.register({
      fullName: this.fullName,
      email: this.email,
      password: this.password,
      role: this.role as 'Renter' | 'Owner'
    }).subscribe({
      next: () => {
        this.loading = false;
        this.success = 'Account created! Redirecting...';
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1500);
      },
      error: (err: any) => {
        if (err?.error?.errors) {
          // Flatten .NET validation errors into a single string
          const validationErrors = Object.values(err.error.errors).flat().join(' ');
          this.error = validationErrors;
        } else {
          this.error = err?.error?.message || 'Registration failed.';
        }
        this.loading = false;
      }
    });
  }
}