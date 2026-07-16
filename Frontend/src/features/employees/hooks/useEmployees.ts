import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getEmployees, getEmployee, createEmployee, deleteEmployee } from '../api/employees';
import type { CreateEmployeeDto } from '../types';

export const useGetEmployees = (page = 1, pageSize = 20) => {
  return useQuery({
    queryKey: ['employees', { page, pageSize }],
    queryFn: () => getEmployees(page, pageSize),
  });
};

export const useGetEmployee = (id: string) => {
  return useQuery({
    queryKey: ['employee', id],
    queryFn: () => getEmployee(id),
    enabled: !!id,
  });
};

export const useCreateEmployee = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateEmployeeDto) => createEmployee(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['employees'] });
    },
  });
};

export const useDeleteEmployee = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => deleteEmployee(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['employees'] });
    },
  });
};
