import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Photo } from '../../core/models/photo';
import { environment } from '../../../environments/environment';
import { PHOTOS_API } from '../../core/constants/servicesConstants/photoServiceConstant';
import { catchError, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PhotosFeedService {
  private http = inject(HttpClient);

  getPhotosByUserId(userId: number): Observable<Photo[]> {
    return this.http
      .get<Photo[]>(
        `${environment.apiBaseUrl}${PHOTOS_API.GET_PHOTOS_BY_USER(userId)}`
      )
      .pipe(
        catchError((error) => {
          console.error(error);
          return of([]);
        })
      );
  }
}
