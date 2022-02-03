import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from '@angular/common/http';
import { map, Observable } from "rxjs";
import { User } from "src/model/Chat";
import { Response } from "src/model/serverResponse";
import { ApiURLService } from "./api-url.service";
import { LogService } from "./log.service";


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

  userValue(searchString: string): Observable<User[]> {
    return this.http.get<Response<User[]>>(`${this.apiURL.userApiUrl}${this.userVersion}`, {params: {search: searchString}})
      .pipe(map(u => u.data));
  }
}
