import { useMutation, useQueryClient } from '@tanstack/react-query';
import { updateDepartment } from '../api';
import type { UpdateDepartmentDto } from '../types';

export const useUpdateDepartment = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UpdateDepartmentDto) => updateDepartment(data),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['department', variables.id] });
      queryClient.invalidateQueries({ queryKey: ['departmentsList'] });
      queryClient.invalidateQueries({ queryKey: ['departments'] });
    },
  });
};
