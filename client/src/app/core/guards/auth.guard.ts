import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthStoreService } from '../services/auth-store.service';

export const authGuard: CanActivateFn = () => {
  const authStore = inject(AuthStoreService);
  const toastr = inject(ToastrService);

  if (authStore.isAuthenticated()) {
    return true;
  } else {
    toastr.error('You shall not pass!');
    return false;
  }
};
