import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { IMember } from "../_models/IMember";
import { MemberService } from "../_services/member.service";

@Injectable({
    providedIn: 'root'
})
export class MemberDetailedResolver implements Resolve<IMember> {
    constructor(private memberService: MemberService) {
    }
    
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<IMember> {
        return this.memberService.getMember(route.paramMap.get('name'))
    }

}