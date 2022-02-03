import {
  HttpEvent, HttpHandler,

  HttpInterceptor, HttpRequest
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiURLService } from 'src/app/services/api-url.service';
import { AccountService } from '../../services/account.service';

@Injectable()
export class JWTInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService, private apiURL: ApiURLService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // add auth header with jwt if user is logged in and request is to the api url
    const user = this.accountService.userValue;
    if (user === null) {
      return next.handle(request);
    }
    const isLoggedIn = user && user.accessToken;
    const isApiUrl = request.url.startsWith(this.apiURL.baseApiUrl);
    if (isLoggedIn && isApiUrl) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${user.accessToken}`
        }
      });
    }

    return next.handle(request);
  }
}
