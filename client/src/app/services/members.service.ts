import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Member } from '../models/member';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);

  getMembers() {
    return this.http.get<Member[]>(environment.apiBaseUrl + 'user');
  }

  getMember(username: string) {
    return this.http.get<Member>(environment.apiBaseUrl + `/user/${username}`);
  }
}
