import { Component, inject } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './navbar.html'
})
export class Navbar {
  private router = inject(Router);
  private authService = inject(AuthService);

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  get isOwner(): boolean {
    return this.authService.isOwner();
  }

  get isRenter(): boolean {
    return this.authService.isRenter();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}