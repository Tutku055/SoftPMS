export interface PaginatedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface EmployeeDto {
  id: string;
  employeeNo: string;
  firstName: string;
  lastName: string;
  profession: string;
  employmentStatus: number;
  hireDate: string;
  terminationDate?: string;
  probationEndDate?: string;
  gender?: number;
  workingHoursPerWeek?: number;
  vacationDaysTotal?: number;
  email?: string;
  phone?: string;
  city?: string;
  rate?: number;
}

export interface CreateEmployeeDto {
  firstName: string;
  lastName: string;
  email: string;
}

export interface FilterCriteria {
  field: string;
  operator: string;
  value: string | null;
}

export interface GetEmployeesParams {
  pageNumber: number;
  pageSize: number;
  filters?: FilterCriteria[];
}
