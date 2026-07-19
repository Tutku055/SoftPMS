import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useDepartmentDetail } from '../../hooks/useDepartmentDetail';
import { useUpdateDepartment } from '../../hooks/useUpdateDepartment';
import { useDeleteDepartment } from '../../hooks/useDeleteDepartment';
import { useEmployees } from '../../../employees/hooks/useEmployees';
import { DataTable } from '../../../../components/DataTable/DataTable';
import type { DataTableColumnDef } from '../../../../components/DataTable/DataTable';
import {
  Box,
  Typography,
  Stack,
  Button,
  Avatar,
  Tabs,
  Tab,
  Divider,
  TextField,
  IconButton,
  Tooltip,
  CircularProgress,
  Snackbar,
  Alert,
} from '@mui/material';
import {
  ArrowBackRounded,
  SaveRounded,
  BadgeRounded,
  PeopleAltRounded,
  FolderSharedRounded,
  OpenInNewRounded,
  BusinessRounded,
  DeleteRounded,
  WarningRounded
} from '@mui/icons-material';
import { PopupDialog } from '../../../../components/PopupDialog/PopupDialog';
import styles from './DepartmentDetail.module.css';

const glassPanelSx = {
  background: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(24, 24, 24, 0.85)' : 'rgba(255, 255, 255, 0.85)',
  backdropFilter: 'blur(12px)',
  border: '1px solid',
  borderColor: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(255, 255, 255, 0.06)' : 'rgba(0, 0, 0, 0.06)',
  borderRadius: '16px',
  p: 3,
  boxShadow: (theme: any) => theme.palette.mode === 'dark'
    ? '0 8px 32px rgba(0, 0, 0, 0.4), 0 1px 2px rgba(0, 0, 0, 0.2)'
    : '0 8px 32px rgba(0, 0, 0, 0.04), 0 1px 2px rgba(0, 0, 0, 0.02)',
  transition: 'box-shadow 0.3s ease',
};

const premiumInputSx = {
  '& .MuiOutlinedInput-root': {
    backgroundColor: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(18, 18, 18, 0.6)' : 'background.paper',
    borderRadius: '10px',
    transition: 'border-color 0.2s ease, box-shadow 0.2s ease',
    boxShadow: '0 1px 2px rgba(0, 0, 0, 0.02)',
    '&:hover': {
      boxShadow: (theme: any) => theme.palette.mode === 'dark' ? '0 2px 8px rgba(0, 0, 0, 0.2)' : '0 2px 8px rgba(0, 0, 0, 0.04)',
    },
    '&.Mui-focused': {
      boxShadow: '0 0 0 3px rgba(128, 128, 128, 0.1)',
    }
  },
  '& .MuiOutlinedInput-notchedOutline': {
    borderColor: 'rgba(128, 128, 128, 0.2) !important',
  },
  '& .Mui-focused .MuiOutlinedInput-notchedOutline': {
    borderColor: 'primary.main !important',
    borderWidth: '1px !important',
  }
};

const actionButtonSx = {
  borderRadius: '10px',
  borderColor: 'rgba(128, 128, 128, 0.25)',
  color: 'text.primary',
  fontWeight: 500,
  textTransform: 'none',
  backgroundColor: 'transparent',
  transition: 'all 0.2s ease',
  '&:hover': {
    backgroundColor: 'rgba(128, 128, 128, 0.06)',
    borderColor: 'rgba(128, 128, 128, 0.5)',
  }
};

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

const TabPanel = (props: TabPanelProps) => {
  const { children, value, index, ...other } = props;
  return (
    <div role="tabpanel" hidden={value !== index} {...other}>
      {value === index && <Box sx={{ pt: 3 }}>{children}</Box>}
    </div>
  );
};

