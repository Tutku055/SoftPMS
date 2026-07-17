// ActiveRosterTable.tsx
import {
  DataGrid,
} from '@mui/x-data-grid';
import type {
  GridColDef,
  GridPaginationModel,
  GridColumnVisibilityModel,
  GridColumnHeaderParams,
} from '@mui/x-data-grid';
import { Box, Chip, Stack, TextField, MenuItem, Typography } from '@mui/material';
import type { EmployeeDto } from '../types';

export type CustomFilterValue = { value: string; operator: string };

interface ActiveRosterTableProps {
  data: EmployeeDto[];
  totalCount: number;
  loading: boolean;
  paginationModel: GridPaginationModel;
  onPaginationModelChange: (model: GridPaginationModel) => void;
  columnVisibilityModel?: GridColumnVisibilityModel;
  onColumnVisibilityModelChange?: (model: GridColumnVisibilityModel) => void;
  customFilters: Record<string, CustomFilterValue>;
  onCustomFilterChange: (field: string, value: string, operator: string) => void;
}

const STRING_OPERATORS = [
  { value: 'contains', label: 'Contains' },
  { value: 'equals', label: 'Equals' },
  { value: 'startswith', label: 'Starts with' },
  { value: 'endswith', label: 'Ends with' },
];

const DATE_OPERATORS = [
  { value: 'is', label: 'Is' },
  { value: 'after', label: 'After' },
  { value: 'before', label: 'Before' },
];

const SELECT_OPERATORS = [
  { value: 'is', label: 'Is' },
  { value: 'not', label: 'Is Not' },
];

// Premium Input Stilleri
const premiumInputSx = {
  '& .MuiOutlinedInput-root': {
    fontSize: '0.75rem',
    borderRadius: '6px',
    backgroundColor: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(255, 255, 255, 0.04)' : 'rgba(0, 0, 0, 0.02)',
    height: '28px',
    transition: 'all 0.2s ease',
    '& fieldset': {
      borderColor: 'transparent',
      transition: 'all 0.2s ease',
    },
    '&:hover fieldset': {
      borderColor: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(255, 255, 255, 0.15)' : 'rgba(0, 0, 0, 0.1)',
    },
    '&.Mui-focused': {
      backgroundColor: 'background.paper',
      boxShadow: (theme: any) => theme.palette.mode === 'dark' 
        ? '0 2px 8px rgba(0,0,0,0.4)' 
        : '0 2px 6px rgba(0,0,0,0.05)',
      '& fieldset': {
        borderColor: 'primary.main',
        borderWidth: '1px',
      }
    }
  },
  // Seçim kutularındaki aşağı ok simgesini tamamen yok etme
  '& .MuiSelect-icon': { 
    display: 'none !important' 
  },
  // Ok kalktığı için sağ tarafta kalan gereksiz boşluğu sıfırlama (Yazının sığmasını sağlar)
  '& .MuiSelect-select': { 
    paddingRight: '8px !important',
    paddingLeft: '8px !important',
  }
};

