import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';
import { firstValueFrom, map, Observable, Observer, Subscriber } from 'rxjs';
import { ChatOfUser, DetailedChat } from 'src/model/Chat';
import { Response, ResponsePaginate } from 'src/model/serverResponse';
import { ApiURLService } from './api-url.service';
import { LogService } from './log.service';
import { MessageDto } from 'src/model/Message';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  public chatVersion: string = "1.0";

  constructor(
    private router: Router,
    private http: HttpClient,
    private logService: LogService,
    private apiURL: ApiURLService) {
    }

  
    public getChats(userId: string): Observable<ChatOfUser[]> {
      return new Observable(obs => this.sub(userId, 0, obs));
    }

    private sub(userId:string, page: number, obs:Subscriber<ChatOfUser[]>) {
      this.getPaginateChat(userId, page, 100).then(chat => 
        {
          obs.next(chat.data);
          if (chat.meta.pageIndex < chat.meta.pageCount - 1) {
            this.sub(userId, chat.meta.pageIndex + 1, obs);
          } else {
            obs.complete();
          }
        }).catch(error => obs.error(error))
    };

    private getPaginateChat(userId: string, pageIndex: number, pageSize: number): Promise<ResponsePaginate<ChatOfUser[]>> {
      let params = new HttpParams()
        .set("userId", userId)
        .set("pageIndex", pageIndex)
        .set("pageSize", pageSize)
        .set("version", this.chatVersion)
      return firstValueFrom(this.http.get<ResponsePaginate<ChatOfUser[]>>(`${this.apiURL.chatApiUrl}`, {params: params}));
    }

    public chatInfo(chatId: string): Observable<DetailedChat> {
      let params = new HttpParams()
      .set("version", this.chatVersion)
      return this.http.get<Response<DetailedChat>>(`${this.apiURL.chatApiUrl}/${chatId}`, {params: params})
        .pipe(map(u => u.data));
    }

    public getOldMessages(chatId: string, pageIndex: number, pageSize: number): Observable<ResponsePaginate<MessageDto[]>> {
      let params = new HttpParams()
        .set("chatId", chatId)
        .set("pageIndex", pageIndex)
        .set("pageSize", pageSize)
        .set("version", this.chatVersion)
      return this.http.get<ResponsePaginate<MessageDto[]>>(`${this.apiURL.messageApiUrl}`, {params: params});
    }
}
