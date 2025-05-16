import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User } from '../models/user';
import { LikesService } from './likes.service';
import { PresenceService } from './presence.service';
import { ACCOUNT_API } from '../constants/servicesConstants/accountServiceConstant';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  private likeService = inject(LikesService);
  private presenceService = inject(PresenceService);
  currentUser = signal<User | null>(null);

  roles = computed(() => {
    const user = this.currentUser();
    if (user && user.token) {
      const decodedToken = this.decodeToken(user.token);
      const role = decodedToken.role;
      return Array.isArray(role) ? role : [role];
    }
    return [];
  });

  login(model: any) {
    return this.http
      .post<User>(environment.apiBaseUrl + ACCOUNT_API.LOGIN, model)
      .pipe(
        map((user) => {
          if (user) {
            this.setCurrentUser(user);
          }
        })
      );
  }

  register(model: any) {
    return this.http
      .post<User>(environment.apiBaseUrl + ACCOUNT_API.REGISTER, model)
      .pipe(
        map((user) => {
          if (user) {
            this.setCurrentUser(user);
          }
          return user;
        })
      );
  }

  setCurrentUser(user: User) {
    this.saveUserToLocalStorage(user);
    this.currentUser.set(user);
    this.likeService.getLikeIds().subscribe({
      next: (ids) => {
        this.likeService.likeIds.set(ids);
      },
      error: (error) => {
        console.error('Error fetching like IDs:', error);
      },
    });
    this.presenceService.createHubConnection(user);
  }

  hasRole(roles: string[]): boolean {
    const userRoles = this.roles();
    return roles.some((role) => userRoles.includes(role));
  }

  isAuthenticated(): boolean {
    const user = this.currentUser();
    return !!user && !this.isTokenExpired(user.token);
  }

  logout() {
    this.removeUserFromLocalStorage();
    this.currentUser.set(null);
    this.presenceService.stopHubConnection();
  }

  private decodeToken(token: string): any {
    return JSON.parse(atob(token.split('.')[1]));
  }

  private saveUserToLocalStorage(user: User): void {
    localStorage.setItem('user', JSON.stringify(user));
  }

  private removeUserFromLocalStorage(): void {
    localStorage.removeItem('user');
  }

  private isTokenExpired(token: string): boolean {
    const decodedToken = this.decodeToken(token);
    const expiration = decodedToken.exp * 1000;
    return Date.now() > expiration;
  }
}
