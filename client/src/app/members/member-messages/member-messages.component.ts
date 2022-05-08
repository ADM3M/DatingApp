import { Component, Input, OnInit } from '@angular/core';
import { IMessage } from 'src/app/_models/IMessage';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @Input() messages: IMessage[];



  constructor() { }

  ngOnInit(): void {
  }
}
