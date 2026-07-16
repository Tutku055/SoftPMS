import { apiClient } from '../../../config/apiClient';
import type { UserDto, CreateUserDto } from '../types';

export const getUsers = async (): Promise<UserDto[]> => {
  const { data } = await apiClient.get<UserDto[]>('/Users');
  return data;
};

export const createUser = async (user: CreateUserDto): Promise<UserDto> => {
  const { data } = await apiClient.post<UserDto>('/Users', user);
  return data;
};

export const deleteUser = async (id: string): Promise<void> => {
  await apiClient.delete(`/Users/${id}`);
};
