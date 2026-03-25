import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.html'
})
export class Login {
  private router = inject(Router);
  private authService = inject(AuthService);

  email = '';
  password = '';
  
  loading = false;
  error = '';

  fillDemo(type: 'renter' | 'owner') {
    if (type === 'renter') {
      this.email = 'renter@demo.com';
      this.password = 'Password123!';
    } else {
      this.email = 'owner@demo.com';
      this.password = 'Password123!';
    }
    this.error = '';
  }

  login() {
    this.error = '';
    if (!this.email || !this.password) {
      this.error = 'Please enter your email and password.';
      return;
    }
    this.loading = true;

    this.authService.login({ email: this.email, password: this.password }).subscribe({
      next: (res) => {
        if (res.role.toLowerCase() === 'owner') {
          this.router.navigate(['/owner']);
        } else {
          this.router.navigate(['/properties']);
        }
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.error?.message || 'Invalid email or password. Please try again.';
      }
    });
  }
}