import { useQuery } from '@tanstack/react-query';
import { apiClient as api } from '../../../config/apiClient';
import type { DocumentDto } from '../api/documentsApi';

export const useDocumentDetail = (id: string | undefined) => {
  return useQuery({
    queryKey: ['document', id],
    queryFn: async (): Promise<DocumentDto> => {
      const { data } = await api.get(`/documents/${id}`);
      return data;
    },
    enabled: !!id,
  });
};
