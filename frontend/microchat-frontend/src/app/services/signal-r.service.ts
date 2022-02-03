import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { Message } from 'src/model/Message';
import * as signalR from "@microsoft/signalr";
import { AccountService } from './account.service';
import { ApiURLService } from './api-url.service';

@Injectable({
  providedIn: 'root'
 })
 export class SignalRService {
  private message$: Subject<Message>;
  private connection: signalR.HubConnection | undefined;
   
  constructor(
    private accountService: AccountService,
    private apiUrlService: ApiURLService
  ) {
    this.message$ = new Subject<Message>();
  }

  public connect() {
    this.connection = new signalR.HubConnectionBuilder()
        .withUrl(this.apiUrlService.chatApiUrl, { accessTokenFactory: () => this.accountService.userValue?.accessToken || "" })
        .configureLogging(signalR.LogLevel.Trace)
        .build();
    this.connection.start().catch(err => console.log(err));
    this.connection.on('ReceiveMessage', (message) => {
      this.message$.next(JSON.parse(message));
    });
  }

  public getMessage(): Observable<Message> {
    return this.message$.asObservable();
  }

  public sendMessage(chatId: string, message: string): void {
    this.connection?.invoke("SendMessageToAll", chatId, this.accountService.userValue?.userId, message)
  }
  
  public disconnect() {
    this.connection?.stop();
  }
 }