import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from '@angular/common/http';
import { map, Observable } from "rxjs";
import { Response } from "src/model/serverResponse";
import { ApiURLService } from "./api-url.service";
import { LogService } from "./log.service";
import { UserInfo } from "src/model/UserInfo";


@Injectable({
  providedIn: 'root'
})
export class UserService {

  public userVersion: string = "?version=1.0";

  constructor(
    private router: Router,
    private http: HttpClient,
    private logService: LogService,
    private apiURL: ApiURLService
  ) {
  }

  usersSearched(searchString: string): Observable<UserInfo[]> {
    return this.http.get<Response<UserInfo[]>>(`${this.apiURL.userApiUrl}${this.userVersion}`, {params: {search: searchString}})
      .pipe(map(u => u.data));
  }
}
