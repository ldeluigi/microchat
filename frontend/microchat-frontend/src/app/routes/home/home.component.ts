import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { AccountService } from 'src/app/services/account.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import { UserService } from 'src/app/services/user.service';
import { Chat } from 'src/model/Chat';
import { Message } from 'src/model/Message';
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
  newIncomingMessage: Message | undefined;
  @ViewChild('chatSelector') appChat!: ElementRef;


  constructor(
    private userService: UserService,
    private signalrService: SignalRService,
    private accountService: AccountService,
    public dialog: MatDialog
  ) {}

  /*ngAfterViewInit(): void {
    console.log(this.appChat);
    this.appChat.nativeElement.scrolltop = this.appChat.nativeElement.scrollHeight;
  }*/

  ngOnInit() {
    this.signalrService.connect();
    this.signalRSubscription = this.signalrService.getMessage().subscribe(
      (message) => {
        if (message.chatId == this.active.id) {
          this.newIncomingMessage = message;
        } else {
          var chat = this.chatList.find(chat => chat.id === message.chatId);
          if (chat) {
            chat.hasNewMessages++;
          }
        }
    });
    //$('#action_menu_btn').on("click", function(){ $('.action_menu').toggle(); });

    //getChatList
    this.chatList.push({id:"a6e155fa-3651-4358-97d3-6394942c2daa", hasNewMessages:8, user: {id: "c6ddc9d5-6a84-4fc1-972f-57b2d866aadb", name: "ThommyN1"}});
    this.chatList.push({id:"f04cc7ad-008c-4662-a581-e0c53aa53167", hasNewMessages:5});
    this.chatList.push({id:"3", hasNewMessages:0});
    this.chatList.push({id:"4", hasNewMessages:2});
    this.initActiveList();
  }
      
  ngOnDestroy(): void {
    if (this.signalRSubscription) {
      this.signalRSubscription.unsubscribe();
    }
    if (this.signalrService) {
      this.signalrService.disconnect();
    }
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
      this.search = "";
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
    this.signalrService.sendMessage(this.active.id, this.newMessage);
    this.newMessage = "";
  }
  
  findChat() {
    console.log("TODO: richiesta chats", this.search);
    let foundChatList: Chat[] = []
    this.userService.userValue(this.search).subscribe(users => {
      console.log(users);
      users.forEach(user => foundChatList.push({id:"", hasNewMessages:0, user:user}))
    })
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

  logout() {
    this.accountService.logout();
  }
}
