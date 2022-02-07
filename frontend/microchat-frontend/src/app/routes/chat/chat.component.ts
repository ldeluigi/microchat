import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { AccountService } from 'src/app/services/account.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import { UserService } from 'src/app/services/user.service';
import { Chat } from 'src/model/Chat';
import { Message } from 'src/model/Message';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy {
  @Input() chat!: Chat;
  @Input() message!: Message | undefined;
  @Input() scrollPerc!: number;

  deletedMessageId!: Subscription;
  editedMessageId!: Subscription;
  _editingId: string | undefined;
  @Input()
  set editingId(val: string | undefined) {
    this.editingIdChange.emit(val);
    this._editingId = val;
  }
  get editingId() {
    return this._editingId;
  }
  @Output() editingIdChange: EventEmitter<string | undefined> = new EventEmitter<string | undefined>();

  messages: Message[] = []; // messages

  constructor(
    private elementRef: ElementRef,
    private accountService: AccountService,
    private userService: UserService,
    private signalrService: SignalRService
  ) {}

  ngOnInit(): void {
    this.deletedMessageId = this.signalrService.deletedMessage().subscribe(messageId =>
      this.messages = this.messages.filter(message => message.id !== messageId)
    );
    this.editedMessageId = this.signalrService.editedMessage().subscribe(editedMessage => {
      const index = this.messages.findIndex(message => message.id !== editedMessage.id);
      if (index >= 0) {
        this.messages[index] = editedMessage;
      } else {
        this.addToMessages(editedMessage, true);
        this.resortMessages();
      }
    });
  }

  ngOnDestroy(): void {
    this.deletedMessageId.unsubscribe();
    this.editedMessageId.unsubscribe();
  }

  ngAfterViewInit(): void {
    this.elementRef.nativeElement.scrollTop = this.elementRef.nativeElement.scrollHeight;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['chat']) {
      console.log("TODO: changed Chat");
      this.elementRef.nativeElement.scrollTop = this.elementRef.nativeElement.scrollHeight;
      this.messages = [];
      this.getOldMessages();
    }
    this.addToMessages(
      {id: "id1",
        chatId: this.chat.id,
        text:"Grazie, anche a me e famiglia!",
        sendTime: new Date(2021,11,25,12,4),
        edited:false,
        sender:"Thommy"
      }, true)
    if (changes['message'] && changes['message'].currentValue) {
      this.addToMessages(changes['message'].currentValue, true);
    }
    if (changes['scrollPerc'] && changes['scrollPerc'].currentValue < 15) {
      console.log("TODO: carica altri messaggi");
      changes['scrollPerc'].currentValue;
    }
  }

  resortMessages() {
    this.messages.sort((msg1, msg2) => msg1.sendTime.getTime() - msg2.sendTime.getTime());
  }

  addToMessages(message:Message, asFirst: boolean) : boolean {
    const index = this.messages.findIndex(m => m.id === message.id);
    const alreadyPresent = index !== -1;
    if (!alreadyPresent) {
      if (asFirst) {
        this.messages.push(message);
      } else {
        this.messages.unshift(message);
      }
    } else {
      this.messages[index] = message;
    }
    this.resortMessages();
    return alreadyPresent;
  }

  getOldMessages() {
    var receivedMessages = [
      {id: "id", chatId: this.chat.id, text:"Buon Natale", sendTime: new Date(2021,11,25,12),edited:true,sender:"Simo"},
      {id: "id1", chatId: this.chat.id, text:"Grazie, anche a te e famiglia!", sendTime: new Date(2021,11,25,12,1), edited:false,sender:"Thommy"},
      {id: "id2", chatId: this.chat.id, text:":)", sendTime: new Date(2021,11,25,12,2), edited:false,sender:"Simo"},
      {id: "id3", chatId: this.chat.id, text:"Dai ricominciamo a lavorare al proj", sendTime: new Date(2021,11,26,12,3), edited:true,sender:"Thommy"}
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
      return this.userService.getSrcImg(message.sender);
  }

  clickedMessage(event: MouseEvent, message: Message) {
    if (message.sender === this.accountService.userValue?.userId && event.ctrlKey) {
      if (event.altKey) {
        console.log("DELETE MESSAGE " + message.text);
        this.signalrService.deleteMessage(message.id);
        return;
      }
      console.log("update message " + message.text);
      if (this.editingId === message.id) {
        this.editingId = undefined;
      } else {
        this.editingId = message.id;
      }
    }
  }

  editingClass(id: string): string {
    return this.editingId === id ? "editing" : ""
  }
}
