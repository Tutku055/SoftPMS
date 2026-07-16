import { Box, Card, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, IconButton, Button, CircularProgress, Chip } from '@mui/material';
import { Add, Delete, Edit, Badge } from '@mui/icons-material';
import { useGetUsers, useDeleteUser } from './hooks/useUsers';
import { HasPermission } from '../../components/HasPermission';

export const UsersList = () => {
  const { data, isLoading, isError } = useGetUsers();
  const { mutate: deleteUser, isPending: isDeleting } = useDeleteUser();

  if (isLoading) return <CircularProgress />;
  if (isError) return <Typography color="error">Error loading users</Typography>;

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3, alignItems: 'center' }}>
        <Typography variant="h4">Users</Typography>
        <HasPermission requiredPermission="Users.Create">
          <Button variant="contained" startIcon={<Add />}>
            Add User
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
                <TableCell>Status</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {data?.map((user) => (
                <TableRow key={user.id} hover>
                  <TableCell>{user.firstName} {user.lastName}</TableCell>
                  <TableCell>{user.email}</TableCell>
                  <TableCell>
                    <Chip 
                      label={user.isActive ? 'Active' : 'Inactive'} 
                      color={user.isActive ? 'success' : 'default'} 
                      size="small" 
                    />
                  </TableCell>
                  <TableCell align="right">
                    <HasPermission requiredPermission="Users.Update">
                      <IconButton color="secondary" size="small" title="Manage Roles">
                        <Badge />
                      </IconButton>
                      <IconButton color="primary" size="small">
                        <Edit />
                      </IconButton>
                    </HasPermission>
                    <HasPermission requiredPermission="Users.Delete">
                      <IconButton 
                        color="error" 
                        size="small"
                        onClick={() => deleteUser(user.id)}
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
                  <TableCell colSpan={4} align="center">No users found.</TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      </Card>
    </Box>
  );
};
