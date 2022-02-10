import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from 'src/app/services/user.service';
import { Chat, UserLeftChat } from 'src/model/Chat';
import { UserInfoComponent } from '../user-info/user-info.component';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss']
})
export class ContactComponent implements OnInit {

  @Input() chat: Chat | undefined

  constructor(
    private userService: UserService,
    public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  title(): string {
    return this.chat ? 
            !UserLeftChat(this.chat) ? 
                this.chat.user?.username +
                  (this.chat.user?.name || this.chat.user?.surname ? " - " + this.chat.user?.name + " " + this.chat.user?.surname : "")
              : "User left chat"
            : "Waiting for chat"
  }

  contactInfo(): string {
    return this.chat && this.chat.hasNewMessages > 0 ? this.chat.hasNewMessages + " new messages" : " No new messages";
  }

  getUserInfo(event: Event) {
    event.stopPropagation();
    if (this.chat?.user) {
      this.dialog.open(UserInfoComponent, {data: {id: this.chat.user.id}});
    }
  }

  getSrcImg() {
    return this.userService.getSrcImg(this.chat?.user?.id || "");
  }

}
