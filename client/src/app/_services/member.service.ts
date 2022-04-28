import { HttpClient, HttpHeaders, JsonpClientBackend } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IMember } from '../_models/IMember';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  baseUrl = environment.apiUrl;


  constructor(private http: HttpClient) { }

  getMembers() {
    return this.http.get<IMember[]>(this.baseUrl + "users");
  }

  getMember(username : string) {
    return this.http.get<IMember>(this.baseUrl + "users/" + username);
  }
}
