import { HttpClient, HttpParams } from '@angular/common/http';
import { map, take } from 'rxjs/operators';
import { PaginatedResult } from '../_models/Pagination';

export function getPaginatedResult<T>(url: string, http: HttpClient, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    return http.get<T>(url, { observe: 'response', params: params }).pipe(
        map(response => {
            paginatedResult.result = response.body;
            if (response.headers.get("pagination") !== null) {
                paginatedResult.pagination = JSON.parse(response.headers.get("pagination"));
            }

            return paginatedResult;
        })
    );
}

export function getPaginationHeader(pageNumber: number, pageSize: Number) {
    let httpParams = new HttpParams();

    httpParams = httpParams
        .append('pageNumber', pageNumber.toString())
        .append('pageSize', pageSize.toString());

    return httpParams;
}