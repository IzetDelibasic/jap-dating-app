import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Member } from '../../core/models/member';
import { Photo } from '../../core/models/photo';
import { PaginatedResult } from '../../core/models/pagination';
import { UserParams } from '../../core/models/userParams';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AccountService } from '../../core/services/account.service';
import {
  setPaginatedResponse,
  setPaginationHeaders,
} from '../../core/services/paginationHelper';
import { USER_API } from '../../core/constants/servicesConstants/userServiceConstant';
import { PHOTOS_API } from '../../core/constants/servicesConstants/photoServiceConstant';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);

  memberCache = new Map();
  user = this.accountService.currentUser();
  userParams = signal<UserParams>(new UserParams(this.user));
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);

  resetUserParams() {
    this.userParams.set(new UserParams(this.user));
  }

  getMembers(): Observable<Member[]> {
    const cacheKey = Object.values(this.userParams()).join('-');
    const cachedResponse = this.memberCache.get(cacheKey);

    if (cachedResponse) {
      setPaginatedResponse(cachedResponse, this.paginatedResult);
      return of(cachedResponse.body ?? []);
    }

    let params = setPaginationHeaders(
      this.userParams().pageNumber,
      this.userParams().pageSize
    );

    params = params.append('minAge', this.userParams().minAge.toString());
    params = params.append('maxAge', this.userParams().maxAge.toString());
    params = params.append('gender', this.userParams().gender);
    params = params.append('orderBy', this.userParams().orderBy);

    return this.http
      .get<Member[]>(environment.apiBaseUrl + USER_API.BASE, {
        observe: 'response',
        params,
      })
      .pipe(
        tap((response) => {
          this.memberCache.set(cacheKey, response);
          setPaginatedResponse(response, this.paginatedResult);
        }),
        map((response) => response.body ?? []),
        catchError(this.handleError<Member[]>('getMembers', []))
      );
  }

  getMember(username: string): Observable<Member> {
    const member: Member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.body), [])
      .find((m: Member) => m.userName === username);

    if (member) return of(member);

    return this.http
      .get<Member>(environment.apiBaseUrl + USER_API.BY_USERNAME(username))
      .pipe(
        catchError(this.handleError<Member>(`getMember username=${username}`))
      );
  }

  updateMember(member: Member): Observable<any> {
    return this.http
      .put(environment.apiBaseUrl + USER_API.UPDATE, member)
      .pipe(catchError(this.handleError<any>('updateMember')));
  }

  getTags(): Observable<{ id: number; name: string }[]> {
    return this.http
      .get<{ id: number; name: string }[]>(
        environment.apiBaseUrl + PHOTOS_API.GET_TAGS
      )
      .pipe(
        catchError(
          this.handleError<{ id: number; name: string }[]>('getTags', [])
        )
      );
  }

  getTagsForPhoto(photoId: number): Observable<string[]> {
    return this.http
      .get<string[]>(
        environment.apiBaseUrl + PHOTOS_API.GET_TAGS_FOR_PHOTO(photoId)
      )
      .pipe(catchError(this.handleError<string[]>('getTagsForPhoto', [])));
  }

  getPhotosByTag(tag: string): Observable<Photo[]> {
    return this.http
      .get<Photo[]>(environment.apiBaseUrl + PHOTOS_API.GET_PHOTOS_BY_TAG(tag))
      .pipe(catchError(this.handleError<Photo[]>('getPhotosByTag', [])));
  }

  setMainPhoto(photo: Photo): Observable<any> {
    return this.http
      .put(environment.apiBaseUrl + USER_API.SET_MAIN_PHOTO(photo.id), {})
      .pipe(catchError(this.handleError<any>('setMainPhoto')));
  }

  deletePhoto(photo: Photo): Observable<any> {
    return this.http
      .delete(environment.apiBaseUrl + USER_API.DELETE_PHOTO(photo.id))
      .pipe(catchError(this.handleError<any>('deletePhoto')));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return of(result as T);
    };
  }
}
