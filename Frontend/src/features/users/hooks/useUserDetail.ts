import { useQuery } from '@tanstack/react-query';
import { getUser } from '../api/users';

export const useUserDetail = (id?: string) => {
  return useQuery({
    queryKey: ['user', id],
    queryFn: () => getUser(id!),
    enabled: !!id,
  });
};
