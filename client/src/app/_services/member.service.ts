import { HttpClient, HttpHeaders, HttpParams, JsonpClientBackend } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IMember } from '../_models/IMember';
import { IUser } from '../_models/IUser';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { getPaginatedResult, getPaginationHeader } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  baseUrl = environment.apiUrl;
  members: IMember[] = [];
  user: IUser;
  userParams: UserParams;
  memberCache = new Map();

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
    })
  }

  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  getMembers(userParams: UserParams) {

    var response = this.memberCache.get(Object.values(userParams).join("-"));

    if (response) {
      return of(response);
    }

    let httpParams = getPaginationHeader(userParams.pageNumber, userParams.pageSize);
    httpParams = httpParams
      .append("minAge", userParams.minAge.toString())
      .append("maxAge", userParams.maxAge.toString())
      .append("gender", userParams.gender)
      .append("orderBy", userParams.orderBy);

    return getPaginatedResult<IMember[]>(this.baseUrl + "users", this.http, httpParams).pipe(map(response => {
      this.memberCache.set(Object.values(userParams).join("-"), response);
      return response;
    }));
  }

  getMember(username: string) {
    let member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: IMember) => member.name === username);

    if (member) {
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

  addLike(username: string) {
    return this.http.post(this.baseUrl + "likes/" + username, {});
  }

  getLikes(predicate: string, pageNumber: number, pageSize: number) {
    let params = getPaginationHeader(pageNumber, pageSize);
    params = params.append("predicate", predicate);
    return getPaginatedResult<Partial<IMember[]>>(this.baseUrl + "likes/", this.http, params);
  }

}
