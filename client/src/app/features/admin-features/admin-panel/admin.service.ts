import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { User } from '../../../core/models/user';
import { Photo } from '../../../core/models/photo';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { PHOTOS_API } from '../../../core/constants/servicesConstants/photoServiceConstant';
import { USER_API } from '../../../core/constants/servicesConstants/userServiceConstant';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private http = inject(HttpClient);

  getUserWithRoles(): Observable<User[]> {
    return this.http
      .get<User[]>(environment.apiBaseUrl + USER_API.USERS_WITH_ROLES)
      .pipe(catchError(this.handleError<User[]>('getUserWithRoles', [])));
  }

  updateUserRoles(username: string, roles: string[]): Observable<string[]> {
    return this.http
      .post<string[]>(
        environment.apiBaseUrl + USER_API.EDIT_ROLES(username, roles),
        {}
      )
      .pipe(catchError(this.handleError<string[]>('updateUserRoles', [])));
  }

  getPhotosForApproval(): Observable<Photo[]> {
    return this.http
      .get<Photo[]>(environment.apiBaseUrl + PHOTOS_API.PHOTOS_TO_MODERATE)
      .pipe(catchError(this.handleError<Photo[]>('getPhotosForApproval', [])));
  }

  approvePhoto(photoId: number): Observable<any> {
    return this.http
      .post(environment.apiBaseUrl + PHOTOS_API.APPROVE_PHOTO(photoId), {})
      .pipe(catchError(this.handleError('approvePhoto')));
  }

  rejectPhoto(photoId: number): Observable<any> {
    return this.http
      .post(environment.apiBaseUrl + PHOTOS_API.REJECT_PHOTO(photoId), {})
      .pipe(catchError(this.handleError('rejectPhoto')));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
}
