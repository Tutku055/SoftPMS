import { createBrowserRouter, Navigate, Outlet } from 'react-router-dom';
import { useAuthStore } from '../store/useAuthStore';
import { DashboardLayout } from '../layouts/DashboardLayout';
import { Login } from '../features/auth/Login';
import { EmployeesList } from '../features/employees/EmployeesList';
import { RolesList } from '../features/roles/RolesList';
import { UsersList } from '../features/users/UsersList';

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
            element: <Navigate to="/employees" replace />
          },
          {
            path: 'employees',
            element: <EmployeesList />,
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
