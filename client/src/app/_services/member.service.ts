import { HttpClient, HttpHeaders, JsonpClientBackend } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IMember } from '../_models/IMember';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  baseUrl = environment.apiUrl;
  members: IMember[] = [];

  constructor(private http: HttpClient) { }

  getMembers() {
    if (this.members.length > 0) {
      return of(this.members);
    }

    return this.http.get<IMember[]>(this.baseUrl + "users").pipe(
      map(members => {
        this.members = members;
        return members;
      })
    );
  }

  getMember(username: string) {
    const member = this.members.find(x => x.name === username)

    if (member !== undefined) {
      return of(member);
    }

    return this.http.get<IMember>(this.baseUrl + "users/" + username);
  }

  updateMember(member: IMember) {
    return this.http.put(this.baseUrl + "users", member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + "users/set-main-photo/" + photoId, {})
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + "users/delete-photo/" + photoId);
  }
}
