import { AfterViewChecked, AfterViewInit, Component, ElementRef, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { AccountService } from 'src/app/services/account.service';
import { ChatService } from 'src/app/services/chat.service';
import { LogService } from 'src/app/services/log.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import { UserService } from 'src/app/services/user.service';
import { Chat } from 'src/model/Chat';
import { Message, toMessage } from 'src/model/Message';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy, AfterViewChecked {
  @Input() chat: Chat | undefined;
  @Input() message!: Message | undefined;
  @Input() scrollPerc!: number;
  haveToScroll: boolean = false;
  scrollPercToGetOldMessages: number = 15;
  messagePage = 0;

  deletedMessageId!: Subscription;
  editedMessageId!: Subscription;
  viewedMessageId!: Subscription;
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
    private logService: LogService,
    private signalrService: SignalRService
  ) {}

  ngAfterViewChecked(): void {
    this.scrollDown();
  }

  ngOnInit(): void {
    this.deletedMessageId = this.signalrService.deletedMessage().subscribe(
      {next: deletedMessage => {
        this.messages = this.messages.filter(message => message.id !== deletedMessage.id)
      }, error: err => this.logService.errorSnackBar(err)
    });
    this.editedMessageId = this.signalrService.editedMessage().subscribe(
      {
        next: editedMessage => this.updateMessage(editedMessage),
        error: err => this.logService.errorSnackBar(err)
    });
    this.viewedMessageId = this.signalrService.viewedMessage().subscribe(
      {
        next: viewedMessage => this.updateMessage(viewedMessage),
        error: err => this.logService.errorSnackBar(err)
    });
  }

  private updateMessage(msg: Message) {
    const index = this.messages.findIndex(message => message.id == msg.id);
    if (index >= 0) {
      this.messages[index] = msg;
    } else {
      this.addToMessages(msg, true);
      this.resortMessages();
    }
  }

  ngOnDestroy(): void {
    this.deletedMessageId.unsubscribe();
    this.editedMessageId.unsubscribe();
    this.viewedMessageId.unsubscribe();
  }

  ngAfterViewInit(): void {
    this.haveToScroll = true;
  }

  private scrollDown(): void {
    if (this.haveToScroll) {
      this.elementRef.nativeElement.scrollTop = this.elementRef.nativeElement.scrollHeight;
    }
    this.haveToScroll = false;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['chat'] /*&& changes['chat'].currentValue*/) { // TODO
      this.messages = [];
      this.messagePage = 0;
      if (changes['chat'].currentValue) {
        this.getOldMessages(true);
      }
    }
    if (changes['message'] && changes['message'].currentValue) {
      this.addToMessages(changes['message'].currentValue, true);
      if (changes['message'].currentValue.sender != this.accountService.userValue?.userId) {
        this.signalrService.viewMessage(changes['message'].currentValue.id);
      }
      this.haveToScroll = true;
    }
    if (changes['scrollPerc'] && changes['scrollPerc'].currentValue < this.scrollPercToGetOldMessages && this.chat) {
      console.log("TODO: carica altri messaggi");
      this.getOldMessages(false);
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
      const pageSize = 5;
      this.chatService.getOldMessages(this.chat.id, this.messagePage, pageSize).subscribe({next: messages => {
        if (messages.data[0] && this.chat?.id == messages.data[0].chat) {
          this.messagePage++;
        }
        var count = 0;
        messages.data.forEach(message => count += this.addToMessages(toMessage(message), false) ? 1 : 0);
        this.haveToScroll = scroll;
        if (this.scrollPerc < this.scrollPercToGetOldMessages && messages.meta.pageIndex < messages.meta.pageCount - 1) {
          this.getOldMessages(scroll);
        }
        this.messages.filter(m => !m.viewed && (m.sender !== this.accountService.userValue?.userId)).forEach(m => {
          this.signalrService.viewMessage(m.id);
        });
        this.chat!.hasNewMessages = 0;
      }, error: err => this.logService.errorSnackBar(err)});
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
        this.signalrService.deleteMessage(message.id);
        return;
      }
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

  isEdited(message: Message): boolean {
    return message.edited;
  }

  isViewed(message: Message): boolean {
    return message.viewed && message.sender == this.accountService.userValue?.userId
  }

}
