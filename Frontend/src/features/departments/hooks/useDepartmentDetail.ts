import { useQuery } from '@tanstack/react-query';
import { getDepartment } from '../api';

export const useDepartmentDetail = (id: string | undefined) => {
  return useQuery({
    queryKey: ['department', id],
    queryFn: () => getDepartment(id!),
    enabled: !!id,
  });
};
