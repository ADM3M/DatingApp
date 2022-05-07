import { Component, OnInit } from '@angular/core';
import { IMember } from 'src/app/_models/IMember';
import { IPagination } from 'src/app/_models/Pagination';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members: Partial<IMember[]>;
  predicate = "liked";
  pageNumber = 1;
  pageSize = 10;
  pagination: IPagination; 
  
  constructor(private memberService: MemberService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe(response => {
      this.members = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadLikes();
  }
}
