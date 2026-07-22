import { apiClient } from '../../../config/apiClient';
import type { UserDto, PaginatedList, GetUsersParams } from '../types';

export const getUsers = async (params: GetUsersParams): Promise<PaginatedList<UserDto>> => {
  const { data } = await apiClient.post<PaginatedList<UserDto>>('/Users/search', params);
  return data;
};

export const getUser = async (id: string): Promise<UserDto> => {
  const { data } = await apiClient.get<UserDto>(`/Users/${id}`);
  return data;
};

export const updateUser = async (id: string, dto: { employeeId?: string | null; username: string; email: string; isActive: boolean }): Promise<void> => {
  await apiClient.put(`/Users/${id}`, dto);
};

export const assignRole = async (id: string, roleId: string): Promise<void> => {
  await apiClient.put(`/Users/${id}/role`, { roleId });
};

export const deleteUser = async (id: string): Promise<void> => {
  await apiClient.delete(`/Users/${id}`);
};
