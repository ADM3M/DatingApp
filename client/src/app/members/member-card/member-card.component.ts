import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { IMember } from 'src/app/_models/IMember';
import { IMemberWithDetails } from 'src/app/_models/IMemberWithDetails';
import { MemberService } from 'src/app/_services/member.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() member: IMemberWithDetails;
  
  constructor(private memberService: MemberService, private toastr: ToastrService, public presence: PresenceService) { }

  ngOnInit(): void {
  }

  getFavoriteButtonClassName(): string {
    if (this.member.isFavorite) {
      return 'btn btn-success';
    }

    return 'btn btn-primary';
  }

  addToFavorites(memberId: number) {
    this.memberService.addLike(memberId).subscribe(() => {
      this.toastr.success("User has been added to favorites")
    });
  }
}
