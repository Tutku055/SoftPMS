import { useQuery } from '@tanstack/react-query';
import { getPermissions } from '../api';
import { useAuthStore } from '../../../store/useAuthStore';

export const usePermissionsList = () => {
  const userId = useAuthStore((state) => state.currentUser?.id);

  return useQuery({
    queryKey: ['permissions', userId],
    queryFn: async () => {
      const allPermissions = await getPermissions();
      return allPermissions;
    },
    staleTime: 0,
    refetchOnMount: 'always',
  });
};
