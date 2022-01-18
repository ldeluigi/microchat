import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Message } from 'src/model/Message';
import * as signalR from "@aspnet/signalr";

@Injectable({
  providedIn: 'root'
 })
 export class SignalRService {
  private message$: Subject<Message>;
  private connection: signalR.HubConnection | undefined;
   
  constructor() {
    this.message$ = new Subject<Message>();
  }

  public connect(url: string) {
    this.connection = new signalR.HubConnectionBuilder()
        .withUrl(environment.hubUrl + url)
        .build();
    this.connection.start().catch(err => console.log(err));
    this.connection.on('SendMessage', (message) => {
      this.message$.next(message);
    });
  }

  public getMessage(): Observable<Message> {
    return this.message$.asObservable();
  }
  
  public disconnect() {
    this.connection?.stop();
  }
 }