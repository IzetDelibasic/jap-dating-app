import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Member } from '../../core/models/member';
import { Photo } from '../../core/models/photo';
import { PaginatedResult } from '../../core/models/pagination';
import { UserParams } from '../../core/models/userParams';
import { of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AccountService } from '../../core/services/account.service';
import {
  setPaginatedResponse,
  setPaginationHeaders,
} from '../../core/services/paginationHelper';
import { MEMBERS_API } from '../../core/constants/membersServiceConstant';

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

    if (response) return setPaginatedResponse(response, this.paginatedResult);

    let params = setPaginationHeaders(
      this.userParams().pageNumber,
      this.userParams().pageSize
    );

    params = params.append('minAge', this.userParams().minAge);
    params = params.append('maxAge', this.userParams().maxAge);
    params = params.append('gender', this.userParams().gender);
    params = params.append('orderBy', this.userParams().orderBy);

    return this.http
      .get<Member[]>(environment.apiBaseUrl + MEMBERS_API.BASE, {
        observe: 'response',
        params,
      })
      .subscribe({
        next: (response) => {
          setPaginatedResponse(response, this.paginatedResult);
          this.memberCache.set(
            Object.values(this.userParams()).join('-'),
            response
          );
        },
      });
  }

  getMember(username: string) {
    const member: Member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.body), [])
      .find((m: Member) => m.userName === username);

    if (member) return of(member);

    return this.http.get<Member>(
      environment.apiBaseUrl + MEMBERS_API.BY_USERNAME(username)
    );
  }

  updateMember(member: Member) {
    return this.http.put(environment.apiBaseUrl + MEMBERS_API.UPDATE, member);
  }

  setMainPhoto(photo: Photo) {
    return this.http.put(
      environment.apiBaseUrl + MEMBERS_API.SET_MAIN_PHOTO(photo.id),
      {}
    );
  }

  deletePhoto(photo: Photo) {
    return this.http.delete(
      environment.apiBaseUrl + MEMBERS_API.DELETE_PHOTO(photo.id)
    );
  }
}
