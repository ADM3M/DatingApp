import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { IMemberWithDetails } from 'src/app/_models/IMemberWithDetails';
import { MemberService } from 'src/app/_services/member.service';
import { PresenceService } from 'src/app/_services/presence.service';
import { LikeState } from 'src/app/enums/LikeState';
import { getLikeMessage } from 'src/app/utils/getLikeMessage';

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
    this.memberService.addLike(memberId).subscribe(likeState => {
      this.toastr.success(getLikeMessage(likeState));
      this.member.isFavorite = likeState === LikeState.Liked ? true : false;
    });
  }
}
