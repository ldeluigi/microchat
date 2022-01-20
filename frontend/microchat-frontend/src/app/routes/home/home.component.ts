import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import * as $ from 'jquery';
import { Subscription } from 'rxjs';
import { SignalRService } from 'src/app/services/signal-r.service';
import { Chat } from 'src/model/Chat';
import { Stats } from 'src/model/Stats';
import { StatsComponent } from '../stats/stats.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy {
  activeList: Chat[] = [];
  chatList: Chat[] = [];
  active!: Chat;
  isWriting: boolean = false
  lastTimeWriting: number = Date.now();
  newMessage!: string;
  search!: string;
  signalRSubscription!: Subscription;


  constructor(
    private signalrService: SignalRService,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.signalrService.connect();
    this.signalRSubscription = this.signalrService.getMessage().subscribe(
      (message) => {
        //this.messages.push(message);
    });
    $('#action_menu_btn').on("click", function(){
      $('.action_menu').toggle();
    });

    //getChatList
    this.chatList.push({id:"1", user: {id: "4218-4124-6315-2412", name: "ThommyN1"}});
    this.chatList.push({id:"2"});
    this.chatList.push({id:"3"});
    this.chatList.push({id:"4"});
    this.initActiveList();
  }
      
  ngOnDestroy(): void {
    this.signalrService.disconnect();
    this.signalRSubscription.unsubscribe();
  }

  initActiveList(): void {
    this.search = "";
    this.setActiveListToChatList(() => {});
  }

  setActiveListToChatList(orElse: () => void): void {
    if (!this.search) {
      this.activeList = this.chatList;
      // if active doesn't exists 
      this.active = this.active ? this.active : this.activeList[0];
    } else {
      orElse();
    }
  }

  getClass(chat: Chat) {
    return this.active.id == chat.id ? "active" : "";
  }

  setActive(chat: Chat) {
    if (this.activeList.find(c => chat.id == c.id)) {
      this.active = chat;
    } else if (this.search) { //searched but not already existing
      console.log("TODO: create with " + chat.user);
      //this.chatService.createChat(chat.user).subscribe(newChat => this.active = newChat); // chat.user list or not?
    }
    this.initActiveList();
  }

  onChange() {
    this.lastTimeWriting = Date.now();
    window.clearTimeout();
    this.startTimeout();
    if (!this.isWriting) {
      this.isWriting = true;
      console.log("TODO: sta scrivendo");
    }
  }

  sendMessage() {
    //this.chatService.sendMessage(this.active.id, this.newMessage, this.accountservice.user)
    console.log("TODO: send " + this.newMessage);
  }
  
  findChat() {
    console.log("TODO: richiesta chats")
    let foundChatList: Chat[] = []
    foundChatList.push({id:"5", user: {id: "4218-4124-6315-2412", name: "Deloo"}});
    foundChatList.push({id:"6", user: {id: "4218-4124-6315-2413", name: "Gimmy"}});
    foundChatList.push({id:"7"});
    foundChatList.push({id:"9", user: {id: "4218-4124-6315-2417", name: "Magno"}});
    this.setActiveListToChatList(() => {this.activeList = foundChatList});
  }

  startTimeout() {
    let isWritingTime: number = 2000; //ms
    window.setTimeout(() => {
      if (this.isWriting) {
        if (Date.now() - this.lastTimeWriting > isWritingTime) {
          console.log("TODO: non sta scrivendo");
          this.isWriting = false;
        } else {
          this.startTimeout();
        }
      }
    }, isWritingTime)
  }

  showStats(active: Chat) {
    console.log("TODO: get stats from chat :"+ active.id);
    const stats: Stats = { totalMessages: 1, avgWeekMsg: 2, avgDaysMsg: 3 }
    this.dialog.open(StatsComponent, {data : stats})
  }
}
