import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  isLoading = signal<boolean>(false);
  errorMessage = signal<string | null>(null);

  // Form validation grouping aligned directly with your .NET RegisterUserCommand
  registerForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    username: ['', [Validators.required, Validators.minLength(3)]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    const command = this.registerForm.getRawValue();

    this.authService.register(command).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.router.navigate(['/dashboard']); // Successfully registered and logged in -> board dashboard
      },
      error: (err) => {
        this.isLoading.set(false);
        // Safely extract the server-side validation error array or string
        this.errorMessage.set(err.error?.message || 'An unexpected error occurred during registration.');
      }
    });
  }
}