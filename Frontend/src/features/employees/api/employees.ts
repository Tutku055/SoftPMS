import { apiClient } from '../../../config/apiClient';
import type { EmployeeDto, PaginatedList, CreateEmployeeDto } from '../types';

export const getEmployees = async (page = 1, pageSize = 20): Promise<PaginatedList<EmployeeDto>> => {
  const { data } = await apiClient.get<PaginatedList<EmployeeDto>>(`/Employees?pageNumber=${page}&pageSize=${pageSize}`);
  return data;
};

export const getEmployee = async (id: string): Promise<EmployeeDto> => {
  const { data } = await apiClient.get<EmployeeDto>(`/Employees/${id}`);
  return data;
};

export const createEmployee = async (employee: CreateEmployeeDto): Promise<EmployeeDto> => {
  const { data } = await apiClient.post<EmployeeDto>('/Employees', employee);
  return data;
};

export const deleteEmployee = async (id: string): Promise<void> => {
  await apiClient.delete(`/Employees/${id}`);
};
