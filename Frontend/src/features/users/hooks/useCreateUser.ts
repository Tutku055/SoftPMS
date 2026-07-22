import { useMutation, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '../../../config/apiClient';

interface CreateUserDto {
  username: string;
  email: string;
  password?: string;
  roleId: string;
  isActive: boolean;
}

export const useCreateUser = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (dto: CreateUserDto) => {
      const response = await apiClient.post('/Users', dto);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['users'] });
    },
  });
};
