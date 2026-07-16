import { Drawer, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Toolbar, Box, Typography } from '@mui/material';
import { People, VerifiedUser, AdminPanelSettings } from '@mui/icons-material';
import { useLocation, useNavigate } from 'react-router-dom';

const drawerWidth = 240;

export const Sidebar = () => {
  const location = useLocation();
  const navigate = useNavigate();

  const menuItems = [
    { title: 'Employees', path: '/employees', icon: <People /> },
    { title: 'Roles', path: '/roles', icon: <VerifiedUser /> },
    { title: 'Users', path: '/users', icon: <AdminPanelSettings /> },
  ];

  return (
    <Box component="nav" sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 } }}>
      <Drawer
        variant="permanent"
        sx={{
          display: { xs: 'none', sm: 'block' },
          '& .MuiDrawer-paper': { 
            boxSizing: 'border-box', 
            width: drawerWidth,
            backgroundColor: (theme) => theme.palette.background.paper,
            borderRight: (theme) => `1px solid ${theme.palette.divider}`
          },
        }}
        open
      >
        <Toolbar>
          <Typography variant="h5" color="primary" sx={{ fontWeight: 700, letterSpacing: '-0.5px' }}>
            Softlare
          </Typography>
        </Toolbar>
        <List sx={{ px: 2 }}>
          {menuItems.map((item) => (
            <ListItem key={item.title} disablePadding sx={{ mb: 1 }}>
              <ListItemButton
                selected={location.pathname.startsWith(item.path)}
                onClick={() => navigate(item.path)}
                sx={{
                  borderRadius: 2,
                  '&.Mui-selected': {
                    backgroundColor: (theme) => theme.palette.mode === 'light' 
                      ? theme.palette.primary.light + '20' 
                      : theme.palette.primary.dark + '40',
                    color: (theme) => theme.palette.primary.main,
                    '& .MuiListItemIcon-root': {
                      color: (theme) => theme.palette.primary.main,
                    }
                  }
                }}
              >
                <ListItemIcon sx={{ minWidth: 40, color: 'text.secondary' }}>
                  {item.icon}
                </ListItemIcon>
                <ListItemText 
                  primary={item.title} 
                  sx={{ '& .MuiListItemText-primary': { fontWeight: 500 } }}
                />
              </ListItemButton>
            </ListItem>
          ))}
        </List>
      </Drawer>
    </Box>
  );
};
