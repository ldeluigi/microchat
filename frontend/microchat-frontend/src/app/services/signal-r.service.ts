import { Injectable } from '@angular/core';
import { first, Observable, Subject } from 'rxjs';
import { Message, MessageDto, toMessage } from 'src/model/Message';
import * as signalR from "@microsoft/signalr";
import { AccountService } from './account.service';
import { ApiURLService } from './api-url.service';
import { LogService } from './log.service';
import { Chat, ChatDto } from 'src/model/Chat';
import { UserService } from './user.service';
import { infoToUser } from 'src/model/UserInfo';

@Injectable({
  providedIn: 'root'
 })
 export class SignalRService {
  private newMessage$: Subject<Message>;
  private editMessage$: Subject<Message>;
  private messageDeleted$: Subject<Message>;
  private chatDeleted$: Subject<string>;
  private chatCreated$: Subject<Chat>;
  private connection: signalR.HubConnection | undefined;
   
  constructor(
    private accountService: AccountService,
    private userService: UserService,
    private logService: LogService,
    private apiUrlService: ApiURLService
  ) {
    this.newMessage$ = new Subject<Message>();
    this.editMessage$ = new Subject<Message>();
    this.messageDeleted$ = new Subject<Message>();
    this.chatDeleted$ = new Subject<string>();
    this.chatCreated$ = new Subject<Chat>();
  }

  public connect(): Promise<void> {
    var started = Promise.resolve();
      if (this.isConnected()) {
        this.disconnect();
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(this.apiUrlService.signalRApiUrl, { accessTokenFactory: () => this.accountService.userValue?.accessToken || "" })
            .configureLogging(signalR.LogLevel.Warning)
            .withAutomaticReconnect()
            .build();
        started = this.connection.start().then(_ => this.initOnMethods()).catch(err => {
          if (this.accountService.userValue) {
            if (Date.parse(this.accountService.userValue?.expirationDate) - new Date().getTimezoneOffset() * 60 * 1000 > Date.now()) {
              this.accountService.refreshToken().pipe(first()).subscribe(_ => this.connect());
            }
            console.log(err);
          }
        });
      }
      return started;
    }

  private initOnMethods() {
    this.connection!.on('message.received', (message: MessageDto) => {
      this.newMessage$.next(toMessage(message));
    });
    this.connection!.on('message.edited', (message: MessageDto) => {
      this.editMessage$.next(toMessage(message));
    });
    this.connection!.on('message.deleted', (message: MessageDto) => {
      this.messageDeleted$.next(toMessage(message));
    });
    this.connection!.on('message.viewed', _ => {}); // ignore viewed message
    this.connection!.on('chat.deleted', (chat: ChatDto) => {
      this.chatDeleted$.next(chat.id);
    });
    this.connection!.on('chat.created', (chatDto : ChatDto) => {
      var userId = chatDto.creator;
      if (this.accountService.userValue?.userId === chatDto.creator) {
        userId = chatDto.partecipant;
      }
      this.userService.userInfo(userId).subscribe(info => {
        var newChat: Chat = {id: chatDto.id, hasNewMessages: 0, lastMessageTime: new Date, user: infoToUser(info)}
        this.chatCreated$.next(newChat);
      })
    });
    this.connection!.on('error', _ => this.logService.errorSnackBar("an error has occured"));
  }

  public newMessage(): Observable<Message> {
    return this.newMessage$.asObservable();
  }

  public editedMessage(): Observable<Message> {
    return this.editMessage$.asObservable();
  }

  public deletedMessage(): Observable<Message> {
    return this.messageDeleted$.asObservable();
  }

  public deletedChat(): Observable<string> {
    return this.chatDeleted$.asObservable();
  }

  public newChat(): Observable<Chat> {
    return this.chatCreated$.asObservable();
  }

  private reconnectIfNecessary(): Promise<void> {
    return this.isConnected() ? this.connect() : Promise.resolve()
  }

  public isConnected(): boolean {
    return this.connection?.state !== "Connected";
  }

  private useConnection(action: (_: any) => Promise<void>): Promise<void> {
    return this.reconnectIfNecessary()
      .then(action)
      .catch(err => this.logService.errorSnackBar(err.message ? err.message : err));
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