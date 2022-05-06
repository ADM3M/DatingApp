export interface IPagination {
    currentPage: number;
    itemsPerPage: number;
    totalPages: number;
    totalItems: number;
}

export class PaginatedResult<T> {
    result: T;
    pagination: IPagination;
}