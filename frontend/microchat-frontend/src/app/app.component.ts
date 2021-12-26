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

  ngOnInit() {
    $('#action_menu_btn').on("click", function(){
      $('.action_menu').toggle();
    });
  }
  sendMessage() {
    console.log($("#newMessage")[0]);
  }
}
