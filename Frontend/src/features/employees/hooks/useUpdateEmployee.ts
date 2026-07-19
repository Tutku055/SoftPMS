import { useMutation, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '../../../config/apiClient';
import type { UpdateEmployeeCommand } from '../types';

export const useUpdateEmployee = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (command: UpdateEmployeeCommand) => {
      const { data } = await apiClient.put(`/employees/${command.employeeId}`, command);
      return data;
    },
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['employee', variables.employeeId] });
      queryClient.invalidateQueries({ queryKey: ['employees'] });
    },
  });
};