export interface PaginatedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface DepartmentDto {
  id: string;
  name: string;
  description: string;
  employeeCount?: number;
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
  department?: DepartmentDto;
}

export interface CreateEmployeeDto {
  employeeNo: string;
  firstName: string;
  lastName: string;
  gender: number;
  dateOfBirth: string;
  nationality: string;
  profession: string;
  employmentStatus: number;
  hireDate: string;
  workingHoursPerWeek: number;
  vacationDaysTotal: number;
  departmentId?: string;
  addressLine: string;
  postalCode: string;
  city: string;
  state: string;
  country: string;
  baseSalary: number;
  salaryType: number;
  payGrade: string;
}

export interface CreatedEmployeeDto {
  id: string;
  employeeNo: string;
  hireDate: string;
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

export interface EmployeeAddressDto {
  id: string;
  addressLine: string;
  country: string;
  city: string;
  state: string;
  postalCode: string;
  isPrimary: boolean;
  startDate?: string;
  endDate?: string;
}

export interface EmployeeDetailDto {
  id: string;
  employeeNo: string;
  firstName: string;
  lastName: string;
  gender: number;
  dateOfBirth: string;
  nationality: string;
  profession: string;
  employmentStatus: number;
  hireDate: string;
  terminationDate?: string;
  probationEndDate?: string;
  workingHoursPerWeek: number;
  vacationDaysTotal: number;
  department?: DepartmentDto;
  
  addresses: EmployeeAddressDto[];
  compensations: any[];
  documents: any[];
  notes: any[];
  references: any[];
}

export interface UpdateEmployeeCommand {
  employeeId: string;
  firstName: string;
  lastName: string;
  gender: number;
  dateOfBirth: string;
  nationality: string;
  profession: string;
  employmentStatus: number;
  hireDate: string;
  terminationDate?: string | null;
  probationEndDate?: string | null;
  workingHoursPerWeek: number;
  vacationDaysTotal: number;
  departmentId?: string | null;
}

export interface UpdateEmployeeAddressCommand {
  employeeId: string;
  addressLine: string;
  postalCode: string;
  city: string;
  state: string;
  country: string;
  isPrimary: boolean;
}
