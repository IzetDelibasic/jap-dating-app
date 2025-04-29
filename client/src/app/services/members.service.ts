import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../environments/environment';
import { Member } from '../models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../models/photo';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);
  members = signal<Member[]>([]);

  getMembers() {
    return this.http.get<Member[]>(environment.apiBaseUrl + 'user').subscribe({
      next: (members) => this.members.set(members),
    });
  }

  getMember(username: string) {
    const member = this.members().find((x) => x.userName === username);
    if (member !== undefined) return of(member);

    return this.http.get<Member>(environment.apiBaseUrl + `user/${username}`);
  }

  updateMember(member: Member) {
    return this.http.put(environment.apiBaseUrl + 'user', member).pipe(
      tap(() => {
        this.members.update((members) =>
          members.map((m) => (m.userName === member.userName ? member : m))
        );
      })
    );
  }

  setMainPhoto(photo: Photo) {
    return this.http
      .put(environment.apiBaseUrl + `user/set-main-photo/${photo.id}`, {})
      .pipe(
        tap(() => {
          this.members.update((members) =>
            members.map((m) => {
              if (m.photos.includes(photo)) {
                m.photoUrl = photo.url;
              }
              return m;
            })
          );
        })
      );
  }
}
