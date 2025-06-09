import { FormsModule } from '@angular/forms';
import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule, NgClass, TitleCasePipe } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { AuthStoreService } from '../../../core/services/auth-store.service';
import { DEFAULT_PHOTO_URL } from '../../../core/constants/contentConstants/imagesConstant';
import { HasRoleDirective } from '../../directives/has-role.directive';

@Component({
  selector: 'app-navbar',
  imports: [
    FormsModule,
    BsDropdownModule,
    RouterLink,
    RouterLinkActive,
    TitleCasePipe,
    NgClass,
    CommonModule,
    HasRoleDirective,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  private authStore = inject(AuthStoreService);
  private router = inject(Router);
  isNavbarOpen: boolean = false;
  defaultPhoto = DEFAULT_PHOTO_URL;

  isLoggedIn$ = this.authStore.isLoggedIn$;
  currentUser$ = this.authStore.currentUser$;

  toggleNavbar() {
    this.isNavbarOpen = !this.isNavbarOpen;
  }

  logout() {
    this.authStore.logout();
    this.router.navigateByUrl('/');
  }
}
