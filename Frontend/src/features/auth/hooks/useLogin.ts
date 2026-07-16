import { useMutation } from '@tanstack/react-query';
import { login } from '../api/auth';
import { useAuthStore } from '../../../store/useAuthStore';
import type { LoginCredentials, LoginResponse } from '../types';
import { useNavigate } from 'react-router-dom';

export const useLogin = () => {
  const loginAction = useAuthStore((state) => state.login);
  const navigate = useNavigate();

  return useMutation<LoginResponse, Error, LoginCredentials>({
    mutationFn: login,
    onSuccess: (data) => {
      loginAction(data.accessToken, data.refreshToken);
      navigate('/');
    },
  });
};
