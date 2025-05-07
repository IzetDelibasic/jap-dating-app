// -Angular-
import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
// -Service-
import { AccountService } from '../services/account.service';
// -NgxToastr-
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  if (accountService.currentUser()) {
    return true;
  } else {
    toastr.error('You shall not pass!');
    return false;
  }
};
