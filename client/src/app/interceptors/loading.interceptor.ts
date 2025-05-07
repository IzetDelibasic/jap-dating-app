// -Angular-
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
// -Service-
import { BusyService } from '../services/busy.service';
// -Rxjs-
import { delay, finalize } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const busyService = inject(BusyService);

  busyService.busy();

  return next(req).pipe(
    delay(1000),
    finalize(() => {
      busyService.idle();
    })
  );
};
