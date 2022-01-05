import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import * as $ from 'jquery';
import { Chat } from 'src/model/Chat';
import { Stats } from 'src/model/Stats';
import { StatsComponent } from './stats/stats.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  chatList: Chat[] = [];
  active!: Chat;
  isWriting: boolean = false
  lastTimeWriting: number = Date.now();
  isWritingTime: number = 2000; //ms
  newMessage!: Event;

  constructor(
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.chatList.push({id:"1", user: {id: "4218-4124-6315-2412", name: "ThommyN1"}});
    this.chatList.push({id:"2"});
    this.chatList.push({id:"3"});
    this.chatList.push({id:"4"});
    $('#action_menu_btn').on("click", function(){
      $('.action_menu').toggle();
    });
    this.active = this.chatList[0];
  }

  sendMessage() {
    console.log(this.newMessage);
  }

  getClass(chat: Chat) {
    return this.active.id == chat.id ? "active" : "";
  }

  setActive(chat: Chat) {
    if (this.chatList.find(c => chat.id == c.id)) {
      this.active = chat;
    }
  }

  onChange() {
    this.lastTimeWriting = Date.now();
    window.clearTimeout();
    this.startTimeout();
    if (!this.isWriting) {
      this.isWriting = true;
      console.log("TODO: sta scrivendo");
      // TODO: invia sto scrivendo
    }
  }

  startTimeout() {
    window.setTimeout(() => {
      if (this.isWriting) {
        if (Date.now() - this.lastTimeWriting > this.isWritingTime) {
          console.log("TODO: non sta scrivendo");
          this.isWriting = false;
        } else {
          this.startTimeout();
        }
      }
    }, this.isWritingTime)
  }

  showStats(active: Chat) {
    // TODO: get stats from chat
    console.log("TODO: get stats from chat");
    const dialogConfig = new MatDialogConfig();
    const stats: Stats = { totalMessages: 1, avgWeekMsg: 2, avgDaysMsg: 3 }

    dialogConfig.data = stats
    console.log(active)
    this.dialog.open(StatsComponent, dialogConfig)
  }
}
