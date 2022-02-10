import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { AccountService } from 'src/app/services/account.service';
import { ChatService } from 'src/app/services/chat.service';
import { LogService } from 'src/app/services/log.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import { UserService } from 'src/app/services/user.service';
import { Chat, UserLeftChat } from 'src/model/Chat';
import { Message } from 'src/model/Message';
import { Stats } from 'src/model/Stats';
import { infoToUser } from 'src/model/UserInfo';
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
  active: Chat | undefined;
  isWriting: boolean = false
  lastTimeWriting: number = Date.now();
  newMessage!: string;
  search!: string;
  editingId: string | undefined;
  signalRSubscription!: Subscription;
  newChatSubscription!: Subscription;
  deletedChatSubscription!: Subscription;
  newIncomingMessage: Message | undefined;
  scrollPerc = 0;
  @ViewChild('chatSelector') appChat!: ElementRef;
  createdWith: string | undefined;

  constructor(
    private userService: UserService,
    private signalrService: SignalRService,
    private logService: LogService,
    private accountService: AccountService,
    private chatService: ChatService,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.signalrService.connect();
    this.signalRSubscription = this.signalrService.newMessage().subscribe(
      (message) => {
        const index = this.chatList.findIndex(chat => chat.id === message.chatId);
        if (message.chatId === this.active?.id) {
          this.newIncomingMessage = message;
        } else {
          if (index >= 0) {
            this.chatList[index].hasNewMessages++;
          } else {
            window.location.reload();
          }
        }
        this.chatList.unshift(this.chatList.splice(index, 1)[0]);
        this.setActiveListToChatList(() => {});
    });
    this.deletedChatSubscription = this.signalrService.deletedChat().subscribe(chatId => {
      this.active = this.active?.id === chatId ? undefined : this.active;
      this.chatList = this.chatList.filter(chat => chat.id !== chatId);
      this.setActiveListToChatList(() => this.activeList =  this.activeList.filter(chat => chat.id !== chatId));
    });
    this.newChatSubscription = this.signalrService.newChat().subscribe(chat => {
      this.chatList = this.chatList.filter(c => chat.id !== c.id);
      this.chatList.unshift(chat);
      if (chat.user?.id === this.createdWith) {
        this.active = chat;
        this.createdWith = undefined;
      }
      this.setActiveListToChatList(() => this.findChat());
    });
    //$('#action_menu_btn').on("click", function(){ $('.action_menu').toggle(); });

    this.chatService.getChats(this.accountService.userValue?.userId || "").subscribe({ // bug in test otherwise
      next: (chats) => {
        chats.forEach(chat => {
          const otherUser = 
            chat.creator && chat.creator != this.accountService.userValue?.userId ?
              chat.creator :
              chat.partecipant && chat.partecipant != this.accountService.userValue?.userId ?
                chat.partecipant :
                undefined;
          const addingChat : Chat = {
            id: chat.id,
            hasNewMessages: chat.numberOfUnreadMessages || 0,
            lastMessageTime: new Date(Date.parse(chat.lastMessageTime)),
          }
          if (otherUser) {
            this.userService.userInfo(otherUser).subscribe(info => {
              addingChat.user = infoToUser(info);
              this.addInChatList(addingChat);
            });
          } else {
            this.addInChatList(addingChat);
          }
        })
      },
      error: () => this.logService.errorSnackBar("chats are not loaded correctly"),
      complete: () => {
        console.log("chat loaded correctly");
        this.initActiveList();
      }
    });
  }

  private addInChatList(chat: Chat) {
    const index = this.chatList.findIndex(c => c.lastMessageTime.getTime < chat.lastMessageTime.getTime)
    if (index >= 0) {
      this.chatList.splice(index, 0, chat);
    } else {
      this.chatList.unshift(chat)
    }
  }
      
  ngOnDestroy(): void {
    this.signalRSubscription.unsubscribe();
    this.deletedChatSubscription.unsubscribe();
    this.newChatSubscription.unsubscribe();
    this.signalrService.disconnect();
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
    return this.active?.id === chat.id ? "active" : "";
  }

  setActive(chat: Chat) {
    if (this.chatList.find(c => chat.id == c.id || chat.user?.id == c.user?.id)) {
      this.active = chat;
    } else if (this.search) { //searched but not already existing
      console.log("TODO: create with " + chat.user?.id);
      if (chat.user) {
        this.signalrService.createChat(chat.user?.id).then(_ => {
          this.createdWith = chat.user?.id;
          this.search = "";
        });
      }
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
      //console.log("TODO: sta scrivendo");
    }
  }

  sendMessage() {
    if (this.active && UserLeftChat(this.active)) {
      this.logService.errorSnackBar("unable to send messages to disabled chat");
    } else if (this.active) {
      if (this.editingId) {
        this.signalrService.editMessage(this.editingId, this.newMessage).then(_ => this.newMessage = "");
      } else {
        this.signalrService.sendMessage(this.active?.id, this.newMessage).then(_ => this.newMessage = "");
      }
    }
  }
  
  findChat() {
    if (this.search) {
      console.log("TODO: richiesta chats", this.search);
      let foundChatList: Chat[] = []
      this.userService.usersSearched(this.search).subscribe(users => {
        users.forEach(user => 
          foundChatList.push({id: "", hasNewMessages: 0, lastMessageTime: new Date, user: infoToUser(user)}))
      })
      this.setActiveListToChatList(() => this.activeList = foundChatList);
    } else {
      this.initActiveList();
    }
  }

  startTimeout() {
    let isWritingTime: number = 2000; //ms
    window.setTimeout(() => {
      if (this.isWriting) {
        if (Date.now() - this.lastTimeWriting > isWritingTime) {
          //console.log("TODO: non sta scrivendo");
          this.isWriting = false;
        } else {
          this.startTimeout();
        }
      }
    }, isWritingTime)
  }

  showStats() {
    if (this.active) {
      this.chatService.chatInfo(this.active.id).subscribe(detailedChat => {
        const days = Math.ceil((Date.now() - Date.parse(detailedChat.creation) + new Date().getTimezoneOffset()) / (1000 * 3600 * 24));
        const stats: Stats = { 
          totalMessages: detailedChat.numberOfMessages,
          avgWeekMsg: detailedChat.numberOfMessages / Math.ceil(days / 7),
          avgDaysMsg: detailedChat.numberOfMessages / days };
        this.dialog.open(StatsComponent, {data : stats});
      });
    } else {
      this.logService.messageSnackBar("Unable to get stats for missing chat");
    }
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
    if (this.active) {
      console.log("TODO: delete chat");
      this.signalrService.deleteChat(this.active.id)
        .then(_ => this.initActiveList())
        .catch(_ => window.location.reload());
    } else {
      this.logService.messageSnackBar("Unable to delete missing chat");
    }
  }
}
