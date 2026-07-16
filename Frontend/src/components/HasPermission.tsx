import type { ReactNode } from 'react';
import React from 'react';
import { useAuthStore } from '../store/useAuthStore';

interface HasPermissionProps {
  requiredPermission: string;
  children: ReactNode;
}

export const HasPermission: React.FC<HasPermissionProps> = ({ requiredPermission, children }) => {
  const hasPermission = useAuthStore((state) => state.hasPermission);
  
  if (!hasPermission(requiredPermission)) {
    return null; // Do not render if the user lacks the required permission
  }
  
  return <>{children}</>;
};
