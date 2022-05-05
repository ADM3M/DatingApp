import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IMember } from 'src/app/_models/IMember';
import { IPagination } from 'src/app/_models/Pagination';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  members: IMember[];
  pagination: IPagination;
  pagenumber = 1;
  pageSize = 5;
  
  constructor(private memberService: MemberService) { }
  
  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers(this.pagenumber, this.pageSize).subscribe(response => {
      this.members = response.result;
      this.pagination = response.pagination;
    })
  }
}
