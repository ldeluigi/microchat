import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from '@angular/common/http';
import { first, firstValueFrom, map, Observable } from "rxjs";
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

  usersSearched(searchString: string): Promise<UserInfo[]> {
    return firstValueFrom(this.http.get<Response<UserInfo[]>>(`${this.apiURL.userApiUrl}${this.userVersion}`, {params: {search: searchString}})
      .pipe(map(u => u.data)));
  }

  userInfo(userId: string): Promise<UserInfo> {
    return firstValueFrom(this.http.get<Response<UserInfo>>(`${this.apiURL.userApiUrl}/${userId}${this.userVersion}`)
      .pipe(map(u => u.data)));
  }

  getSrcImg(userId: string): string {
    return "https://therichpost.com/wp-content/uploads/2020/06/avatar2.png";
  }

  updateName(userId: string, name: string): Promise<void> {
    const data = {name: name};
    return this.update(userId, data);
  }

  updateSurname(userId: string, surname: string): Promise<void> {
    const data = {surname: surname};
    return this.update(userId, data);
  }

  async update(userId: string, data: any): Promise<void> {
    this.http.put<Response<UserInfo>>(`${this.apiURL.userApiUrl}/${userId}${this.userVersion}`, data)
      .pipe(first()).subscribe({
        next: data => {
          console.log(data);
          this.logService.messageSnackBar('Data updated correctly');
        },
        error: (err: Error) => {
          this.logService.errorSnackBar(err.message)
        }
      });
  }
}
