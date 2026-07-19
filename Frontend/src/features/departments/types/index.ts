import type { FilterCriteria } from '../../employees/types';

export interface GetDepartmentsParams {
  pageNumber: number;
  pageSize: number;
  filters?: FilterCriteria[];
}

export interface UpdateDepartmentDto {
  id: string;
  name: string;
  description: string;
}
