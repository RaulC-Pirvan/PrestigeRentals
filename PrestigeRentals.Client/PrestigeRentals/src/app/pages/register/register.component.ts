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

  idCardFile: File | null = null;
  idCardPreviewUrl: string | null = null;

  ocrResult: boolean | null = null; // true if over 18, false if not, null if not verified

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
    if (this.step === 1 && this.userForm.valid) {
      this.step = 2;
    } else if (this.step === 2 && this.credentialsForm.valid) {
      if (
        this.credentialsForm.value.password !==
        this.credentialsForm.value.repeatPassword
      ) {
        alert('Passwords do not match');
        return;
      }
      this.step = 3;
    }
  }

  onIdCardSelected(event: any) {
    const file = event.target.files[0];
    if (!file || !this.tempUserId) return;

    this.idCardFile = file;

    const reader = new FileReader();
    reader.onload = () => (this.idCardPreviewUrl = reader.result as string);
    reader.readAsDataURL(file);

    const formData = new FormData();
    formData.append('image', file, file.name);

    // 1. Upload imagine buletin
    this.userService
      .uploadUserIdCardImage(this.tempUserId, formData)
      .subscribe({
        next: () => {
          console.log('✅ ID card uploaded. Starting OCR...');

          // 2. Verificare CNP
          this.userService.verifyCnp(this.tempUserId!).subscribe({
            next: (res: boolean) => {
              this.ocrResult = res;
              if (res) {
                console.log('✅ Over 18, redirecting...');
                this.router.navigate(['/register-success']);
              } else {
                alert('❌ You must be over 18 to register.');
              }
            },
            error: () => {
              alert('❌ Failed to verify age. Please try again.');
            },
          });
        },
        error: () => {
          alert('❌ Failed to upload ID card.');
        },
      });
  }

  previousStep() {
    if (this.step > 1) this.step--;
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

  tempUserId: string | null = null;

  async submit() {
    if (
      !this.credentialsForm.valid ||
      this.credentialsForm.value.password !==
        this.credentialsForm.value.repeatPassword
    ) {
      alert('Please fix the errors before submitting.');
      return;
    }

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
        this.tempUserId = userId;

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
        this.step = 3; // Move to OCR step
      },
      error: () => {
        this.isLoading = false;
        alert('Registration failed. Please try again.');
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
