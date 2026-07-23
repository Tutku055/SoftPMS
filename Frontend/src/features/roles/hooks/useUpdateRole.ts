import { useMutation, useQueryClient } from '@tanstack/react-query';
import { updateRole } from '../api';
import type { UpdateRoleDto } from '../types';

export const useUpdateRole = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UpdateRoleDto) => updateRole(data),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['role', variables.id] });
      queryClient.invalidateQueries({ queryKey: ['roles'] });
    },
  });
};
