import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../services/auth.service';

import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  step = 1;
  isLoading = false;

  userForm: FormGroup;
  credentialsForm: FormGroup;

  selectedImageFile: File | null = null;
  imagePreviewUrl: string | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private userService: UserService
  ) {
    this.userForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      profileImage: [null],
    });

    this.credentialsForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      repeatPassword: ['', Validators.required],
      agreeToTerms: [false, Validators.requiredTrue],
    });
  }

  nextStep() {
    if (this.userForm.valid) {
      this.step = 2;
    }
  }

  previousStep() {
    this.step = 1;
  }

  onImageSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedImageFile = file;
      const reader = new FileReader();
      reader.onload = () => (this.imagePreviewUrl = reader.result as string);
      reader.readAsDataURL(file);
    }
  }

  async submit() {
    if (
      !this.credentialsForm.valid ||
      this.credentialsForm.value.password !==
        this.credentialsForm.value.repeatPassword
    )
      return;

    this.isLoading = true;

    const registerPayload = {
      firstName: this.userForm.value.firstName,
      lastName: this.userForm.value.lastName,
      dateOfBirth: this.userForm.value.dateOfBirth,
      email: this.credentialsForm.value.email,
      password: this.credentialsForm.value.password,
    };

    this.authService.register(registerPayload).subscribe({
      next: async (res: any) => {
        const token = res.token || res.Token;
        const userId = this.getUserIdFromToken(token);

        if (this.selectedImageFile) {
          const formData = new FormData();
          formData.append(
            'image',
            this.selectedImageFile,
            this.selectedImageFile.name
          );

          await this.userService.uploadUserImage(userId, formData).toPromise();
        } else {
          await this.userService.uploadDefaultUserImage(userId).toPromise();
        }

        this.isLoading = false;
        this.router.navigate(['/register-success']);
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  getUserIdFromToken(token: string): string {
    if (!token) return '';

    const payload = token.split('.')[1];
    const decodedPayload = atob(payload);
    const obj = JSON.parse(decodedPayload);
    return obj['userId'] || obj['sub'] || '';
  }
}
