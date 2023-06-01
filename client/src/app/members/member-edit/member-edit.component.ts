import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { IMember } from 'src/app/_models/IMember';
import { IUser } from 'src/app/_models/IUser';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';
import { getLocalizedDateTime } from 'src/app/utils/getLocalizedDate';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  @ViewChild("editForm") editForm: NgForm;
  member: IMember;
  user: IUser;

  constructor(private accountService: AccountService, private memberService: MemberService, private toastService: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user)
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    this.memberService.getMember(this.user.name).subscribe(member => this.member = member);
  }

  updateMember() {
    this.memberService.updateMember(this.member).subscribe(() => {
      this.toastService.success("Профиль успешно обновлён!");
      this.editForm.reset(this.member);
    })
  }

  translateDate(dateTime: string) {
    return getLocalizedDateTime(dateTime)
  }
}
