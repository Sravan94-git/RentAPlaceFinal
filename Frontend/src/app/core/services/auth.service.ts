import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, tap } from 'rxjs';

interface LoginRequest {
  email: string;
  password: string;
}

interface LoginResponse {
  token: string;
  role: string;
  name: string;
  userId: number;
}

interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  role: 'Renter' | 'Owner';
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = 'http://localhost:5255/api/Auth';
  private readonly roleSubject = new BehaviorSubject<string | null>(localStorage.getItem('role'));
  readonly role$ = this.roleSubject.asObservable();

  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) {}

  login(data: LoginRequest) {
    return this.http.post<LoginResponse>(`${this.baseUrl}/login`, data).pipe(
      tap((res) => {
        localStorage.setItem('token', res.token);
        localStorage.setItem('role', res.role);
        localStorage.setItem('name', res.name);
        localStorage.setItem('userId', String(res.userId));
        this.roleSubject.next(res.role);
      })
    );
  }

  register(data: RegisterRequest) {
    return this.http.post<{ message: string }>(`${this.baseUrl}/register`, data);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getRole(): string | null {
    return localStorage.getItem('role');
  }

  getUserId(): number | null {
    const raw = localStorage.getItem('userId');
    return raw ? Number(raw) : null;
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  isOwner(): boolean {
    return this.getRole() === 'Owner';
  }

  isRenter(): boolean {
    return this.getRole() === 'Renter';
  }

  logout(redirect = true) {
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    localStorage.removeItem('name');
    localStorage.removeItem('userId');
    this.roleSubject.next(null);
    if (redirect) {
      this.router.navigate(['/login']);
    }
  }
}
