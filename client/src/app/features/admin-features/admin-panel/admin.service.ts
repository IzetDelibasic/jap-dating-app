// -Angular-
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
// -Environment-
import { environment } from '../../../../environments/environment';
// -Models-
import { User } from '../../../shared/models/user';
import { Photo } from '../../../shared/models/photo';

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

  getPhotosForApproval() {
    return this.http.get<Photo[]>(
      environment.apiBaseUrl + 'admin/photos-to-moderate'
    );
  }

  approvePhoto(photoId: number) {
    return this.http.post(
      environment.apiBaseUrl + `admin/approve-photo/${photoId}`,
      {}
    );
  }

  rejectPhoto(photoId: number) {
    return this.http.post(
      environment.apiBaseUrl + `admin/reject-photo/${photoId}`,
      {}
    );
  }
}
