import { useMutation, useQueryClient } from '@tanstack/react-query';
import { createRole } from '../api';
import type { CreateRoleDto } from '../types';

export const useCreateRole = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateRoleDto) => createRole(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['rolesList'] });
      queryClient.invalidateQueries({ queryKey: ['roles'] });
    },
  });
};
