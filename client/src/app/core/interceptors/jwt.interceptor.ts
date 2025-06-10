import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthStoreService } from '../services/auth-store.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authStore = inject(AuthStoreService);

  if (authStore.getCurrentUser()) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${authStore.getCurrentUser()?.token}`,
      },
    });
  }

  return next(req);
};
