import { apiClient } from '../../../config/apiClient';
import type { LoginCredentials, LoginResponse } from '../types';

export const login = async (credentials: LoginCredentials): Promise<LoginResponse> => {
  const { data } = await apiClient.post<LoginResponse>('/Auth/login', credentials);
  return data;
};
