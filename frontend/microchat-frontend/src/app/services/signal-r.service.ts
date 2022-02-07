import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { Message } from 'src/model/Message';
import * as signalR from "@microsoft/signalr";
import { AccountService } from './account.service';
import { ApiURLService } from './api-url.service';
import { LogService } from './log.service';

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
    private logService: LogService,
    private apiUrlService: ApiURLService
  ) {
    this.newMessage$ = new Subject<Message>();
    this.editMessage$ = new Subject<Message>();
    this.messageDeleted$ = new Subject<string>();
  }

  public connect(): Promise<void> {
    this.connection = new signalR.HubConnectionBuilder()
        .withUrl(this.apiUrlService.chatApiUrl, { accessTokenFactory: () => this.accountService.userValue?.accessToken || "" })
        .configureLogging(signalR.LogLevel.Trace)
        .build();
    var started = this.connection.start().catch(err => console.log(err));
    this.connection.on('ReceiveMessage', (message) => {
      this.newMessage$.next(JSON.parse(message));
    });
    this.connection.on('UpdateMessage', (message) => {
      this.editMessage$.next(JSON.parse(message));
    });
    this.connection.on('DeleteMessage', (messageId) =>{
      this.messageDeleted$.next(messageId);
    });
    return started;
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

  private reconnectIfNecessary(): Promise<void> {
    return this.connection?.state !== "Connected" ? this.connect() : Promise.resolve()
  }

  private useConnection(action: (_: any) => Promise<void>): Promise<void> {
    return this.reconnectIfNecessary().then(action).catch(err => this.logService.errorSnackBar("An error has occured with message"));
  }

  public sendMessage(chatId: string, message: string): Promise<void> {
    return this.useConnection(_ => 
      this.connection ? 
        this.connection.invoke("message.send", chatId, message) :
        Promise.reject("Unable to establish connection"));
  }

  public editMessage(messageId: string, message: string): Promise<void> {
    return this.useConnection(_ => 
      this.connection ? 
        this.connection?.invoke("message.edit", messageId, message) :
        Promise.reject("Unable to establish connection"));
  }

  public deleteMessage(messageId: string): Promise<void> {
    return this.useConnection(_ => 
      this.connection ? 
        this.connection?.invoke("message.delete", messageId) :
        Promise.reject("Unable to establish connection"));
  }
  
  public disconnect() {
    this.connection?.stop();
  }
 }