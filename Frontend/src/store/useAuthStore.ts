import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { jwtDecode } from 'jwt-decode';

interface DecodedToken {
  sub?: string;
  email?: string;
  permissions?: string | string[];
  exp?: number;
  [key: string]: any;
}

interface AuthState {
  accessToken: string | null;
  refreshToken: string | null;
  permissions: string[];
  isAuthenticated: boolean;
  login: (accessToken: string, refreshToken: string) => void;
  logout: () => void;
  hasPermission: (permission: string) => boolean;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      accessToken: null,
      refreshToken: null,
      permissions: [],
      isAuthenticated: false,

      login: (accessToken: string, refreshToken: string) => {
        try {
          const decoded = jwtDecode<DecodedToken>(accessToken);
          
          let parsedPermissions: string[] = [];
          if (decoded.permissions) {
            parsedPermissions = Array.isArray(decoded.permissions) 
              ? decoded.permissions 
              : [decoded.permissions];
          }

          set({
            accessToken,
            refreshToken,
            permissions: parsedPermissions,
            isAuthenticated: true,
          });
        } catch (error) {
          console.error('Invalid token during login', error);
          set({ accessToken: null, refreshToken: null, permissions: [], isAuthenticated: false });
        }
      },

      logout: () => {
        set({ accessToken: null, refreshToken: null, permissions: [], isAuthenticated: false });
      },

      hasPermission: (permission: string) => {
        return get().permissions.includes(permission);
      },
    }),
    {
      name: 'auth-storage',
    }
  )
);
