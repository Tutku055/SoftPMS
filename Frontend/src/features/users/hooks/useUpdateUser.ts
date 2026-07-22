import { useMutation, useQueryClient } from '@tanstack/react-query';
import { updateUser, assignRole } from '../api/users';

interface UpdateUserVariables {
  id: string;
  dto: {
    employeeId?: string | null;
    username: string;
    email: string;
    isActive: boolean;
  };
  roleId?: string;
}

export const useUpdateUser = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async ({ id, dto, roleId }: UpdateUserVariables) => {
      await updateUser(id, dto);
      if (roleId) {
        await assignRole(id, roleId);
      }
    },
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['users'] });
      queryClient.invalidateQueries({ queryKey: ['user', variables.id] });
    },
  });
};
