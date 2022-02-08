import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { AccountService } from 'src/app/services/account.service';
import { ChatService } from 'src/app/services/chat.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import { UserService } from 'src/app/services/user.service';
import { Chat } from 'src/model/Chat';
import { Message, toMessage } from 'src/model/Message';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy {
  @Input() chat: Chat | undefined;
  @Input() message!: Message | undefined;
  @Input() scrollPerc!: number;
  scrollPercToGetOldMessages: number = 15;
  messagePage = 0;

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
    private chatService: ChatService,
    private signalrService: SignalRService
  ) {}

  ngOnInit(): void {
    this.deletedMessageId = this.signalrService.deletedMessage().subscribe(deletedMessage => {
      this.messages = this.messages.filter(message => message.id !== deletedMessage.id)
    });
    this.editedMessageId = this.signalrService.editedMessage().subscribe(editedMessage => {
      const index = this.messages.findIndex(message => message.id == editedMessage.id);
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
    this.scrollDown();
  }

  private scrollDown(): void {
    this.elementRef.nativeElement.scrollTop = this.elementRef.nativeElement.scrollHeight + this.elementRef.nativeElement.offsetHeight;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['chat'] /*&& changes['chat'].currentValue*/) { // TODO
      this.messages = [];
      this.messagePage = 0;
      if (changes['chat'].currentValue) {
        console.log(changes['chat'].currentValue.id);
        console.log("TODO: changed Chat");
        this.getOldMessages(true);
        this.messages.filter(m => m.viewed && m.sender !== this.accountService.userValue?.userId).forEach(m => {
          this.signalrService.viewedMessage(m.id);
        });
        this.chat!.hasNewMessages = 0;
        this.scrollDown();
      }
    }
    if (changes['message'] && changes['message'].currentValue) {
      this.addToMessages(changes['message'].currentValue, true);
      if (changes['message'].currentValue.sender !== this.accountService.userValue?.userId) {
        this.signalrService.viewedMessage(changes['message'].currentValue.id);
      }
      this.scrollDown();
    }
    if (changes['scrollPerc'] && changes['scrollPerc'].currentValue < this.scrollPercToGetOldMessages && this.chat) {
      console.log("TODO: carica altri messaggi");
      this.getOldMessages(false);
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
    return !alreadyPresent;
  }

  getOldMessages(scroll: boolean) {
    if (this.chat) {
      const pageSize = 100;
      this.chatService.getOldMessages(this.chat.id, this.messagePage, pageSize).subscribe(messages => {
        if (messages.data[0] && this.chat?.id == messages.data[0].chat) {
          this.messagePage++;
        }
        var count = 0;
        messages.data.forEach(message => count += this.addToMessages(toMessage(message), false) ? 1 : 0);
        if (scroll) {
          this.scrollDown();
        }
        if (this.scrollPerc < this.scrollPercToGetOldMessages && messages.meta.pageIndex < messages.meta.pageCount - 1) {
          this.getOldMessages(scroll);
        }
      });
    }
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
