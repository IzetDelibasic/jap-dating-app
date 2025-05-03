import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private http = inject(HttpClient);

  getUserWithRoles() {
    return this.http.get<User[]>(
      environment.apiBaseUrl + 'admin/users-with-roles'
    );
  }

  updateUserRoles(username: string, roles: string[]) {
    return this.http.post<string[]>(
      environment.apiBaseUrl + `admin/edit-roles/${username}?roles=${roles}`,
      {}
    );
  }
}
