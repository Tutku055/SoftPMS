import { useQuery } from '@tanstack/react-query';
import { apiClient } from '../../../config/apiClient';

export interface DepartmentLookupDto {
  id: string;
  name: string;
}

export const getDepartmentsLookup = async (): Promise<DepartmentLookupDto[]> => {
  const { data } = await apiClient.get<DepartmentLookupDto[]>('/Departments/lookup');
  return data;
};

export const useDepartmentsLookup = () => {
  return useQuery({
    queryKey: ['departments', 'lookup'],
    queryFn: getDepartmentsLookup,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
};
