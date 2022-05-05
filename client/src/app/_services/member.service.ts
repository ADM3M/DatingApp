import { HttpClient, HttpHeaders, HttpParams, JsonpClientBackend } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IMember } from '../_models/IMember';
import { PaginatedResult } from '../_models/Pagination';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  baseUrl = environment.apiUrl;
  members: IMember[] = [];
  paginatedResult: PaginatedResult<IMember[]> = new PaginatedResult<IMember[]>();

  constructor(private http: HttpClient) { }

  getMembers(page?: number, itemsPerPage?: number) {
    
    let params = new HttpParams();

    if (page !== null && itemsPerPage !== null) {
      params = params
        .append('pageNumber', page.toString())
        .append('pageSize', itemsPerPage.toString())
    }
    
    return this.http.get<IMember[]>(this.baseUrl + "users", {observe: 'response', params}).pipe(
      map(response => {
        this.paginatedResult.result = response.body;
        if (response.headers.get("pagination") !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get("pagination"))
        }

        return this.paginatedResult;
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
