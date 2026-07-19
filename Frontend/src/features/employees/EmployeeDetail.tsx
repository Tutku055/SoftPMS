// EmployeeDetail.tsx
import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useEmployeeDetail } from './hooks/useEmployeeDetail';
import { useUpdateEmployee } from './hooks/useUpdateEmployee';
import { useUpdateEmployeeAddress } from './hooks/useUpdateEmployeeAddress';
import { useDepartments } from './hooks/useDepartments';
import {
  Box,
  Typography,
  Stack,
  Button,
  Avatar,
  Chip,
  Tabs,
  Tab,
  Divider,
  TextField,
  MenuItem,
  IconButton,
  Tooltip,
  CircularProgress,
} from '@mui/material';
import {
  ArrowBackRounded,
  SaveRounded,
  PersonRemoveRounded,
  BadgeRounded,
  AccountBalanceWalletRounded,
  FolderSharedRounded,
  ContactsRounded,
  OpenInNewRounded,
} from '@mui/icons-material';
import styles from './EmployeeDetail.module.css';

// ─── PREMIUM TEMA OBJELERİ (Kaynak CSS ile Birebir Aynı) ──────────────

// .glassPanel sınıfının birebir sx karşılığı[cite: 9]
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

// .premiumInput sınıfının birebir sx karşılığı[cite: 9]
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

// .actionButton sınıfının birebir sx karşılığı[cite: 9]
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

// ──────────────────────────────────────────────────────────────────────────

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

