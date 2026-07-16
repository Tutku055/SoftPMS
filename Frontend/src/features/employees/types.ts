export interface EmployeeDto {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  profession: string;
  city: string;
  employmentStatus: number; // Enum value
  rate?: number;
}

export interface PaginatedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface CreateEmployeeDto {
  firstName: string;
  lastName: string;
  email: string;
  profession: string;
  city: string;
  employmentStatus: number;
  rate?: number;
}
