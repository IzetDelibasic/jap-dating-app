import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User } from '../models/user';
import { ACCOUNT_API } from '../constants/servicesConstants/accountServiceConstant';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);

  login(model: any) {
    return this.http
      .post<User>(environment.apiBaseUrl + ACCOUNT_API.LOGIN, model)
      .pipe(map((user) => user));
  }

  register(model: any) {
    return this.http
      .post<User>(environment.apiBaseUrl + ACCOUNT_API.REGISTER, model)
      .pipe(map((user) => user));
  }
}
