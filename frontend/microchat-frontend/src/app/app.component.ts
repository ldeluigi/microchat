import { Component, OnInit, ViewChild } from '@angular/core';
import * as $ from 'jquery';
import { Message } from 'src/model/Message';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'microchat-frontend';
  messages: Message[] = []; // messages
  //@ViewChild('TextArea') newMessage!: TextArea

  ngOnInit() {
    this.messages.push({id: "id",text:"Buon Natale", sendTime: new Date(2021,11,25,12),edited:true,sender:"Simo"});
    this.messages.push({id: "id",text:"Grazie, anche a te e famiglia!", sendTime: new Date(2021,11,25,12,1), edited:false,sender:"Thommy"});
    this.messages.push({id: "id",text:":)", sendTime: new Date(2021,11,25,12,2), edited:false,sender:"Simo"});
    this.messages.push({id: "id",text:"Dai ricominciamo a lavorare al proj", sendTime: new Date(2021,11,26,12), edited:true,sender:"Thommy"});
    $('#action_menu_btn').on("click", function(){
      $('.action_menu').toggle();
    });
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

  sendMessage() {
    console.log($("#newMessage")[0]);
  }
}