import { HttpParams, HttpResponse } from '@angular/common/http';
import { signal } from '@angular/core';
import { PaginatedResult } from '../models/pagination';

/**
 * Updates a signal with paginated data and pagination metadata extracted from an HTTP response.
 *
 * @param response - The HTTP response containing the data and a 'Pagination' header.
 * @param paginatedResultSignal - The signal to update with the paginated result.
 *
 * @remark
 * Assumes the 'Pagination' header is present and contains valid JSON.
 */
export function setPaginatedResponse<T>(
  response: HttpResponse<T>,
  paginatedResultSignal: ReturnType<typeof signal<PaginatedResult<T> | null>>
) {
  paginatedResultSignal.set({
    items: response.body as T,
    pagination: JSON.parse(response.headers.get('Pagination')!),
  });
}

export function setPaginationHeaders(pageNumber: number, pageSize: number) {
  let params = new HttpParams();

  if (pageNumber && pageSize) {
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);
  }

  return params;
}
