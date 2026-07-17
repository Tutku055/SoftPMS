import { createBrowserRouter, Navigate, Outlet } from 'react-router-dom';
import { useAuthStore } from '../store/useAuthStore';
import { DashboardLayout } from '../layouts/DashboardLayout';
import { Login } from '../features/auth/components/Login/Login';

import { ActiveRoster } from '../features/employees/ActiveRoster';

const RolesList = () => <div>RolesList</div>;
const UsersList = () => <div>UsersList</div>;

const PrivateRoute = () => {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
};

export const router = createBrowserRouter([
  {
    path: '/login',
    element: <Login />,
  },
  {
    element: <PrivateRoute />,
    children: [
      {
        path: '/',
        element: <DashboardLayout />,
        children: [
          {
            index: true,
            element: <Navigate to="/dashboard" replace />
          },
          {
            path: 'dashboard',
            element: <div>Dashboard</div>
          },
          {
            path: 'employees/active',
            element: <ActiveRoster />,
          },
          {
            path: 'roles',
            element: <RolesList />,
          },
          {
            path: 'users',
            element: <UsersList />,
          }
        ]
      }
    ]
  },
  {
    path: '*',
    element: <Navigate to="/" replace />
  }
]);
