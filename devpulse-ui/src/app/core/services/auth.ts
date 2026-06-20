import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

// Match the exact AuthResponse contract returned by our .NET MediatR backend
export interface AuthResponse {
  id: string;
  username: string;
  email: string;
  token: string;
  expiration: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private readonly API_URL = 'http://localhost:5165/api/auth'; // Ensure this matches your .NET launch settings port

  // 1. Core Reactive State tracking the current user profile metadata
  // We initialize it by checking if a token passport is already saved in localStorage
  #currentUser = signal<AuthResponse | null>(this.getStoredAuth());

  // 2. Public read-only Signals exposed to our components
  currentUser = this.#currentUser.asReadonly();
  isAuthenticated = computed(() => this.#currentUser() !== null);

  register(command: any): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.API_URL}/register`, command).pipe(
      tap(response => this.handleAuthentication(response))
    );
  }

  login(query: any): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.API_URL}/login`, query).pipe(
      tap(response => this.handleAuthentication(response))
    );
  }

  logout(): void {
    localStorage.removeItem('devpulse_auth');
    this.#currentUser.set(null);
    this.router.navigate(['/login']);
  }

  // Token helper to easily pull the raw JWT out for our future Interceptor
  getToken(): string | null {
    return this.#currentUser()?.token || null;
  }

  private handleAuthentication(response: AuthResponse): void {
    localStorage.setItem('devpulse_auth', JSON.stringify(response));
    this.#currentUser.set(response);
  }

  private getStoredAuth(): AuthResponse | null {
    const stored = localStorage.getItem('devpulse_auth');
    if (!stored) return null;
    
    try {
      const authData: AuthResponse = JSON.parse(stored);
      // Optional: Add a client-side expiration check against authData.expiration here
      return authData;
    } catch {
      return null;
    }
  }
}