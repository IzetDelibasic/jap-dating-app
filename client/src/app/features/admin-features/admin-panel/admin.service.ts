import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { User } from '../../../core/models/user';
import { Photo } from '../../../core/models/photo';
import { ADMIN_API } from '../../../core/constants/adminServiceConstant';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private http = inject(HttpClient);

  getUserWithRoles() {
    return this.http.get<User[]>(
      environment.apiBaseUrl + ADMIN_API.USERS_WITH_ROLES
    );
  }

  updateUserRoles(username: string, roles: string[]) {
    return this.http.post<string[]>(
      environment.apiBaseUrl + ADMIN_API.EDIT_ROLES(username, roles),
      {}
    );
  }

  getPhotosForApproval() {
    return this.http.get<Photo[]>(
      environment.apiBaseUrl + ADMIN_API.PHOTOS_TO_MODERATE
    );
  }

  approvePhoto(photoId: number) {
    return this.http.post(
      environment.apiBaseUrl + ADMIN_API.APPROVE_PHOTO(photoId),
      {}
    );
  }

  rejectPhoto(photoId: number) {
    return this.http.post(
      environment.apiBaseUrl + ADMIN_API.REJECT_PHOTO(photoId),
      {}
    );
  }
}
