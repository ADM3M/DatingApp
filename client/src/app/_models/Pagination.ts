export interface IPagination {
    currentPage: number;
    itemsPerPage: number;
    totalPages: number;
    totaItems: number;
}

export class PaginatedResult<T> {
    result: T;
    pagination: IPagination;
}