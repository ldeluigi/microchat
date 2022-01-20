import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Chat } from 'src/model/Chat';
import { Message } from 'src/model/Message';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  @Input() chat!: Chat;
  messages: Message[] = []; // messages

  constructor() {}

  ngOnInit() {
    this.getOldMessages()
    this.getOldMessages()
    this.getOldMessages()
    this.getOldMessages()
  }

  getOldMessages() {
    this.messages.push({id: "id",text:"Buon Natale", sendTime: new Date(2021,11,25,12),edited:true,sender:"Simo"});
    this.messages.push({id: "id",text:"Grazie, anche a te e famiglia!", sendTime: new Date(2021,11,25,12,1), edited:false,sender:"Thommy"});
    this.messages.push({id: "id",text:":)", sendTime: new Date(2021,11,25,12,2), edited:false,sender:"Simo"});
    this.messages.push({id: "id",text:"Dai ricominciamo a lavorare al proj", sendTime: new Date(2021,11,26,12), edited:true,sender:"Thommy"});
  }

  getId(message: Message) {
    return this.amITheSender(message.sender) ? "mine" : "other";
  }

  imgFirst(message: Message) {
    return this.amITheSender(message.sender);
  }

  private amITheSender(sender: string) {
    return sender == "Simo"
  }

  getSrcImg(message: Message) {
    return "https://therichpost.com/wp-content/uploads/2020/06/avatar2.png";
  }
}
