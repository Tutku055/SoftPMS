import type { CustomFilterValue } from '../../../components/DataTable/DataTable';

export const parseDocumentFilters = (columnFilters: Record<string, CustomFilterValue>) => {
  const activeDocType = columnFilters['documentType']?.value ? Number(columnFilters['documentType']?.value) : undefined;
  
  let minFileSizeBytes: number | undefined;
  let maxFileSizeBytes: number | undefined;
  const sizeFilter = columnFilters['fileSizeBytes'];
  if (sizeFilter && sizeFilter.value) {
    if (sizeFilter.operator === 'biggerthan') minFileSizeBytes = Number(sizeFilter.value);
    else if (sizeFilter.operator === 'smallerthan') maxFileSizeBytes = Number(sizeFilter.value);
  }

  let extension: string | undefined;
  let extensionOperator: string | undefined;
  const extFilter = columnFilters['extension'];
  if (extFilter && extFilter.value) {
    extension = extFilter.value;
    extensionOperator = extFilter.operator;
  }

  let expiryDateStart: string | undefined;
  let expiryDateEnd: string | undefined;
  const expFilter = columnFilters['expiryDate'];
  if (expFilter && expFilter.value) {
    if (expFilter.operator === 'before') {
      expiryDateEnd = expFilter.value;
    } else if (expFilter.operator === 'after') {
      expiryDateStart = expFilter.value;
    } else {
      expiryDateStart = expFilter.value;
      expiryDateEnd = expFilter.value;
    }
  }

  let uploadDateStart: string | undefined;
  let uploadDateEnd: string | undefined;
  const uploadFilter = columnFilters['createdAt'];
  if (uploadFilter && uploadFilter.value) {
    if (uploadFilter.operator === 'before') {
      uploadDateEnd = uploadFilter.value;
    } else if (uploadFilter.operator === 'after') {
      uploadDateStart = uploadFilter.value;
    } else {
      uploadDateStart = uploadFilter.value;
      uploadDateEnd = uploadFilter.value;
    }
  }

  let fileName: string | undefined;
  let fileNameOperator: string | undefined;
  const nameFilter = columnFilters['fileName'];
  if (nameFilter && nameFilter.value) {
    fileName = nameFilter.value;
    fileNameOperator = nameFilter.operator;
  }

  let isAvailable: boolean | undefined;
  const availFilter = columnFilters['isAvailable'];
  if (availFilter && availFilter.value) {
    if (availFilter.value === 'available') {
      isAvailable = availFilter.operator === 'not' ? false : true;
    } else if (availFilter.value === 'missing') {
      isAvailable = availFilter.operator === 'not' ? true : false;
    }
  }

  return {
    documentType: activeDocType,
    minFileSizeBytes,
    maxFileSizeBytes,
    extension,
    extensionOperator,
    expiryDateStart,
    expiryDateEnd,
    uploadDateStart,
    uploadDateEnd,
    fileName,
    fileNameOperator,
    isAvailable,
  };
};
