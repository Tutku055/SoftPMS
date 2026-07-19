import { useQuery } from '@tanstack/react-query';
import { getDepartmentsList } from '../api';
import type { GetDepartmentsParams } from '../types';

export const useDepartmentsList = (params: GetDepartmentsParams) => {
  return useQuery({
    queryKey: ['departmentsList', params],
    queryFn: () => getDepartmentsList(params),
    placeholderData: (previousData) => previousData,
  });
};
