import { useMutation, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '../../../config/apiClient';
import type { UpdateEmployeeAddressCommand } from '../types';

export const useUpdateEmployeeAddress = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (command: UpdateEmployeeAddressCommand) => {
      const { data } = await apiClient.put(`/employees/${command.employeeId}/addresses/primary`, command);
      return data;
    },
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['employee', variables.employeeId] });
    },
  });
};