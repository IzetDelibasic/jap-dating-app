// -Angular-
import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
// -Ngx-
import { ToastrService } from 'ngx-toastr';
// -Services-
import { AccountService } from '../../../core/services/account.service';
// -Components-
import { TextInputComponent } from '../../../shared/components/forms/text-input/text-input.component';

@Component({
  selector: 'app-login-form',
  imports: [ReactiveFormsModule, TextInputComponent, RouterLink],
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css'],
})
export class LoginFormComponent {
  accountService = inject(AccountService);
  private router = inject(Router);
  private toastr = inject(ToastrService);
  loginForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  login() {
    if (this.loginForm.valid) {
      this.accountService.login(this.loginForm.value).subscribe({
        next: () => {
          this.router.navigateByUrl('/members');
        },
        error: (error) => this.toastr.error(error.error),
      });
    }
  }
}
