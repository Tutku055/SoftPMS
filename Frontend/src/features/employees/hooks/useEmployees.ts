import { useQuery } from '@tanstack/react-query';
import { getEmployees } from '../api/employees';
import type { GetEmployeesParams } from '../types';

export const useEmployees = (params: GetEmployeesParams) => {
  return useQuery({
    queryKey: ['employees', params],
    queryFn: () => getEmployees(params),
    placeholderData: (previousData) => previousData,
  });
};
