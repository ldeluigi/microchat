import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { first, map } from 'rxjs/operators';
import { LoggedUser } from 'src/model/LoggedUser';
import { LogLevel } from 'src/model/logLevel';
import { Response } from 'src/model/serverResponse';
import { TokenRefresh } from 'src/model/tokenRefresh';
import { UserRegistration, UserRegistrationResponse } from 'src/model/UserRegistration';
import { ApiURLService } from './api-url.service';
import { LogService } from './log.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private userSubject: BehaviorSubject<LoggedUser | null>;
  private userLocalStorage = 'user';
  public user: Observable<LoggedUser | null>;

  public authVersion: string = "?version=1.0";

  constructor(
    private router: Router,
    private http: HttpClient,
    private logService: LogService,
    private apiURL: ApiURLService
  ) {
    this.userSubject = new BehaviorSubject<LoggedUser | null>(this.extractUser());
    this.user = this.userSubject.asObservable();
  }

  public get userValue(): LoggedUser | null {
    const eu = this.extractUser();
    if (eu !== this.userSubject.value && eu !== null) {
      this.userSubject.next(eu);
    }
    return this.userSubject.value;
  }

  private saveUser(user: LoggedUser | null): void {
    if (user === null) {
      localStorage.removeItem(this.userLocalStorage);
    } else {
      localStorage.setItem(this.userLocalStorage, JSON.stringify(user));
    }
    this.userSubject.next(user);
  }

  private extractUser(): LoggedUser | null {
    const u = localStorage.getItem(this.userLocalStorage);
    if (u !== null) {
      return JSON.parse(u);
    }
    return null;
  }

  login(username: string, password: string): Observable<LoggedUser | null> {
    // console.log({ username: username, password: password });
    return this.http.post<Response<LoggedUser | null>>(`${this.apiURL.authApiUrl}/token/login${this.authVersion}`, { username: username, password: password })
      .pipe(map(u => {
        this.saveUser(u.data);
        this.logService.messageSnackBar('Logged in properly');
        return u.data;
      }));
  }

  logout(message?: string): void {
    // remove user from local storage and set current user to null
    this.saveUser(null);
    if (message) { // User is logout for some unkown reason
      this.logService.infoSnackBar(message);
    } else { // User ask for logout
      this.logService.messageSnackBar('Logged out properly');
    }
    this.router.navigate(['/']);
  }

  register(user: UserRegistration): Observable<UserRegistrationResponse> {
    return this.http.post<Response<UserRegistrationResponse>>(`${this.apiURL.authApiUrl}/accounts${this.authVersion}`, user)
      .pipe(map(u => u.data));
  }

  async updateEmail(newEmail: string): Promise<void> {
    const data = { email: newEmail };
    await this.update(data);
    return;
  }

  async updatePassword(oldPassword: string, newPassword: string): Promise<void> {
    const data = { oldPassword, password: newPassword };
    await this.update(data);
    return;
  }

  // tslint:disable-next-line: no-any
  private async update(data: any): Promise<void> {
    const user = this.extractUser();
    if (user === null) {
      this.logService.log('No user logged while try to update user info', LogLevel.Error);
      throw Error('Invalid user');
    }
    this.http.put(`${this.apiURL.authApiUrl}/accounts/${user.userId}${this.authVersion}`, data)
      .pipe(map(_ => {
        this.logService.messageSnackBar('Data updated correctly');
        return;
      })).pipe(first()).subscribe();
  }

  refreshToken(): Observable<TokenRefresh> {
    const user: LoggedUser | null = this.userValue;
    if (user === null) {
      this.logService.log('No user logged while try to refresh token', LogLevel.Error);
      return throwError(() => new Error('No user logged'));
    }
    return this.http.post<Response<TokenRefresh>>(`${this.apiURL.authApiUrl}/token/refresh${this.authVersion}`, { token: user.accessToken, refresh: user.refreshToken })
      .pipe(map(a => {
        user.accessToken = a.data.token;
        user.refreshToken = a.data.refresh;
        this.saveUser(user);
        return a.data;
      }));
  }

  deleteUser(): Observable<LoggedUser> {
    const user: LoggedUser | null = this.userValue;
    if (user === null) {
      this.logService.log('No user logged while try to delete user', LogLevel.Error);
      return throwError(() => new Error('No user logged'));
    }
    return this.http.delete<Response<LoggedUser>>(`${this.apiURL.authApiUrl}/accounts/${user.userId}${this.authVersion}`)
      .pipe(map(a => {
        this.saveUser(null);
        return a.data;
      }));
  }
}
