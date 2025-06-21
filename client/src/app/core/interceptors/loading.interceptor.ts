import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { delay, finalize } from 'rxjs';
import { LoadingService } from '../services/loading.service';
import { MESSAGES_API } from '../constants/servicesConstants/messagesServiceConstant';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);

  if (req.url.includes(MESSAGES_API.BASE)) {
    return next(req).pipe(delay(1000));
  }

  loadingService.setLoading(true);

  return next(req).pipe(finalize(() => loadingService.setLoading(false)));
};
