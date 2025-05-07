// -Angular-
import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
// -Service-
import { AccountService } from '../services/account.service';
// -NgxToastr-
import { ToastrService } from 'ngx-toastr';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  if (
    accountService.roles().includes('Admin') ||
    accountService.roles().includes('Moderator')
  ) {
    return true;
  } else {
    toastr.error('You cannot enter this area');
    return false;
  }
};
