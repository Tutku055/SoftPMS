import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Typography,
  Stack,
  TextField,
  MenuItem,
  CircularProgress,
} from '@mui/material';
import { DataTable } from '../../../../components/DataTable/DataTable';
import type { DataTableColumnDef } from '../../../../components/DataTable/DataTable';
import { useEmployees } from '../../../employees/hooks/useEmployees';
import { useDepartmentsLookup } from '../../hooks/useDepartmentsLookup';

const premiumInputSx = {
  '& .MuiOutlinedInput-root': {
    backgroundColor: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(18, 18, 18, 0.6)' : 'background.paper',
    borderRadius: '10px',
    minWidth: '250px',
    transition: 'border-color 0.2s ease, box-shadow 0.2s ease',
    boxShadow: '0 1px 2px rgba(0, 0, 0, 0.02)',
    '&:hover': {
      boxShadow: (theme: any) => theme.palette.mode === 'dark' ? '0 2px 8px rgba(0, 0, 0, 0.2)' : '0 2px 8px rgba(0, 0, 0, 0.04)',
    },
    '&.Mui-focused': {
      boxShadow: '0 0 0 3px rgba(128, 128, 128, 0.1)',
    }
  },
};

export const DepartmentEmployees = () => {
  const navigate = useNavigate();
  const [selectedDepartmentId, setSelectedDepartmentId] = useState<string>('');

  const [paginationModel, setPaginationModel] = useState({
    page: 0,
    pageSize: 10,
  });

  const { data: departmentsLookup, isLoading: isDepartmentsLoading } = useDepartmentsLookup();

  // Create filters array
  const filters = [];
  if (selectedDepartmentId) {
    filters.push({ field: 'departmentId', operator: 'equals', value: selectedDepartmentId });
  }

  const { data: employeesData, isLoading: isEmployeesLoading, isFetching: isEmployeesFetching } = useEmployees({
    pageNumber: paginationModel.page + 1,
    pageSize: paginationModel.pageSize,
    filters
  });

  const employeeColumns: DataTableColumnDef[] = [
    {
      field: 'employeeNo',
      headerName: 'Employee No',
      flex: 1,
      minWidth: 150,
      filterType: 'text',
    },
    {
      field: 'fullName',
      headerName: 'Full Name',
      flex: 1.5,
      minWidth: 200,
      filterType: 'text',
      valueGetter: (_, row: any) => `${row.firstName} ${row.lastName}`,
    },
    {
      field: 'profession',
      headerName: 'Profession',
      flex: 1.5,
      minWidth: 200,
      filterType: 'text',
      renderCell: (params) => (
        <Typography variant="body2" sx={{ fontWeight: 500, color: 'text.secondary' }}>
          {params.value}
        </Typography>
      )
    }
  ];

  return (
    <Box sx={{ p: { xs: 2, md: 4 }, maxWidth: 1536, margin: '0 auto' }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-end', mb: 3, flexWrap: 'wrap', gap: 2 }}>
        <Box>
          <Typography
            variant="h4"
            component="h1"
            sx={{
              fontWeight: 700,
              letterSpacing: '-0.02em',
              color: 'text.primary',
              textShadow: (theme) => theme.palette.mode === 'dark' 
                ? '0 1px 2px rgba(0,0,0,0.5)' 
                : '0 1px 2px rgba(0,0,0,0.05)',
            }}
          >
            Department Employees
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5 }}>
            View and filter employees by their assigned departments.
          </Typography>
        </Box>

        <Stack direction="row" spacing={1.5} sx={{ alignItems: 'center' }}>
          {isDepartmentsLoading ? (
            <CircularProgress size={24} />
          ) : (
            <TextField
              select
              label="Filter by Department"
              size="small"
              value={selectedDepartmentId}
              onChange={(e) => {
                setSelectedDepartmentId(e.target.value);
                setPaginationModel(prev => ({ ...prev, page: 0 })); // Reset page on filter
              }}
              sx={premiumInputSx}
            >
              <MenuItem value="">
                <em>All Departments</em>
              </MenuItem>
              {departmentsLookup?.map((dept) => (
                <MenuItem key={dept.id} value={dept.id}>
                  {dept.name}
                </MenuItem>
              ))}
            </TextField>
          )}
        </Stack>
      </Box>

      <Box 
        sx={{
          borderRadius: 4,
          overflow: 'hidden',
          backgroundColor: 'background.paper',
          border: '1px solid',
          borderColor: 'divider',
          boxShadow: '0 4px 24px rgba(0, 0, 0, 0.03)',
        }}
      >
        <DataTable
          data={employeesData?.items || []}
          totalCount={employeesData?.totalCount || 0}
          loading={isEmployeesLoading || isEmployeesFetching}
          columns={employeeColumns}
          paginationModel={paginationModel}
          onPaginationModelChange={setPaginationModel}
          customFilters={{}}
          onCustomFilterChange={() => {}}
          onRowClick={(employeeId) => navigate(`/employees/${employeeId}`)}
        />
      </Box>
    </Box>
  );
};