const renderHeaderWithFilter = (
  field: string,
  headerName: string,
  filterType: 'text' | 'date' | 'select',
  customFilters: Record<string, CustomFilterValue>,
  onCustomFilterChange: (field: string, value: string, operator: string) => void,
  options?: { value: string; label: string }[]
) => {
  return (params: GridColumnHeaderParams) => {
    const filterState = customFilters[field] || { value: '', operator: filterType === 'text' ? 'contains' : 'is' };
    const { value, operator } = filterState;

    const opList = filterType === 'text' ? STRING_OPERATORS : filterType === 'date' ? DATE_OPERATORS : SELECT_OPERATORS;

    return (
      <Stack spacing={0.5} sx={{ width: '100%', pt: 1, pb: 0.5, height: '100%', justifyContent: 'flex-end' }}>
        <Typography 
          variant="subtitle2" 
          sx={{ 
            fontWeight: 600, 
            lineHeight: 1,
            color: value ? 'primary.main' : 'text.primary',
            letterSpacing: '-0.01em',
            whiteSpace: 'nowrap',
            overflow: 'hidden',
            textOverflow: 'ellipsis',
            transition: 'color 0.2s ease',
          }}
        >
          {headerName}
        </Typography>
        
        <Box sx={{ display: 'flex', gap: 0.5 }}>
          {/* Operator Selector (Genişlik 92px'e çıkarıldı, oklar gizlendi) */}
          <TextField
            select
            size="small"
            value={operator}
            onChange={(e) => onCustomFilterChange(field, value, e.target.value)}
            onClick={(e) => e.stopPropagation()}
            onKeyDown={(e) => e.stopPropagation()}
            sx={{
              width: '92px',
              flexShrink: 0,
              ...premiumInputSx,
              '& .MuiSelect-select': { py: 0.5, display: 'flex', alignItems: 'center' }
            }}
          >
            {opList.map((opt) => (
              <MenuItem key={opt.value} value={opt.value} sx={{ fontSize: '0.75rem', fontWeight: 500 }}>
                {opt.label}
              </MenuItem>
            ))}
          </TextField>

          {/* Value Input */}
          {filterType === 'select' && options ? (
            <TextField
              select
              size="small"
              value={value}
              onChange={(e) => onCustomFilterChange(field, e.target.value, operator)}
              onClick={(e) => e.stopPropagation()}
              onKeyDown={(e) => e.stopPropagation()}
              sx={{
                flex: 1,
                ...premiumInputSx,
                '& .MuiSelect-select': { py: 0.5, display: 'flex', alignItems: 'center' }
              }}
            >
              <MenuItem value="" sx={{ fontSize: '0.75rem', fontWeight: 500 }}>All</MenuItem>
              {options.map((opt) => (
                <MenuItem key={opt.value} value={opt.value} sx={{ fontSize: '0.75rem', fontWeight: 500 }}>
                  {opt.label}
                </MenuItem>
              ))}
            </TextField>
          ) : filterType === 'date' ? (
            <TextField
              type="date"
              size="small"
              value={value}
              onChange={(e) => onCustomFilterChange(field, e.target.value, operator)}
              onClick={(e) => e.stopPropagation()}
              onKeyDown={(e) => e.stopPropagation()}
              sx={{
                flex: 1,
                ...premiumInputSx,
                '& input': { py: 0.5, px: 1, fontSize: '0.75rem' }
              }}
            />
          ) : (
            <TextField
              size="small"
              placeholder={`Value...`}
              value={value}
              onChange={(e) => onCustomFilterChange(field, e.target.value, operator)}
              onClick={(e) => e.stopPropagation()}
              onKeyDown={(e) => e.stopPropagation()}
              sx={{
                flex: 1,
                ...premiumInputSx,
                '& input': { py: 0.5, px: 1, fontSize: '0.75rem', '&::placeholder': { opacity: 0.5 } }
              }}
            />
          )}
        </Box>
      </Stack>
    );
  };
};

