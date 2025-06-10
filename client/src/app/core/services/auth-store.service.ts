import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { User } from '../models/user';
import { AccountService } from './account.service';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root',
})
export class AuthStoreService {
  private presenceService = inject(PresenceService);
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);

  currentUser$: Observable<User | null> =
    this.currentUserSubject.asObservable();
  isLoggedIn$: Observable<boolean> = this.isLoggedInSubject.asObservable();

  constructor(private accountService: AccountService) {
    this.rehydrateUser();
  }

  login(model: any): Observable<User | null> {
    return this.accountService.login(model).pipe(
      map((user) => {
        if (user) {
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

  logout(): void {
    this.clearCurrentUser();
    this.presenceService.stopHubConnection();
  }

  hasRole(roles: string[]): boolean {
    const user = this.currentUserSubject.value;
    if (user && user.token) {
      const decodedToken = this.decodeToken(user.token);
      const userRoles = Array.isArray(decodedToken.role)
        ? decodedToken.role
        : [decodedToken.role];
      return roles.some((role) => userRoles.includes(role));
    }
    return false;
  }

  isAuthenticated(): boolean {
    const user = this.currentUserSubject.value;
    const isAuthenticated = !!user && !this.isTokenExpired(user.token);
    return isAuthenticated;
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  updateCurrentUser(user: User): void {
    this.setCurrentUser(user);
  }

  private setCurrentUser(user: User): void {
    this.saveUserToLocalStorage(user);
    this.currentUserSubject.next(user);
    this.isLoggedInSubject.next(true);
    this.presenceService.createHubConnection(user);
  }

  private clearCurrentUser(): void {
    this.removeUserFromLocalStorage();
    this.currentUserSubject.next(null);
    this.isLoggedInSubject.next(false);
  }

  private rehydrateUser(): void {
    console.log('rehydrateUser called');
    const userJson = localStorage.getItem('user');
    if (userJson) {
      const user = JSON.parse(userJson);
      this.setCurrentUser(user);
    } else {
      console.error('No user found in localStorage');
    }
  }

  private saveUserToLocalStorage(user: User): void {
    localStorage.setItem('user', JSON.stringify(user));
  }

  private removeUserFromLocalStorage(): void {
    localStorage.removeItem('user');
  }

  private decodeToken(token: string): any {
    return JSON.parse(atob(token.split('.')[1]));
  }

  private isTokenExpired(token: string): boolean {
    const decodedToken = this.decodeToken(token);
    const expiration = decodedToken.exp * 1000;
    return Date.now() > expiration;
  }
}
