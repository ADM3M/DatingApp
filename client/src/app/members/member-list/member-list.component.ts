import { Component, OnInit } from '@angular/core';
import { IMemberWithDetails } from 'src/app/_models/IMemberWithDetails';
import { IUser } from 'src/app/_models/IUser';
import { IPagination } from 'src/app/_models/Pagination';
import { UserParams } from 'src/app/_models/userParams';
import { MemberService } from 'src/app/_services/member.service';


@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: IMemberWithDetails[];
  pagination: IPagination;
  userParams: UserParams;
  user: IUser;
  genderList = [{value: "male", display: "Males"}, {value: "female", display: "Females"}]
  
  constructor(private memberService: MemberService) {
      this.userParams = this.memberService.getUserParams();
   }
  
  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.setUserParams(this.userParams);

    this.memberService.getMembersWithDetails(this.userParams).subscribe(response => {
      this.members = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.loadMembers();
  }

  resetFilters() {
    this.userParams = this.memberService.resetUserParams();
    this.memberService.setUserParams(this.userParams);
    this.loadMembers();
  }

  isPaginationRequired(): boolean {
    return this.pagination.totalItems > this.pagination.itemsPerPage;
  }
}
