// -Angular-
import { FormsModule } from '@angular/forms';
import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { NgClass, TitleCasePipe } from '@angular/common';
// -Services-
import { AccountService } from '../../../core/services/account.service';
// -Ngx-
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrService } from 'ngx-toastr';
// -Directives-
import { HasRoleDirective } from '../../directives/has-role.directive';

@Component({
  selector: 'app-navbar',
  imports: [
    FormsModule,
    BsDropdownModule,
    RouterLink,
    RouterLinkActive,
    TitleCasePipe,
    HasRoleDirective,
    NgClass,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  accountService = inject(AccountService);
  private router = inject(Router);
  private toastr = inject(ToastrService);
  model: any = {};
  isNavbarOpen: boolean = false;

  toggleNavbar() {
    this.isNavbarOpen = !this.isNavbarOpen;
  }

  login() {
    if (!this.model.username || !this.model.password) {
      this.toastr.error('Username and password are required');
      return;
    }

    this.accountService.login(this.model).subscribe({
      next: () => this.router.navigateByUrl('/members'),
      error: (error) => this.handleError(error),
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

  private handleError(error: any): void {
    this.toastr.error(error.error || 'An unexpected error occurred');
  }
}
