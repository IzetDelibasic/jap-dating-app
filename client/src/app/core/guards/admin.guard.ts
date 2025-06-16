import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthStoreService } from '../services/auth-store.service';

export const adminGuard: CanActivateFn = () => {
  const authStore = inject(AuthStoreService);
  const toastr = inject(ToastrService);

  if (authStore.hasRole(['Admin', 'Moderator'])) {
    return true;
  } else {
    toastr.error('Access denied: Admins or Moderators only.');
    return false;
  }
};
