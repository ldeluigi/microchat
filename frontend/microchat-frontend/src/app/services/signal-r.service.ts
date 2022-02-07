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
  private newMessage$: Subject<Message>;
  private editMessage$: Subject<Message>;
  private messageDeleted$: Subject<string>;
  private connection: signalR.HubConnection | undefined;
   
  constructor(
    private accountService: AccountService,
    private apiUrlService: ApiURLService
  ) {
    this.newMessage$ = new Subject<Message>();
    this.editMessage$ = new Subject<Message>();
    this.messageDeleted$ = new Subject<string>();
  }

  public connect() {
    this.connection = new signalR.HubConnectionBuilder()
        .withUrl(this.apiUrlService.chatApiUrl, { accessTokenFactory: () => this.accountService.userValue?.accessToken || "" })
        .configureLogging(signalR.LogLevel.Trace)
        .build();
    this.connection.start().catch(err => console.log(err));
    this.connection.on('ReceiveMessage', (message) => {
      this.newMessage$.next(JSON.parse(message));
    });
    this.connection.on('UpdateMessage', (message) => {
      this.editMessage$.next(JSON.parse(message));
    });
    this.connection.on('DeleteMessage', (messageId) =>{
      this.messageDeleted$.next(messageId);
    })
  }

  public newMessage(): Observable<Message> {
    return this.newMessage$.asObservable();
  }

  public editedMessage(): Observable<Message> {
    return this.editMessage$.asObservable();
  }

  public deletedMessage(): Observable<string> {
    return this.messageDeleted$.asObservable();
  }

  public sendMessage(chatId: string, message: string): void {
    this.connection?.invoke("message.send", chatId, message);
  }

  public editMessage(messageId: string, message: string): void {
    this.connection?.invoke("message.edit", messageId, message);
  }

  public deleteMessage(messageId: string): void {
    this.connection?.invoke("message.delete", messageId);
  }
  
  public disconnect() {
    this.connection?.stop();
  }
 }