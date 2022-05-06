import { HttpClient, HttpHeaders, HttpParams, JsonpClientBackend } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IMember } from '../_models/IMember';
import { PaginatedResult } from '../_models/Pagination';
import { UserParams } from '../_models/userParams';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  baseUrl = environment.apiUrl;
  members: IMember[] = [];
  memberCache = new Map();

  constructor(private http: HttpClient) { }

  getMembers(userParams: UserParams) {

    var response = this.memberCache.get(Object.values(userParams).join("-"));

    if (response) {
      return of(response);
    }

    let httpParams = this.getPaginationHeader(userParams.pageNumber, userParams.pageSize);
    httpParams = httpParams
      .append("minAge", userParams.minAge.toString())
      .append("maxAge", userParams.maxAge.toString())
      .append("gender", userParams.gender)
      .append("orderBy", userParams.orderBy);

    return this.getPaginatedResult<IMember[]>(this.baseUrl + "users", httpParams).pipe(map(response => {
      this.memberCache.set(Object.values(userParams).join("-"), response);
      return response;
    }));
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

  private getPaginatedResult<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    return this.http.get<T>(url, { observe: 'response', params: params }).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get("pagination") !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get("pagination"));
        }

        return paginatedResult;
      })
    );
  }

  private getPaginationHeader(pageNumber: number, pageSize: Number) {
    let httpParams = new HttpParams();

    httpParams = httpParams
      .append('pageNumber', pageNumber.toString())
      .append('pageSize', pageSize.toString());

    return httpParams;
  }
}
