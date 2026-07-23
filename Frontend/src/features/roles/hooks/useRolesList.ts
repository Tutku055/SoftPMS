import { useQuery } from '@tanstack/react-query';
import { getRolesList } from '../api';
import type { GetRolesParams } from '../types';

export const useRolesList = (params: GetRolesParams) => {
  return useQuery({
    queryKey: ['rolesList', params],
    queryFn: () => getRolesList(params),
    placeholderData: (previousData) => previousData,
  });
};
