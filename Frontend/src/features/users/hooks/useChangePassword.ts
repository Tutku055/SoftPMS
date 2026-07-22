import { useMutation } from '@tanstack/react-query';
import { apiClient } from '../../../config/apiClient';

interface ChangePasswordDto {
  oldPassword: string;
  newPassword: string;
}

export const useChangePassword = () => {
  return useMutation({
    mutationFn: async ({ userId, data }: { userId: string; data: ChangePasswordDto }) => {
      const response = await apiClient.post(`/Users/${userId}/change-password`, data);
      return response.data;
    },
  });
};
