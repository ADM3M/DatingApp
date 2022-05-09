import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IUser } from '../_models/IUser';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) { }

  getUsersWithRoles() {
    return this.http.get<Partial<IUser[]>>(this.baseUrl + "admin/users-with-roles");
  }
}
