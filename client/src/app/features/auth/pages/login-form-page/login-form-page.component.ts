import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TextInputComponent } from '../../../../shared/components/forms/text-input/text-input.component';
import { HERO_VIDEO_URL } from '../../../../core/constants/contentConstants/videosConstant';
import { catchError, of, tap } from 'rxjs';
import { AuthStoreService } from '../../../../core/services/auth-store.service';

@Component({
  selector: 'app-login-form',
  imports: [ReactiveFormsModule, TextInputComponent, RouterLink],
  templateUrl: './login-form-page.component.html',
  styleUrls: ['./login-form-page.component.css'],
})
export class LoginFormPageComponent {
  private router = inject(Router);
  private toastr = inject(ToastrService);
  private authStore = inject(AuthStoreService);
  loginForm: FormGroup;
  heroVideo = HERO_VIDEO_URL;

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  login() {
    if (this.loginForm.valid) {
      this.authStore
        .login(this.loginForm.value)
        .pipe(
          tap(() => {
            this.router.navigateByUrl('/board');
          }),
          catchError((error) => {
            this.toastr.error(error.error);
            return of(null);
          })
        )
        .subscribe();
    }
  }
}
