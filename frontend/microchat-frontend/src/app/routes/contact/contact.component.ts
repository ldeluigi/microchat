import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from 'src/app/services/user.service';
import { Chat } from 'src/model/Chat';
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
            this.chat.user ? this.chat.user.name.length > 0 ? this.chat.user.name + " - " + this.chat.user.id : this.chat.user.id : this.chat.id
            : "Waiting for chat"
  }

  contactInfo(): string {
    return this.chat && this.chat.user ? this.chat.user.name + " is online" : "";
  }

  getUserInfo(event: Event) {
    event.stopPropagation();
    if (this.chat?.user) {
    this.userService.usersInfo(this.chat.user.id).subscribe(info => 
      this.dialog.open(UserInfoComponent, {data: info}))
    }
  }

}