export const DepartmentDetail = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState(0);

  const { data: department, isLoading, isError } = useDepartmentDetail(id);
  const { mutate: updateDepartment, isPending: isUpdatingDepartment } = useUpdateDepartment();
  const { mutate: deleteDepartment, isPending: isDeleting } = useDeleteDepartment();

  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');
  const [snackbarSeverity, setSnackbarSeverity] = useState<'success' | 'error'>('success');

  const [formState, setFormState] = useState({
    name: '',
    description: '',
  });

  const [employeePage, setEmployeePage] = useState({
    page: 0,
    pageSize: 10,
  });

  const { data: employeesData, isLoading: isEmployeesLoading, isFetching: isEmployeesFetching } = useEmployees({
    pageNumber: employeePage.page + 1,
    pageSize: employeePage.pageSize,
    filters: id ? [
      { field: 'departmentId', operator: 'equals', value: id },
      { field: 'employmentStatus', operator: 'is', value: '1' }
    ] : []
  });

  useEffect(() => {
    if (department) {
      setFormState({
        name: department.name || '',
        description: department.description || '',
      });
    }
  }, [department]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormState({ ...formState, [e.target.name]: e.target.value });
  };

  const handleSave = () => {
    if (!id) return;
    updateDepartment({
      id: id,
      ...formState,
    });
  };

  const handleTabChange = (_event: React.SyntheticEvent, newValue: number) => {
    setActiveTab(newValue);
  };

  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '60vh' }}>
        <CircularProgress />
      </Box>
    );
  }

  if (isError || !department) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '60vh', flexDirection: 'column', gap: 2 }}>
        <Typography variant="h6" color="error">Failed to load department details.</Typography>
        <Button variant="outlined" onClick={() => navigate('/departments/list')}>Go Back</Button>
      </Box>
    );
  }

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
    <Box className={styles.pageContainer}>
      
      {/* ── TOP ACTION BAR ────────────────────────────────────────────────── */}
      <Box className={styles.headerContainer}>
        <Stack direction="row" spacing={2} sx={{ alignItems: 'center' }}>
          <Tooltip title="Back to Departments">
            <IconButton onClick={() => navigate('/departments/list')} sx={{ bgcolor: 'action.hover' }}>
              <ArrowBackRounded />
            </IconButton>
          </Tooltip>
          <Box>
            <Typography 
              variant="h4" 
              sx={{ 
                fontWeight: 700, 
                letterSpacing: '-0.02em', 
                color: 'text.primary',
                textShadow: (theme) => theme.palette.mode === 'dark' ? '0 1px 2px rgba(0,0,0,0.5)' : '0 1px 2px rgba(0,0,0,0.05)'
              }}
            >
              Department Profile
            </Typography>
            <Typography variant="body2" color="text.secondary">
              ID: {department.id}
            </Typography>
          </Box>
        </Stack>

        <Stack direction="row" spacing={1.5}>
          <Button 
            onClick={() => setIsDeleteDialogOpen(true)}
            variant="outlined" 
            color="error"
            startIcon={<DeleteRounded />} 
            sx={{ borderRadius: '10px', fontWeight: 600, textTransform: 'none' }}
          >
            Delete
          </Button>
          <Button onClick={handleSave} disabled={isUpdatingDepartment} variant="contained" startIcon={<SaveRounded />} sx={{ borderRadius: '10px', fontWeight: 600, textTransform: 'none', boxShadow: 'none' }}>
            {isUpdatingDepartment ? 'Saving...' : 'Save Changes'}
          </Button>
        </Stack>
      </Box>

      {/* ── PROFILE SUMMARY CARD ────────────────────────────────────────── */}
      <Box sx={glassPanelSx}>
        <Box className={styles.profileSummaryGrid}>
          <Avatar sx={{ width: 84, height: 84, bgcolor: 'primary.main', fontSize: '2rem', fontWeight: 600 }}>
            <BusinessRounded fontSize="large" />
          </Avatar>
          <Box sx={{ flex: 1 }}>
            <Stack direction="row" spacing={2} sx={{ mb: 0.5, alignItems: 'center' }}>
              <Typography variant="h5" sx={{ fontWeight: 700 }}>{department.name}</Typography>
            </Stack>
            <Typography variant="body1" color="text.secondary" sx={{ fontWeight: 500, mb: 1 }}>
              {department.description}
            </Typography>
            <Stack direction="row" spacing={3} sx={{ mt: 1.5 }}>
              <Typography variant="caption" color="text.secondary">
                <strong>Total Employees:</strong> {department.employeeCount || 0}
              </Typography>
            </Stack>
          </Box>
        </Box>
      </Box>

      {/* ── NAVIGATION TABS ───────────────────────────────────────────────── */}
      <Box sx={{ borderBottom: 1, borderColor: 'divider', mt: 1 }}>
        <Tabs 
          value={activeTab} 
          onChange={handleTabChange}
          sx={{
            '& .MuiTab-root': { fontWeight: 600, textTransform: 'none', fontSize: '0.95rem', minHeight: '48px' },
          }}
        >
          <Tab icon={<BadgeRounded sx={{ mr: 1 }}/>} iconPosition="start" label="General Info" />
          <Tab icon={<PeopleAltRounded sx={{ mr: 1 }}/>} iconPosition="start" label="Employees" />
          <Tab icon={<FolderSharedRounded sx={{ mr: 1 }}/>} iconPosition="start" label="Documents" />
        </Tabs>
      </Box>

      {/* ── TAB 1: GENERAL INFO ────────────────────── */}
      <TabPanel value={activeTab} index={0}>
        <Box sx={glassPanelSx}>
          <Typography variant="h6" className={styles.sectionTitle}>
            Department Details
          </Typography>
          <Divider sx={{ mb: 3, opacity: 0.5 }} />
          
          <Box className={styles.formGrid}>
            <TextField label="Department Name" name="name" value={formState.name} onChange={handleChange} size="small" fullWidth sx={premiumInputSx} />
            <TextField label="Description" name="description" value={formState.description} onChange={handleChange} size="small" fullWidth sx={{ ...premiumInputSx, gridColumn: '1 / -1' }} multiline rows={3} />
          </Box>
        </Box>
      </TabPanel>

      {/* ── TAB 2: EMPLOYEES ───────────────────────────────── */}
      <TabPanel value={activeTab} index={1}>
        <Box sx={glassPanelSx}>
          <Typography variant="h6" className={styles.sectionTitle}>
            Active Employees
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
            This list is read-only. To manage assignments, go to the Employee module.
          </Typography>
          
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
              paginationModel={employeePage}
              onPaginationModelChange={setEmployeePage}
              customFilters={{}}
              onCustomFilterChange={() => {}}
              onRowClick={(employeeId) => navigate(`/employees/${employeeId}`)}
            />
          </Box>
        </Box>
      </TabPanel>

      {/* ── TAB 3: DOCUMENTS ──────────────────────────────────────────────── */}
      <TabPanel value={activeTab} index={2}>
        <Box className={styles.actionCardsGrid}>
          <Box sx={{ ...glassPanelSx, display: 'flex', flexDirection: 'column', gap: 2 }}>
            <Box>
              <Typography variant="h6" sx={{ fontWeight: 700 }}>Department Policies & Documents</Typography>
              <Typography variant="body2" color="text.secondary">View and manage documents associated with this department.</Typography>
            </Box>
            <Divider sx={{ opacity: 0.5 }} />
            <Stack direction="row" sx={{ justifyContent: 'space-between', alignItems: 'center' }}>
              <Typography variant="body1" sx={{ fontWeight: 600 }}>0 Active Documents</Typography>
              <Button variant="outlined" endIcon={<OpenInNewRounded />} sx={actionButtonSx}>
                Open Vault
              </Button>
            </Stack>
          </Box>
        </Box>
      </TabPanel>

      <PopupDialog
        open={isDeleteDialogOpen}
        onClose={() => setIsDeleteDialogOpen(false)}
        title="Delete Department"
        icon={<WarningRounded color="error" />}
        confirmColor="error"
        confirmText="Delete"
        isProcessing={isDeleting}
        onConfirm={() => {
          if (id) {
            deleteDepartment(id, {
              onSuccess: () => {
                setIsDeleteDialogOpen(false);
                navigate('/departments/list');
              },
              onError: (_error: any) => {
                setIsDeleteDialogOpen(false);
                setSnackbarMessage(`You can't delete ${department.name} Department`);
                setSnackbarSeverity('error');
                setSnackbarOpen(true);
              }
            });
          }
        }}
        content={
          <>
            Are you sure you want to delete the <strong>{department.name}</strong> department? 
            This action cannot be undone. If there are any employees assigned to this department, the deletion will be rejected.
          </>
        }
      />

      <Snackbar 
        open={snackbarOpen} 
        autoHideDuration={6000} 
        onClose={() => setSnackbarOpen(false)}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
      >
        <Alert onClose={() => setSnackbarOpen(false)} severity={snackbarSeverity} sx={{ width: '100%', fontWeight: 500 }}>
          {snackbarMessage}
        </Alert>
      </Snackbar>
    </Box>
  );
};
