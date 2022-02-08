import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';
import { firstValueFrom, map, Observable, Observer, Subscriber } from 'rxjs';
import { ChatDto } from 'src/model/Chat';
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

  
    public getChats(chatId: string): Observable<ChatDto[]> {
      return new Observable(obs => this.sub(chatId, 0, obs));
    }

    private sub(chatId:string, page: number, obs:Subscriber<unknown>) {
      this.getPaginateChat(chatId, page, 100).then(chat => 
        {
          if (chat.meta.pageIndex < chat.meta.pageCount - 1) {
            this.sub(chatId, chat.meta.pageIndex + 1, obs);
          } else {
            obs.complete();
          }
          obs.next(chat.data);
        })
    };

    private getPaginateChat(chatId: string, pageIndex: number, pageSize: number): Promise<ResponsePaginate<ChatDto[]>> {
      let params = new HttpParams()
        .set("chatId", chatId)
        .set("pageIndex", pageIndex)
        .set("pageSize", pageSize)
        .set("version", this.chatVersion)
      return firstValueFrom(this.http.get<ResponsePaginate<ChatDto[]>>(`${this.apiURL.chatApiUrl}/${chatId}`, {params: params}));
    }


    public chatInfo(chatId: string): Observable<ChatDto> {
      let params = new HttpParams()
      .set("version", this.chatVersion)
      return this.http.get<Response<ChatDto>>(`${this.apiURL.chatApiUrl}/${chatId}`, {params: params})
        .pipe(map(u => u.data));
    }

    public getOldMessages(chatId: string, pageIndex: number, pageSize: number): Observable<MessageDto> {
      let params = new HttpParams()
        .set("chatId", chatId)
        .set("pageIndex", pageIndex)
        .set("pageSize", pageSize)
        .set("version", this.chatVersion)
      return this.http.get<Response<MessageDto>>(`${this.apiURL.messageApiUrl}/${chatId}`, {params: params})
        .pipe(map(u => u.data));
    }
}
