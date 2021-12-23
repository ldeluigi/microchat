import { Component, OnInit } from '@angular/core';
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

  ngOnInit() {
    this.messages.push({id: "id",text:"", editText:(a)=>"id",sender:""});
    $('#action_menu_btn').on("click", function(){
      $('.action_menu').toggle();
    });
  }

  getId(message: Message) {
    return message.id ? "mine" : "other";
  }
}
