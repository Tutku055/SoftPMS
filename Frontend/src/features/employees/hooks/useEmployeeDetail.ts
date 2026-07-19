import { useQuery } from '@tanstack/react-query';
import { apiClient } from '../../../config/apiClient';
import type { EmployeeDetailDto } from '../types';

export const useEmployeeDetail = (id: string | undefined) => {
  return useQuery({
    queryKey: ['employee', id],
    queryFn: async () => {
      if (!id) return null;
      const { data } = await apiClient.get<EmployeeDetailDto>(`/employees/${id}`);
      return data;
    },
    enabled: !!id,
  });
};
