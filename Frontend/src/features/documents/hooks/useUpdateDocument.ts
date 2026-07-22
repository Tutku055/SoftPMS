import { useMutation, useQueryClient } from '@tanstack/react-query';
import { documentsApi } from '../api/documentsApi';

export const useUpdateDocument = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (data: { id: string } & any) => {
      await documentsApi.updateDocument(data.id, data);
    },
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['documents'] });
      queryClient.invalidateQueries({ queryKey: ['document', variables.id] });
    },
  });
};
