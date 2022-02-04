import { AfterViewInit, Component, ElementRef, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { Chat } from 'src/model/Chat';
import { Message } from 'src/model/Message';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnChanges, AfterViewInit {
  @Input() chat!: Chat;
  @Input() message!: Message | undefined;
  @Input() scrollPerc!: number;
  messages: Message[] = []; // messages

  constructor(
    private elementRef: ElementRef,
    private accountService: AccountService
  ) {}

  ngAfterViewInit(): void {
    this.elementRef.nativeElement.scrollTop = this.elementRef.nativeElement.scrollHeight;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['chat']) {
      console.log("TODO: changed Chat");
      this.elementRef.nativeElement.scrollTop = this.elementRef.nativeElement.scrollHeight;
      this.messages = [];
      this.getOldMessages();
      this.getOldMessages();
      this.getOldMessages();
      this.getOldMessages();
    } 
    if (changes['message'] && changes['message'].currentValue) {
      this.addToMessages(changes['message'].currentValue, true);
    }
    if (changes['scrollPerc'] && changes['scrollPerc'].currentValue < 15) {
      changes['scrollPerc'].currentValue;
    }
  }

  addToMessages(message:Message, asFirst: boolean) : boolean {
    var haveToUpdate = !this.messages.includes(message)
    if (haveToUpdate) {
      if (asFirst) {
        this.messages.push(message);
      } else {
        this.messages.unshift(message);
      }
    }
    return haveToUpdate;
  }

  getOldMessages() {
    var receivedMessages = [
      {id: "id", chatId: this.chat.id, text:"Buon Natale", sendTime: new Date(2021,11,25,12),edited:true,sender:"Simo"},
      {id: "id", chatId: this.chat.id, text:"Grazie, anche a te e famiglia!", sendTime: new Date(2021,11,25,12,1), edited:false,sender:"Thommy"},
      {id: "id", chatId: this.chat.id, text:":)", sendTime: new Date(2021,11,25,12,2), edited:false,sender:"Simo"},
      {id: "id", chatId: this.chat.id, text:"Dai ricominciamo a lavorare al proj", sendTime: new Date(2021,11,26,12), edited:true,sender:"Thommy"}
    ]
    var count = 0;
    receivedMessages.reverse().forEach(message => count += this.addToMessages(message, false) ? 1 : 0);
  }

  getId(message: Message) {
    return !this.amITheSender(message.sender) ? "mine" : "other";
  }

  imgFirst(message: Message) {
    return !this.amITheSender(message.sender);
  }

  private amITheSender(sender: string) {
    return sender == this.accountService.userValue?.userId
  }

  getSrcImg(message: Message) {
    return "https://therichpost.com/wp-content/uploads/2020/06/avatar2.png";
  }
}
