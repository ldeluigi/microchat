import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { Message } from 'src/model/Message';
import * as signalR from "@microsoft/signalr";
import { AccountService } from './account.service';
import { ApiURLService } from './api-url.service';
import { LogService } from './log.service';
import { Chat } from 'src/model/Chat';

@Injectable({
  providedIn: 'root'
 })
 export class SignalRService {
  private newMessage$: Subject<Message>;
  private editMessage$: Subject<Message>;
  private messageDeleted$: Subject<string>;
  private chatDeleted$: Subject<string>;
  private chatCreated$: Subject<Chat>;
  private connection: signalR.HubConnection | undefined;
   
  constructor(
    private accountService: AccountService,
    private logService: LogService,
    private apiUrlService: ApiURLService
  ) {
    this.newMessage$ = new Subject<Message>();
    this.editMessage$ = new Subject<Message>();
    this.messageDeleted$ = new Subject<string>();
    this.chatDeleted$ = new Subject<string>();
    this.chatCreated$ = new Subject<Chat>();
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
    this.connection.on('DeleteChat', (chatId) =>{
      this.chatDeleted$.next(chatId);
    });
    this.connection.on('CreateChat', (chat) =>{
      this.chatCreated$.next(JSON.parse(chat));
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

  public deletedChat(): Observable<string> {
    return this.chatDeleted$.asObservable();
  }

  public newChat(): Observable<Chat> {
    return this.chatCreated$.asObservable();
  }

  private reconnectIfNecessary(): Promise<void> {
    return this.connection?.state !== "Connected" ? this.connect() : Promise.resolve()
  }

  private useConnection(action: (_: any) => Promise<void>): Promise<void> {
    return this.reconnectIfNecessary().then(action).catch(err => this.logService.errorSnackBar(err));
  }

  public sendMessage(chatId: string, message: string): Promise<void> {
    return this.useConnection(_ => 
      this.connection ? 
        this.connection.invoke("message.send", chatId, message) :
        Promise.reject("An error has occured while sending message: Unable to establish connection"));
  }

  public viewedMessage(messageId: string): Promise<void> {
    return this.useConnection(_ => 
      this.connection ? 
        this.connection.invoke("message.view", messageId) :
        Promise.reject("An error has occured while sending message: Unable to establish connection"));
  }

  public editMessage(messageId: string, message: string): Promise<void> {
    return this.useConnection(_ => 
      this.connection ? 
        this.connection?.invoke("message.edit", messageId, message) :
        Promise.reject("An error has occured while editing message: Unable to establish connection"));
  }

  public deleteMessage(messageId: string): Promise<void> {
    return this.useConnection(_ => 
      this.connection ? 
        this.connection?.invoke("message.delete", messageId) :
        Promise.reject("An error has occured while deleting message: Unable to establish connection"));
  }

  public deleteChat(chatId: string): Promise<void> {
    return this.useConnection(_ => 
      this.connection ? 
        this.connection?.invoke("chat.delete", chatId) :
        Promise.reject("An error has occured while deleting chat: Unable to establish connection"));
  }

  public createChat(userId: string): Promise<void> {
    return this.useConnection(_ => 
      this.connection ? 
        this.connection?.invoke("chat.createWith", userId) :
        Promise.reject("An error has occured while creating chat: Unable to establish connection"));
  }
  
  public disconnect() {
    this.connection?.stop();
  }
 }