// -Angular -
import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
// -Models-
import { Member } from '../../shared/models/member';
import { Photo } from '../../shared/models/photo';
import { PaginatedResult } from '../../shared/models/pagination';
import { UserParams } from '../../shared/models/userParams';
// -Rxjs-
import { of } from 'rxjs';
// -Environment-
import { environment } from '../../../environments/environment';
// -Service-
import { AccountService } from '../../core/services/account.service';
// -PaginationHelper-
import {
  setPaginatedResponse,
  setPaginationHeaders,
} from '../../core/services/paginationHelper';

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

  getMembers() {
    const response = this.memberCache.get(
      Object.values(this.userParams()).join('-')
    );

    if (response) {
      setPaginatedResponse(response, this.paginatedResult);
      return;
    }

    let params = setPaginationHeaders(
      this.userParams().pageNumber,
      this.userParams().pageSize
    );

    params = params.append('minAge', this.userParams().minAge);
    params = params.append('maxAge', this.userParams().maxAge);
    params = params.append('gender', this.userParams().gender);
    params = params.append('orderBy', this.userParams().orderBy);

    this.http
      .get<Member[]>(environment.apiBaseUrl + 'user', {
        observe: 'response',
        params,
      })
      .subscribe({
        next: (response) => {
          const paginationHeader = response.headers.get('Pagination');
          const pagination = paginationHeader
            ? JSON.parse(paginationHeader)
            : null;

          if (response.body && pagination) {
            const paginatedResult: PaginatedResult<Member[]> = {
              items: response.body,
              pagination: pagination,
            };

            setPaginatedResponse(paginatedResult, this.paginatedResult);
            this.memberCache.set(
              Object.values(this.userParams()).join('-'),
              paginatedResult
            );
          } else {
            console.error('Response body or pagination is missing.');
          }
        },
        error: (err) => {
          console.error('Error fetching members:', err);
        },
      });
  }

  getMember(username: string) {
    const members = [...this.memberCache.values()].reduce((arr, elem) => {
      if (elem && elem.body) {
        return arr.concat(elem.body);
      }
      return arr;
    }, []);

    const member = members.find((m: Member) => m.userName === username);

    if (member) return of(member);

    return this.http.get<Member>(environment.apiBaseUrl + `user/${username}`);
  }

  updateMember(member: Member) {
    return this.http.put(environment.apiBaseUrl + 'user', member);
  }

  setMainPhoto(photo: Photo) {
    return this.http.put(
      environment.apiBaseUrl + `user/set-main-photo/${photo.id}`,
      {}
    );
  }

  deletePhoto(photo: Photo) {
    return this.http.delete(
      environment.apiBaseUrl + `user/delete-photo/${photo.id}`
    );
  }
}
