import { Component, OnInit } from '@angular/core';
import { IMember } from 'src/app/_models/IMember';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  members : IMember[];
  
  constructor(private memberService: MemberService) { }

  loadMembers() {
    this.memberService.getMembers().subscribe(members => {
      this.members = members;
    })
  }
  
  ngOnInit(): void {
    this.loadMembers();
  }

}
