// -Angular-
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
// -Service-
import { AccountService } from '../services/account.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);

  if (accountService.currentUser()) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accountService.currentUser()?.token}`,
      },
    });
  }

  return next(req);
};
