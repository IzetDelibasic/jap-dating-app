import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../environments/environment';
import { Member } from '../models/member';

@Injectable({
  providedIn: 'root',
})
export class LikesService {
  private http = inject(HttpClient);
  likeIds = signal<number[]>([]);

  toggleLike(targetId: number) {
    return this.http.post(`${environment.apiBaseUrl}likes/${targetId}`, {});
  }

  getLikes(predicate: string) {
    return this.http.get<Member[]>(
      `${environment.apiBaseUrl}likes?predicate=${predicate}`
    );
  }

  getLikeIds() {
    return this.http
      .get<number[]>(`${environment.apiBaseUrl}likes/list`)
      .subscribe({
        next: (ids) => this.likeIds.set(ids),
      });
  }
}
