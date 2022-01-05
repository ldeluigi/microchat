import { Component, Input, OnInit } from '@angular/core';
import { Chat } from 'src/model/Chat';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss']
})
export class ContactComponent implements OnInit {

  @Input() chat!: Chat

  constructor() { }

  ngOnInit(): void {
  }

}
