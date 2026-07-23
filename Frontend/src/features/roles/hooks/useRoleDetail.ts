import { useQuery } from '@tanstack/react-query';
import { getRoleById } from '../api';

export const useRoleDetail = (id?: string) => {
  return useQuery({
    queryKey: ['role', id],
    queryFn: () => {
      if (!id) throw new Error('Role ID is required');
      return getRoleById(id);
    },
    enabled: !!id,
  });
};
