import { useQuery } from '@tanstack/react-query';
import { apiClient } from '../../../config/apiClient';
import type { DepartmentDto, PaginatedList } from '../types';

export const useDepartments = () => {
  return useQuery({
    queryKey: ['departments'],
    queryFn: async () => {
      // Fetch departments with a large page size to get all of them for the selection box
      const response = await apiClient.get<PaginatedList<DepartmentDto>>('/departments', {
        params: {
          pageNumber: 1,
          pageSize: 1000,
        },
      });
      return response.data;
    },
  });
};