export const ActiveRosterTable = ({
  data,
  totalCount,
  loading,
  paginationModel,
  onPaginationModelChange,
  columnVisibilityModel,
  onColumnVisibilityModelChange,
  customFilters,
  onCustomFilterChange,
}: ActiveRosterTableProps) => {

  // Kolonların minWidth değerleri yan yana duran iki kutunun sıkışmaması için 185px-190px bandına çekildi
  const columns: GridColDef[] = [
    {
      field: 'employeeNo',
      headerName: 'Employee No',
      flex: 1,
      minWidth: 185,
      renderHeader: renderHeaderWithFilter('employeeNo', 'Employee No', 'text', customFilters, onCustomFilterChange),
    },
    {
      field: 'firstName',
      headerName: 'First Name',
      flex: 1,
      minWidth: 185,
      renderHeader: renderHeaderWithFilter('firstName', 'First Name', 'text', customFilters, onCustomFilterChange),
    },
    {
      field: 'lastName',
      headerName: 'Last Name',
      flex: 1,
      minWidth: 185,
      renderHeader: renderHeaderWithFilter('lastName', 'Last Name', 'text', customFilters, onCustomFilterChange),
    },
    {
      field: 'profession',
      headerName: 'Profession',
      flex: 1.5,
      minWidth: 195,
      renderHeader: renderHeaderWithFilter('profession', 'Profession', 'text', customFilters, onCustomFilterChange),
      renderCell: (params) => (
        <Typography variant="body2" sx={{ fontWeight: 500, color: 'text.secondary' }}>
          {params.value}
        </Typography>
      )
    },
    {
      field: 'employmentStatus',
      headerName: 'Status',
      flex: 1,
      minWidth: 185,
      renderHeader: renderHeaderWithFilter('employmentStatus', 'Status', 'select', customFilters, onCustomFilterChange, [
        { value: '1', label: 'Active' },
        { value: '2', label: 'On Leave' },
        { value: '3', label: 'Terminated' },
      ]),
      renderCell: (params) => {
        const val = params.value as number;
        
        const getChipStyles = (status: number) => {
          switch(status) {
            case 1:
              return {
                bg: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(46, 125, 50, 0.15)' : 'rgba(46, 125, 50, 0.08)',
                color: (theme: any) => theme.palette.mode === 'dark' ? '#81c784' : '#2e7d32',
                border: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(129, 199, 132, 0.2)' : 'rgba(46, 125, 50, 0.15)',
                label: 'Active'
              };
            case 2:
              return {
                bg: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(237, 108, 2, 0.15)' : 'rgba(237, 108, 2, 0.08)',
                color: (theme: any) => theme.palette.mode === 'dark' ? '#ffb74d' : '#ed6c02',
                border: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(255, 183, 77, 0.2)' : 'rgba(237, 108, 2, 0.15)',
                label: 'On Leave'
              };
            case 3:
              return {
                bg: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(211, 47, 47, 0.15)' : 'rgba(211, 47, 47, 0.08)',
                color: (theme: any) => theme.palette.mode === 'dark' ? '#e57373' : '#d32f2f',
                border: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(229, 115, 115, 0.2)' : 'rgba(211, 47, 47, 0.15)',
                label: 'Terminated'
              };
            default:
              return {
                bg: 'transparent',
                color: 'text.secondary',
                border: 'divider',
                label: 'Unknown'
              };
          }
        };

        const styles = getChipStyles(val);

        return (
          <Chip
            label={styles.label}
            size="small"
            sx={{
              fontWeight: 600,
              fontSize: '0.75rem',
              backgroundColor: styles.bg,
              color: styles.color,
              border: '1px solid',
              borderColor: styles.border,
              borderRadius: '6px',
              height: '24px',
            }}
          />
        );
      },
    },
    {
      field: 'hireDate',
      headerName: 'Hire Date',
      flex: 1,
      minWidth: 185,
      renderHeader: renderHeaderWithFilter('hireDate', 'Hire Date', 'date', customFilters, onCustomFilterChange),
      valueGetter: (value: string | null | undefined) => {
        if (!value) return null;
        return new Date(value);
      },
      valueFormatter: (value: Date | null | undefined) => {
        if (!value) return '';
        return new Date(value).toLocaleDateString();
      },
    },
  ];

  return (
    <Box sx={{ width: '100%' }}>
      <DataGrid
        rows={data}
        columns={columns}
        getRowId={(row) => row.id}
        loading={loading}
        rowCount={totalCount}
        autoHeight
        paginationMode="server"
        paginationModel={paginationModel}
        onPaginationModelChange={onPaginationModelChange}
        pageSizeOptions={[5, 10, 25, 50]}
        columnHeaderHeight={84}
        columnVisibilityModel={columnVisibilityModel}
        onColumnVisibilityModelChange={onColumnVisibilityModelChange}
        disableRowSelectionOnClick
        disableMultipleRowSelection
        disableColumnMenu
        sx={{
          border: 'none',
          backgroundColor: 'transparent',
          '& .MuiDataGrid-columnHeaders': {
            backgroundColor: (theme) => theme.palette.mode === 'dark' ? 'rgba(255, 255, 255, 0.02)' : 'rgba(0, 0, 0, 0.01)',
            borderBottom: '1px solid',
            borderColor: 'divider',
          },
          '& .MuiDataGrid-columnHeader:focus, & .MuiDataGrid-columnHeader:focus-within': {
            outline: 'none',
          },
          '& .MuiDataGrid-cell': {
            borderBottom: '1px solid',
            borderColor: (theme) => theme.palette.mode === 'dark' ? 'rgba(255, 255, 255, 0.05)' : 'rgba(0, 0, 0, 0.04)',
            fontSize: '0.875rem',
          },
          '& .MuiDataGrid-cell:focus, & .MuiDataGrid-cell:focus-within': {
            outline: 'none',
          },
          '& .MuiDataGrid-row': {
            transition: 'background-color 0.2s ease',
          },
          '& .MuiDataGrid-row:hover': {
            backgroundColor: (theme) => theme.palette.mode === 'dark' ? 'rgba(255, 255, 255, 0.03)' : 'rgba(0, 0, 0, 0.02)',
          },
          '& .MuiDataGrid-footerContainer': {
            borderTop: '1px solid',
            borderColor: 'divider',
            backgroundColor: (theme) => theme.palette.mode === 'dark' ? 'rgba(255, 255, 255, 0.01)' : 'transparent',
          }
        }}
      />
    </Box>
  );
};