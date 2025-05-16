import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';

export const adminGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  if (accountService.hasRole(['Admin', 'Moderator'])) {
    return true;
  } else {
    toastr.error('Access denied: Admins or Moderators only.');
    return false;
  }
};
