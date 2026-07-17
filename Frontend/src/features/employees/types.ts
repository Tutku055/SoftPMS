export interface EmployeeDto {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
}

export interface CreateEmployeeDto {
  firstName: string;
  lastName: string;
  email: string;
}

export interface PaginatedList<T> {
  items: T[];
  totalCount: number;
  pageIndex: number;
  totalPages: number;
}
