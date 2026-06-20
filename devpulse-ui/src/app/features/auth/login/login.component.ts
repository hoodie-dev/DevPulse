import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  // Reactive state management for loading indicators and API error feedback
  isLoading = signal<boolean>(false);
  errorMessage = signal<string | null>(null);

  // Highly typed form validation definition matching our LoginUserQuery schema
  loginForm = this.fb.nonNullable.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    const query = this.loginForm.getRawValue();

    this.authService.login(query).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.router.navigate(['/dashboard']); // Route straight into the Kanban board workspace upon success
      },
      error: (err) => {
        this.isLoading.set(false);
        // Safely extract the server-side validation message returned from our API middleware
        this.errorMessage.set(err.error?.message || 'An unexpected error occurred during login.');
      }
    });
  }
}