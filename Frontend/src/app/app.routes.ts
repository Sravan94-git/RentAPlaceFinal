import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Properties } from './pages/properties/properties';
import { PropertyDetail } from './pages/property-detail/property-detail';
import { OwnerDashboard } from './pages/owner/owner-dashboard';
import { Login } from './pages/auth/login/login';
import { Register } from './pages/auth/register/register';
import { AddProperty } from './pages/owner/add-property';
import { ownerGuard } from './core/guards/owner.guard';
import { renterGuard } from './core/guards/renter.guard';
import { RenterDashboard } from './pages/renter/renter-dashboard';
import { OwnerBookings } from './pages/owner/owner-bookings';
import { MessagesPage } from './pages/messages/messages';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'properties', component: Properties },
  { path: 'properties/:id', component: PropertyDetail },
  { path: 'owner', component: OwnerDashboard, canActivate: [ownerGuard] },
  { path: 'owner/bookings', component: OwnerBookings, canActivate: [ownerGuard] },
  { path: 'add-property', component: AddProperty, canActivate: [ownerGuard] },
  { path: 'my-bookings', component: RenterDashboard, canActivate: [renterGuard] },
  { path: 'messages', component: MessagesPage, canActivate: [authGuard] },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: '**', redirectTo: '' }
];
