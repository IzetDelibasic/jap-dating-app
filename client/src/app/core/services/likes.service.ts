import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Member } from '../models/member';
import { PaginatedResult } from '../models/pagination';
import { setPaginationHeaders } from './paginationHelper';
import { map, Observable, tap } from 'rxjs';
import { LIKES_API } from '../constants/servicesConstants/likesServiceConstant';

@Injectable({
  providedIn: 'root',
})
export class LikesService {
  private http = inject(HttpClient);
  likeIds = signal<number[]>([]);
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);

  toggleLike(targetId: number) {
    return this.http.post(
      `${environment.apiBaseUrl}${LIKES_API.TOGGLE(targetId)}`,
      {}
    );
  }

  getLikes(
    predicate: string,
    pageNumber: number,
    pageSize: number
  ): Observable<PaginatedResult<Member[]>> {
    let params = setPaginationHeaders(pageNumber, pageSize);
    params = params.append('predicate', predicate);

    return this.http
      .get<Member[]>(`${environment.apiBaseUrl}${LIKES_API.BASE}`, {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          const paginationHeader = response.headers.get('Pagination');
          const pagination = paginationHeader
            ? JSON.parse(paginationHeader)
            : null;

          if (!response.body || !pagination) {
            throw new Error('Missing pagination or response body');
          }

          return {
            items: response.body,
            pagination: pagination,
          } as PaginatedResult<Member[]>;
        }),
        tap((result) => this.paginatedResult.set(result))
      );
  }

  updateLikeIds(memberId: number, hasLiked: boolean): void {
    if (hasLiked) {
      this.likeIds.update((ids) => ids.filter((id) => id !== memberId));
    } else {
      this.likeIds.update((ids) => [...ids, memberId]);
    }
  }

  getLikeIds(): Observable<number[]> {
    return this.http.get<number[]>(
      `${environment.apiBaseUrl}${LIKES_API.LIST}`
    );
  }
}
