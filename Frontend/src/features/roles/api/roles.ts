import { apiClient } from '../../../config/apiClient';
import type { RoleDto, CreateRoleDto } from '../types';

export const getRoles = async (): Promise<RoleDto[]> => {
  const { data } = await apiClient.get<RoleDto[]>('/Roles');
  return data;
};

export const createRole = async (role: CreateRoleDto): Promise<RoleDto> => {
  const { data } = await apiClient.post<RoleDto>('/Roles', role);
  return data;
};

export const deleteRole = async (id: string): Promise<void> => {
  await apiClient.delete(`/Roles/${id}`);
};
