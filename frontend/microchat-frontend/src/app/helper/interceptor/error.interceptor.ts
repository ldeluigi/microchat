import {
  HttpErrorResponse, HttpEvent, HttpHandler,
  HttpInterceptor, HttpRequest
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { AccountService } from '../../services/account.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');

  constructor(private accountService: AccountService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(catchError(err => {
      if (err instanceof HttpErrorResponse && err.status === 401 && this.accountService.user !== null) {
        // refresh the token
        return this.handle401Error(request, next);
      }
      // console.log(err);

      try {
      const error =
        err.error.errors[0] ?
          err.error.errors[0].detail : (err.statusText ? err.statusText : "Unknown Error");
        return throwError(() => new Error(error));
      } catch (ex) {
        return throwError(() => new Error("Unknown error occured"));
      }
    }));
  }

  private addToken(request: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  private handle401Error(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next('');
      return this.accountService.refreshToken()
        .pipe(catchError(err => {
          this.accountService.logout();
          return throwError(() => new Error('Error in token refresh'));
        }))
        .pipe(
          switchMap(t => {
            this.isRefreshing = false;
            this.refreshTokenSubject.next(t.accessToken);
            return next.handle(this.addToken(request, t.accessToken));
          })
        );
    } else {
      return this.refreshTokenSubject.pipe(
        filter(token => token.length > 0),
        take(1),
        switchMap(jwt => {
          return next.handle(this.addToken(request, jwt));
        }));
    }
  }
}
