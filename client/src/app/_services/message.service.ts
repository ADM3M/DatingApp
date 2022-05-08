import { HttpClient } from '@angular/common/http';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IMessage } from '../_models/IMessage';
import { PaginatedResult } from '../_models/Pagination';
import { getPaginatedResult, getPaginationHeader } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  

  constructor(private http: HttpClient) { }

  getMessages(pageNumber, pageSize, container: string) {
    let params = getPaginationHeader(pageNumber, pageSize);
    params = params.append("container", container);
    return getPaginatedResult<IMessage[]>(this.baseUrl + "messages", this.http, params);
  }
}
