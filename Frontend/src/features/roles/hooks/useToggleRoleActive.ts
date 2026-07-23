import { useMutation, useQueryClient } from '@tanstack/react-query';
import { toggleRoleActive } from '../api';

export const useToggleRoleActive = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => toggleRoleActive(id),
    onSuccess: (_, id) => {
      queryClient.invalidateQueries({ queryKey: ['role', id] });
      queryClient.invalidateQueries({ queryKey: ['roles'] });
    },
  });
};
