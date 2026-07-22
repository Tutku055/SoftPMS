import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { documentsApi } from '../api/documentsApi';
import type { DocumentDto, PaginatedList } from '../api/documentsApi';

export const useDocuments = (params?: {
  referenceId?: string;
  ownerModule?: number;
  documentType?: number;
  minFileSizeBytes?: number;
  maxFileSizeBytes?: number;
  extension?: string;
  extensionOperator?: string;
  expiryDateStart?: string;
  expiryDateEnd?: string;
  uploadDateStart?: string;
  uploadDateEnd?: string;
  fileName?: string;
  fileNameOperator?: string;
  quickSearch?: string;
  pageNumber?: number;
  pageSize?: number;
}) => {
  return useQuery<PaginatedList<DocumentDto>>({
    queryKey: ['documents', params],
    queryFn: () => documentsApi.getDocuments(params),
  });
};

export const useUploadDocument = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (formData: FormData) => documentsApi.uploadDocument(formData),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['documents'] });
    },
  });
};

export const useDeleteDocument = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => documentsApi.deleteDocument(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['documents'] });
    },
  });
};
