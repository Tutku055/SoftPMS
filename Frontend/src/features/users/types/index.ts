export interface PaginatedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface RoleDto {
  id: string;
  name: string;
  description: string;
  color: string;
}

export interface UserDto {
  id: string;
  employeeId?: string | null;
  username: string;
  email: string;
  isActive: boolean;
  isSystemUser: boolean;
  role?: RoleDto;
}

export interface FilterCriteria {
  field: string;
  operator: string;
  value: string | null;
}

export interface GetUsersParams {
  pageNumber: number;
  pageSize: number;
  filters?: FilterCriteria[];
}
