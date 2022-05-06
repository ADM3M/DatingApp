import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { IMember } from 'src/app/_models/IMember';
import { IUser } from 'src/app/_models/IUser';
import { IPagination } from 'src/app/_models/Pagination';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  members: IMember[];
  pagination: IPagination;
  userParams: UserParams;
  user: IUser;
  genderList = [{value: "male", display: "Males"}, {value: "female", display: "Females"}]
  
  constructor(private memberService: MemberService, private accountService: AccountService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
        this.user = user;
        this.userParams = new UserParams(user);
      })
   }
  
  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers(this.userParams).subscribe(response => {
      this.members = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.loadMembers();
  }

  resetFilters() {
    this.userParams = new UserParams(this.user);
    this.loadMembers();
  }
}
