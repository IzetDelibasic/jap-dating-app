import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { User } from '../models/user';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root',
})
export class AuthStoreService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);

  currentUser$: Observable<User | null> =
    this.currentUserSubject.asObservable();
  isLoggedIn$: Observable<boolean> = this.isLoggedInSubject.asObservable();

  constructor(private accountService: AccountService) {
    const user = this.accountService.currentUser();
    if (user) {
      this.currentUserSubject.next(user);
      this.isLoggedInSubject.next(true);
    } else {
      this.currentUserSubject.next(null);
      this.isLoggedInSubject.next(false);
    }
  }

  login(model: any): Observable<User | null> {
    return this.accountService.login(model).pipe(
      map((user) => {
        if (user) {
          this.currentUserSubject.next(user);
          this.isLoggedInSubject.next(true);
        }
        return user;
      })
    );
  }

  logout(): void {
    this.accountService.logout();
    this.currentUserSubject.next(null);
    this.isLoggedInSubject.next(false);
  }
}
