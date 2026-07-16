import { Box, Card, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, IconButton, Button, CircularProgress } from '@mui/material';
import { Add, Delete, Edit } from '@mui/icons-material';
import { useGetEmployees, useDeleteEmployee } from './hooks/useEmployees';
import { HasPermission } from '../../components/HasPermission';

export const EmployeesList = () => {
  const { data, isLoading, isError } = useGetEmployees(1, 20);
  const { mutate: deleteEmployee, isPending: isDeleting } = useDeleteEmployee();

  if (isLoading) return <CircularProgress />;
  if (isError) return <Typography color="error">Error loading employees</Typography>;

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3, alignItems: 'center' }}>
        <Typography variant="h4">Employees</Typography>
        <HasPermission requiredPermission="Employees.Create">
          <Button variant="contained" startIcon={<Add />}>
            Add Employee
          </Button>
        </HasPermission>
      </Box>

      <Card>
        <TableContainer>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Name</TableCell>
                <TableCell>Email</TableCell>
                <TableCell>Profession</TableCell>
                <TableCell>City</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {data?.items.map((emp) => (
                <TableRow key={emp.id} hover>
                  <TableCell>{emp.firstName} {emp.lastName}</TableCell>
                  <TableCell>{emp.email}</TableCell>
                  <TableCell>{emp.profession}</TableCell>
                  <TableCell>{emp.city}</TableCell>
                  <TableCell align="right">
                    <HasPermission requiredPermission="Employees.Update">
                      <IconButton color="primary" size="small">
                        <Edit />
                      </IconButton>
                    </HasPermission>
                    <HasPermission requiredPermission="Employees.Delete">
                      <IconButton 
                        color="error" 
                        size="small"
                        onClick={() => deleteEmployee(emp.id)}
                        disabled={isDeleting}
                      >
                        <Delete />
                      </IconButton>
                    </HasPermission>
                  </TableCell>
                </TableRow>
              ))}
              {data?.items.length === 0 && (
                <TableRow>
                  <TableCell colSpan={5} align="center">No employees found.</TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      </Card>
    </Box>
  );
};
