import { Box, Card, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, IconButton, Button, CircularProgress } from '@mui/material';
import { Add, Delete, Edit, Security } from '@mui/icons-material';
import { useGetRoles, useDeleteRole } from './hooks/useRoles';
import { HasPermission } from '../../components/HasPermission';

export const RolesList = () => {
  const { data, isLoading, isError } = useGetRoles();
  const { mutate: deleteRole, isPending: isDeleting } = useDeleteRole();

  if (isLoading) return <CircularProgress />;
  if (isError) return <Typography color="error">Error loading roles</Typography>;

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3, alignItems: 'center' }}>
        <Typography variant="h4">Roles</Typography>
        <HasPermission requiredPermission="Roles.Create">
          <Button variant="contained" startIcon={<Add />}>
            Add Role
          </Button>
        </HasPermission>
      </Box>

      <Card>
        <TableContainer>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Name</TableCell>
                <TableCell>Description</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {data?.map((role) => (
                <TableRow key={role.id} hover>
                  <TableCell>{role.name}</TableCell>
                  <TableCell>{role.description}</TableCell>
                  <TableCell align="right">
                    <HasPermission requiredPermission="Roles.Update">
                      <IconButton color="primary" size="small" title="Permissions">
                        <Security />
                      </IconButton>
                      <IconButton color="primary" size="small">
                        <Edit />
                      </IconButton>
                    </HasPermission>
                    <HasPermission requiredPermission="Roles.Delete">
                      <IconButton 
                        color="error" 
                        size="small"
                        onClick={() => deleteRole(role.id)}
                        disabled={isDeleting}
                      >
                        <Delete />
                      </IconButton>
                    </HasPermission>
                  </TableCell>
                </TableRow>
              ))}
              {(!data || data.length === 0) && (
                <TableRow>
                  <TableCell colSpan={3} align="center">No roles found.</TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      </Card>
    </Box>
  );
};
