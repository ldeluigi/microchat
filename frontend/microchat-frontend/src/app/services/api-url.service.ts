import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiURLService {

  private apiURL: string | undefined;

  constructor() { }

  get baseApiUrl(): string {
    return this.loadApiUrl();
  }

  get signalRApiUrl(): string {
    var port = environment.chatPort;
    return `${this.loadApiUrl()}:${port}/signalr/chat`;
  }

  get chatApiUrl(): string {
    var port = environment.chatPort;
    return `${this.loadApiUrl()}:${port}/chats`;
  }

  get messageApiUrl(): string {
    var port = environment.chatPort;
    return `${this.loadApiUrl()}:${port}/messages`;
  }

  get authApiUrl(): string {
    var port = environment.authPort;
    return `${this.loadApiUrl()}:${port}/accounts`;
  }

  get passwordApiUrl(): string {
    var port = environment.authPort;
    return `${this.loadApiUrl()}:${port}/passwords`;
  }

  get tokenApiUrl(): string {
    var port = environment.authPort;
    return `${this.loadApiUrl()}:${port}/token`;
  }

  get userApiUrl(): string {
    var port = environment.userPort;
    return `${this.loadApiUrl()}:${port}/users`;
  }

  private loadApiUrl(): string {
    if (this.apiURL) {
      return this.apiURL;
    }
    if (environment.production) {
      const baseUrl = window.location.origin;
      this.apiURL = baseUrl;
      // this.apiURL = environment.apiUrl;
    } else {
      this.apiURL = environment.apiUrl;
    }
    return this.apiURL;
  }
}
