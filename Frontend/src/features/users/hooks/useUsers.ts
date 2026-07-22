import { useQuery } from '@tanstack/react-query';
import { getUsers } from '../api/users';
import type { GetUsersParams } from '../types';

export const useUsers = (params: GetUsersParams) => {
  return useQuery({
    queryKey: ['users', params],
    queryFn: () => getUsers(params),
    placeholderData: (previousData) => previousData,
  });
};
