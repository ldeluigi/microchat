import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Message } from 'src/model/Message';
import * as signalR from "@microsoft/signalr";
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
 })
 export class SignalRService {
  private message$: Subject<Message>;
  private connection: signalR.HubConnection | undefined;
   
  constructor(
    private accountService: AccountService
  ) {
    this.message$ = new Subject<Message>();
  }

  public connect() {
    this.connection = new signalR.HubConnectionBuilder()
        .withUrl(environment.hubUrl)
        .configureLogging(signalR.LogLevel.Trace)
        .build();
    this.connection.start().catch(err => console.log(err));
    this.connection.on('SendMessage', (message) => {
      this.message$.next(message);
    });
  }

  public getMessage(): Observable<Message> {
    return this.message$.asObservable();
  }

  public sendMessage(chat: string, message: string): void {
    this.connection?.invoke("ReceiveMessage", chat, message)
  }
  
  public disconnect() {
    this.connection?.stop();
  }
 }