import { Component, Input, OnInit } from '@angular/core';
import { Chat } from 'src/model/Chat';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss']
})
export class ContactComponent implements OnInit {

  @Input() chat: Chat | undefined

  constructor() { }

  ngOnInit(): void {
  }

  title(): string {
    return this.chat ? 
            this.chat.user ? this.chat.user.name + " - " + this.chat.user.id : this.chat.id
            : "Waiting for chat"
  }

  contactInfo(): string {
    return this.chat && this.chat.user ? this.chat.user.name + " is online" : "";
  }

}