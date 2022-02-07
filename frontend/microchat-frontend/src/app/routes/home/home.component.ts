import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { AccountService } from 'src/app/services/account.service';
import { LogService } from 'src/app/services/log.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import { UserService } from 'src/app/services/user.service';
import { Chat, UserLeftChat } from 'src/model/Chat';
import { Message } from 'src/model/Message';
import { Stats } from 'src/model/Stats';
import { toUser } from 'src/model/UserInfo';
import { StatsComponent } from '../stats/stats.component';
import { UserInfoComponent } from '../user-info/user-info.component';

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
  editingId: string | undefined;
  signalRSubscription!: Subscription;
  newIncomingMessage: Message | undefined;
  scrollPerc = 0;
  @ViewChild('chatSelector') appChat!: ElementRef;

  constructor(
    private userService: UserService,
    private signalrService: SignalRService,
    private logService: LogService,
    private accountService: AccountService,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.signalrService.connect();
    this.signalRSubscription = this.signalrService.newMessage().subscribe(
      (message) => {
        if (message.chatId == this.active.id) {
          this.newIncomingMessage = message;
        } else {
          const index = this.chatList.findIndex(chat => chat.id === message.chatId);
          if (index >= 0) {
            this.chatList[index].hasNewMessages++;
            this.chatList.unshift(this.chatList.splice(index, 1)[0]);
          } else {
            console.log("TODO: richiedi chat");
          }
        }
    });
    //$('#action_menu_btn').on("click", function(){ $('.action_menu').toggle(); });

    //getChatList
    this.chatList.push({id:"a6e155fa-3651-4358-97d3-6394942c2daa", hasNewMessages:8, lastMessageTime: new Date(2021, 11, 1, 16), user: {id: "c6ddc9d5-6a84-4fc1-972f-57b2d866aadb", name: "ThommyN1"}});
    this.chatList.push({id:"f04cc7ad-008c-4662-a581-e0c53aa53167", hasNewMessages:5, lastMessageTime: new Date(2021, 11, 1, 13)});
    this.chatList.push({id:"3", hasNewMessages:0, lastMessageTime: new Date(2021, 11, 1, 14)});
    this.chatList.push({id:"4", hasNewMessages:2, lastMessageTime: new Date(2021, 11, 1, 15)});
    this.chatList.sort((chat1, chat2) => chat2.lastMessageTime.getTime() - chat1.lastMessageTime.getTime());
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
    if (this.chatList.find(c => chat.id == c.id)) {
      this.active = chat;
    } else if (this.search) { //searched but not already existing
      console.log("TODO: create with " + chat.user?.id);
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
    if (UserLeftChat(this.active)) {
      this.logService.errorSnackBar("unable to send messages to disabled chat");
    } else {
      if (this.editingId) {
        this.signalrService.editMessage(this.editingId, this.newMessage).then(_ => this.newMessage = "");
      } else {
        this.signalrService.sendMessage(this.active.id, this.newMessage).then(_ => this.newMessage = "");
      }
    }
  }
  
  findChat() {
    if (this.search) {
      console.log("TODO: richiesta chats", this.search);
      let foundChatList: Chat[] = []
      this.userService.usersSearched(this.search).subscribe(users => {
        users.forEach(user => 
          foundChatList.push({id: "", hasNewMessages: 0, lastMessageTime: new Date, user: toUser(user)}))
      })
      this.setActiveListToChatList(() => {this.activeList = foundChatList});
    } else {
      this.initActiveList();
    }
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
    const detailedChat = {
      Id: active.id,
      CreationTimestamp: "05/02/2022",
      NumberOfMessages: 10}
    const creationDate = new Date();
    console.log("TODO date from utc");
    const days = Math.ceil(creationDate.getTime() - Date.now() / (1000 * 3600 * 24));
    const stats: Stats = { 
      totalMessages: detailedChat.NumberOfMessages,
      avgWeekMsg: detailedChat.NumberOfMessages / Math.ceil(days / 7),
      avgDaysMsg: detailedChat.NumberOfMessages / days }
    this.dialog.open(StatsComponent, {data : stats})
  }

  logout() {
    this.accountService.logout();
  }

  scroll(event: number) {
    this.scrollPerc = event;
  }

  getSrcImg() {
    return this.userService.getSrcImg(this.accountService.userValue?.userId || "");
  }

  getUserInfo(event: Event) {
    this.dialog.open(UserInfoComponent, {data: {id: this.accountService.userValue?.userId || ""}});
  }

  sendOrEditClass() {
    return this.editingId ? "fas fa-edit" : "fas fa-location-arrow";
  }

  deleteChat() {
    this.signalrService.deleteChat(this.active.id).then();
    console.log("TODO: delete chat");
  }
}
