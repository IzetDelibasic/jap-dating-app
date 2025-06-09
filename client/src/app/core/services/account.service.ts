import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { catchError, forkJoin, map, of } from 'rxjs';
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

  constructor() {
    this.rehydrateUser();
  }

  login(model: any) {
    return this.http
      .post<User>(environment.apiBaseUrl + ACCOUNT_API.LOGIN, model)
      .pipe(map((user) => this.handleUser(user)));
  }

  register(model: any) {
    return this.http
      .post<User>(environment.apiBaseUrl + ACCOUNT_API.REGISTER, model)
      .pipe(map((user) => this.handleUser(user)));
  }

  setCurrentUser(user: User) {
    this.saveUserToLocalStorage(user);
    this.currentUser.set(user);
    this.presenceService.createHubConnection(user);

    forkJoin({
      likeIds: this.likeService.getLikeIds().pipe(
        catchError((error) => {
          console.error('Error fetching like IDs:', error);
          return of([]);
        })
      ),
    }).subscribe(({ likeIds }) => {
      this.likeService.likeIds.set(likeIds);
    });
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

  private handleUser(user: User | null): User | null {
    if (user) {
      this.setCurrentUser(user);
    }
    return user;
  }

  private rehydrateUser(): void {
    const userJson = localStorage.getItem('user');
    if (userJson) {
      const user = JSON.parse(userJson);
      this.currentUser.set(user);
    }
  }
}
