import { catchError, of, tap } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { NgClass, TitleCasePipe } from '@angular/common';
import { AccountService } from '../../../core/services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrService } from 'ngx-toastr';
import { HasRoleDirective } from '../../directives/has-role.directive';
import { DEFAULT_PHOTO_URL } from '../../../core/constants/contentConstants/imagesConstant';

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
  public accountService = inject(AccountService);
  private router = inject(Router);
  private toastr = inject(ToastrService);
  model: any = {};
  isNavbarOpen: boolean = false;

  defaultPhoto = DEFAULT_PHOTO_URL;

  toggleNavbar() {
    this.isNavbarOpen = !this.isNavbarOpen;
  }

  login() {
    if (!this.model.username || !this.model.password) {
      this.toastr.error('Username and password are required');
      return;
    }

    this.accountService.login(this.model).pipe(
      tap(() => {this.router.navigateByUrl('/members'); }),
      catchError((error) =>{
        this.handleError(error);
        return of(null);
      })
    ).subscribe();
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

  private handleError(error: any): void {
    this.toastr.error(error.error || 'An unexpected error occurred');
  }
}
