import { useMutation, useQueryClient } from '@tanstack/react-query';
import { createEmployee } from '../api/employees';
import type { CreateEmployeeDto } from '../types';

export const useCreateEmployee = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (dto: CreateEmployeeDto) => createEmployee(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['employees'] });
    },
  });
};