export const EmployeeDetail = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState(0);

  const { data: employee, isLoading, isError } = useEmployeeDetail(id);
  const { mutate: updateEmployee, isPending: isUpdatingEmployee } = useUpdateEmployee();
  const { mutate: updateAddress, isPending: isUpdatingAddress } = useUpdateEmployeeAddress();
  const { data: deptData } = useDepartments();
  const departmentOptions = deptData?.items.map((d: any) => ({ value: d.id, label: d.name })) || [];

  const [formState, setFormState] = useState({
    firstName: '',
    lastName: '',
    gender: 0,
    dateOfBirth: '',
    nationality: '',
    profession: '',
    employmentStatus: 1,
    hireDate: '',
    probationEndDate: '',
    workingHoursPerWeek: 0,
    vacationDaysTotal: 0,
    departmentId: '',
  });

  const [addressState, setAddressState] = useState({
    addressLine: '',
    city: '',
    state: '',
    postalCode: '',
    country: '',
  });

  useEffect(() => {
    if (employee) {
      setFormState({
        firstName: employee.firstName || '',
        lastName: employee.lastName || '',
        gender: employee.gender ?? 0,
        dateOfBirth: employee.dateOfBirth?.split('T')[0] || '',
        nationality: employee.nationality || '',
        profession: employee.profession || '',
        employmentStatus: employee.employmentStatus || 1,
        hireDate: employee.hireDate?.split('T')[0] || '',
        probationEndDate: employee.probationEndDate?.split('T')[0] || '',
        workingHoursPerWeek: employee.workingHoursPerWeek || 0,
        vacationDaysTotal: employee.vacationDaysTotal || 0,
        departmentId: employee.department?.id || '',
      });

      const primaryAddress = employee.addresses?.find(a => a.isPrimary) 
        || employee.addresses?.[employee.addresses.length - 1];
      if (primaryAddress) {
        setAddressState({
          addressLine: primaryAddress.addressLine || '',
          city: primaryAddress.city || '',
          state: primaryAddress.state || '',
          postalCode: primaryAddress.postalCode || '',
          country: primaryAddress.country || '',
        });
      }
    }
  }, [employee]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormState({ ...formState, [e.target.name]: e.target.value });
  };
  
  const handleAddressChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setAddressState({ ...addressState, [e.target.name]: e.target.value });
  };

  const handleSave = () => {
    if (!id) return;
    updateEmployee({
      employeeId: id,
      ...formState,
      departmentId: formState.departmentId || null,
      probationEndDate: formState.probationEndDate || null,
      workingHoursPerWeek: Number(formState.workingHoursPerWeek),
      vacationDaysTotal: Number(formState.vacationDaysTotal),
    });
    if (addressState.addressLine) {
      updateAddress({
        employeeId: id,
        ...addressState,
        isPrimary: true,
      });
    }
  };

  const handleTerminate = () => {
    if (!id) return;
    setFormState(prev => ({ ...prev, employmentStatus: 3 }));
    updateEmployee({
      employeeId: id,
      ...formState,
      employmentStatus: 3,
      departmentId: formState.departmentId || null,
      probationEndDate: formState.probationEndDate || null,
      workingHoursPerWeek: Number(formState.workingHoursPerWeek),
      vacationDaysTotal: Number(formState.vacationDaysTotal),
    });
  };

  const genders = [{ value: 0, label: 'Unspecified' }, { value: 1, label: 'Male' }, { value: 2, label: 'Female' }];
  const statuses = [{ value: 1, label: 'Active' }, { value: 2, label: 'On Leave' }, { value: 3, label: 'Terminated' }];

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

  if (isError || !employee) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '60vh', flexDirection: 'column', gap: 2 }}>
        <Typography variant="h6" color="error">Failed to load employee details.</Typography>
        <Button variant="outlined" onClick={() => navigate('/employees/roster')}>Go Back</Button>
      </Box>
    );
  }

  const getStatusProps = (status: number) => {
    switch(status) {
      case 1:
        return {
          label: 'Active',
          color: (theme: any) => theme.palette.mode === 'dark' ? '#81c784' : '#2e7d32',
          bg: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(46, 125, 50, 0.15)' : 'rgba(46, 125, 50, 0.08)',
          border: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(129, 199, 132, 0.2)' : 'rgba(46, 125, 50, 0.15)'
        };
      case 2:
        return {
          label: 'On Leave',
          color: (theme: any) => theme.palette.mode === 'dark' ? '#ffb74d' : '#ed6c02',
          bg: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(237, 108, 2, 0.15)' : 'rgba(237, 108, 2, 0.08)',
          border: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(255, 183, 77, 0.2)' : 'rgba(237, 108, 2, 0.15)'
        };
      case 3:
        return {
          label: 'Terminated',
          color: (theme: any) => theme.palette.mode === 'dark' ? '#e57373' : '#d32f2f',
          bg: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(211, 47, 47, 0.15)' : 'rgba(211, 47, 47, 0.08)',
          border: (theme: any) => theme.palette.mode === 'dark' ? 'rgba(229, 115, 115, 0.2)' : 'rgba(211, 47, 47, 0.15)'
        };
      default:
        return { label: 'Unknown', color: 'text.secondary', bg: 'transparent', border: 'divider' };
    }
  };

  const statusProps = getStatusProps(employee.employmentStatus);

  return (
    <Box className={styles.pageContainer}>
      
      {/* ── TOP ACTION BAR ────────────────────────────────────────────────── */}
      <Box className={styles.headerContainer}>
        <Stack direction="row" spacing={2} sx={{ alignItems: 'center' }}>
          <Tooltip title="Back to Roster">
            <IconButton onClick={() => navigate('/employees/roster')} sx={{ bgcolor: 'action.hover' }}>
              <ArrowBackRounded />
            </IconButton>
          </Tooltip>
          <Box>
            {/* .pageTitle sınıfındaki tasarım[cite: 9] */}
            <Typography 
              variant="h4" 
              sx={{ 
                fontWeight: 700, 
                letterSpacing: '-0.02em', 
                color: 'text.primary',
                textShadow: (theme) => theme.palette.mode === 'dark' ? '0 1px 2px rgba(0,0,0,0.5)' : '0 1px 2px rgba(0,0,0,0.05)'
              }}
            >
              Employee Profile
            </Typography>
            <Typography variant="body2" color="text.secondary">
              ID: {employee.id}
            </Typography>
          </Box>
        </Stack>

        <Stack direction="row" spacing={1.5}>
          <Button onClick={handleTerminate} disabled={isUpdatingEmployee || isUpdatingAddress || formState.employmentStatus === 3} variant="outlined" color="error" startIcon={<PersonRemoveRounded />} sx={actionButtonSx}>
            {formState.employmentStatus === 3 ? 'Terminated' : 'Terminate'}
          </Button>
          <Button onClick={handleSave} disabled={isUpdatingEmployee || isUpdatingAddress} variant="contained" startIcon={<SaveRounded />} sx={{ borderRadius: '10px', fontWeight: 600, textTransform: 'none', boxShadow: 'none' }}>
            {isUpdatingEmployee || isUpdatingAddress ? 'Saving...' : 'Save Changes'}
          </Button>
        </Stack>
      </Box>

      {/* ── PROFILE SUMMARY CARD ────────────────────────────────────────── */}
      <Box sx={glassPanelSx}>
        <Box className={styles.profileSummaryGrid}>
          <Avatar sx={{ width: 84, height: 84, bgcolor: 'primary.main', fontSize: '2rem', fontWeight: 600 }}>
            {employee.firstName.charAt(0)}{employee.lastName.charAt(0)}
          </Avatar>
          <Box sx={{ flex: 1 }}>
            <Stack direction="row" spacing={2} sx={{ mb: 0.5, alignItems: 'center' }}>
              <Typography variant="h5" sx={{ fontWeight: 700 }}>{employee.firstName} {employee.lastName}</Typography>
              <Chip 
                label={statusProps.label} 
                size="small" 
                sx={{ 
                  fontWeight: 600, 
                  bgcolor: statusProps.bg, 
                  color: statusProps.color,
                  border: '1px solid',
                  borderColor: statusProps.border
                }} 
              />
            </Stack>
            <Typography variant="body1" color="text.secondary" sx={{ fontWeight: 500, mb: 1 }}>
              {employee.profession} • {employee.employeeNo} {employee.department ? `• ${employee.department.name}` : ''}
            </Typography>
            <Stack direction="row" spacing={3} sx={{ mt: 1.5 }}>
              <Typography variant="caption" color="text.secondary">
                <strong>Hire Date:</strong> {new Date(employee.hireDate).toLocaleDateString()}
              </Typography>
              <Typography variant="caption" color="text.secondary">
                <strong>Probation Ends:</strong> {employee.probationEndDate ? new Date(employee.probationEndDate).toLocaleDateString() : 'N/A'}
              </Typography>
              <Typography variant="caption" color="text.secondary">
                <strong>Nationality:</strong> {employee.nationality}
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
          <Tab icon={<AccountBalanceWalletRounded sx={{ mr: 1 }}/>} iconPosition="start" label="Financial & Compensation" />
          <Tab icon={<FolderSharedRounded sx={{ mr: 1 }}/>} iconPosition="start" label="Documents" />
          <Tab icon={<ContactsRounded sx={{ mr: 1 }}/>} iconPosition="start" label="Notes & References" />
        </Tabs>
      </Box>

      {/* ── TAB 1: GENERAL INFO (Employee Entity Form) ────────────────────── */}
      <TabPanel value={activeTab} index={0}>
        <Box sx={glassPanelSx}>
          <Typography variant="h6" className={styles.sectionTitle}>
            Personal Details
          </Typography>
          <Divider sx={{ mb: 3, opacity: 0.5 }} />
          
          <Box className={styles.formGrid}>
            <TextField label="Employee No" value={employee.employeeNo} size="small" fullWidth sx={premiumInputSx} disabled />
            <TextField label="First Name" name="firstName" value={formState.firstName} onChange={handleChange} size="small" fullWidth sx={premiumInputSx} />
            <TextField label="Last Name" name="lastName" value={formState.lastName} onChange={handleChange} size="small" fullWidth sx={premiumInputSx} />
            <TextField select label="Gender" name="gender" value={formState.gender} onChange={handleChange} size="small" fullWidth sx={premiumInputSx}>
              {genders.map((g) => <MenuItem key={g.value} value={g.value}>{g.label}</MenuItem>)}
            </TextField>
            <TextField label="Date of Birth" name="dateOfBirth" type="date" value={formState.dateOfBirth} onChange={handleChange} size="small" fullWidth slotProps={{ inputLabel: { shrink: true } }} sx={premiumInputSx} />
            <TextField label="Nationality" name="nationality" value={formState.nationality} onChange={handleChange} size="small" fullWidth sx={premiumInputSx} />
            <TextField label="Profession" name="profession" value={formState.profession} onChange={handleChange} size="small" fullWidth sx={premiumInputSx} />
            <TextField select label="Employment Status" name="employmentStatus" value={formState.employmentStatus} onChange={handleChange} size="small" fullWidth sx={premiumInputSx}>
              {statuses.map((s) => <MenuItem key={s.value} value={s.value}>{s.label}</MenuItem>)}
            </TextField>
            <TextField label="Hire Date" name="hireDate" type="date" value={formState.hireDate} onChange={handleChange} size="small" fullWidth slotProps={{ inputLabel: { shrink: true } }} sx={premiumInputSx} />
            <TextField label="Probation End Date" name="probationEndDate" type="date" value={formState.probationEndDate} onChange={handleChange} size="small" fullWidth slotProps={{ inputLabel: { shrink: true } }} sx={premiumInputSx} />
            <TextField label="Working Hours / Week" name="workingHoursPerWeek" type="number" value={formState.workingHoursPerWeek} onChange={handleChange} size="small" fullWidth sx={premiumInputSx} />
            <TextField label="Vacation Days Total" name="vacationDaysTotal" type="number" value={formState.vacationDaysTotal} onChange={handleChange} size="small" fullWidth sx={premiumInputSx} />
            <TextField select label="Department" name="departmentId" value={formState.departmentId} onChange={handleChange} size="small" fullWidth sx={premiumInputSx}>
              <MenuItem value=""><em>None</em></MenuItem>
              {departmentOptions.map((d: any) => <MenuItem key={d.value} value={d.value}>{d.label}</MenuItem>)}
            </TextField>
          </Box>
        </Box>

        {/* Action Card for Address Entity */}
        <Box sx={{ mt: 3 }}>
          <Typography variant="h6" className={styles.sectionTitle}>
            Primary Address
          </Typography>
          <Divider sx={{ mb: 3, opacity: 0.5 }} />
          <Box sx={glassPanelSx}>
            <Box className={styles.formGrid}>
              <TextField label="Address Line" name="addressLine" value={addressState.addressLine} onChange={handleAddressChange} size="small" fullWidth sx={{ ...premiumInputSx, gridColumn: '1 / -1' }} />
              <TextField label="City" name="city" value={addressState.city} onChange={handleAddressChange} size="small" fullWidth sx={premiumInputSx} />
              <TextField label="State/Province" name="state" value={addressState.state} onChange={handleAddressChange} size="small" fullWidth sx={premiumInputSx} />
              <TextField label="Postal Code" name="postalCode" value={addressState.postalCode} onChange={handleAddressChange} size="small" fullWidth sx={premiumInputSx} />
              <TextField label="Country" name="country" value={addressState.country} onChange={handleAddressChange} size="small" fullWidth sx={premiumInputSx} />
            </Box>
          </Box>
        </Box>
      </TabPanel>

      {/* ── TAB 2: FINANCIAL & COMPENSATION ───────────────────────────────── */}
      <TabPanel value={activeTab} index={1}>
        <Box className={styles.actionCardsGrid}>
          <Box sx={{ ...glassPanelSx, display: 'flex', flexDirection: 'column', gap: 2 }}>
            <Box>
              <Typography variant="h6" sx={{ fontWeight: 700 }}>Compensation History</Typography>
              <Typography variant="body2" color="text.secondary">Manage pay grades, base salary, and effective dates.</Typography>
            </Box>
            <Divider sx={{ opacity: 0.5 }} />
            <Stack direction="row" sx={{ justifyContent: 'space-between', alignItems: 'center' }}>
              <Box>
                <Typography variant="caption" color="text.secondary" sx={{ fontWeight: 600, textTransform: 'uppercase' }}>Current Base Salary</Typography>
                <Typography variant="h5" sx={{ fontWeight: 700, color: 'success.main' }}>15,29 € <Typography component="span" variant="body2" color="text.secondary">/ Hour</Typography></Typography>
              </Box>
              <Button variant="outlined" endIcon={<OpenInNewRounded />} sx={actionButtonSx}>
                Manage Salaries
              </Button>
            </Stack>
          </Box>
        </Box>
      </TabPanel>

      {/* ── TAB 3: DOCUMENTS ──────────────────────────────────────────────── */}
      <TabPanel value={activeTab} index={2}>
        <Box className={styles.actionCardsGrid}>
          <Box sx={{ ...glassPanelSx, display: 'flex', flexDirection: 'column', gap: 2 }}>
            <Box>
              <Typography variant="h6" sx={{ fontWeight: 700 }}>Employee Documents</Typography>
              <Typography variant="body2" color="text.secondary">View contracts, ID copies, and official records linked to this employee.</Typography>
            </Box>
            <Divider sx={{ opacity: 0.5 }} />
            <Stack direction="row" sx={{ justifyContent: 'space-between', alignItems: 'center' }}>
              <Typography variant="body1" sx={{ fontWeight: 600 }}>4 Active Documents</Typography>
              <Button variant="outlined" endIcon={<OpenInNewRounded />} sx={actionButtonSx}>
                Open Document Vault
              </Button>
            </Stack>
          </Box>
        </Box>
      </TabPanel>

      {/* ── TAB 4: NOTES & REFERENCES ─────────────────────────────────────── */}
      <TabPanel value={activeTab} index={3}>
        <Box className={styles.actionCardsGrid}>
          <Box sx={{ ...glassPanelSx, display: 'flex', flexDirection: 'column', gap: 2 }}>
            <Box>
              <Typography variant="h6" sx={{ fontWeight: 700 }}>References & Notes</Typography>
              <Typography variant="body2" color="text.secondary">Previous employers, contact persons, and internal HR notes.</Typography>
            </Box>
            <Divider sx={{ opacity: 0.5 }} />
            <Stack direction="row" sx={{ justifyContent: 'space-between', alignItems: 'center' }}>
              <Typography variant="body1" sx={{ fontWeight: 600 }}>2 References • 1 Internal Note</Typography>
              <Button variant="outlined" endIcon={<OpenInNewRounded />} sx={actionButtonSx}>
                Manage Records
              </Button>
            </Stack>
          </Box>
        </Box>
      </TabPanel>
    </Box>
  );
};