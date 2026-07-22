export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  username: string;
  email: string;
  requiresPasswordChange?: boolean;
}

export interface LoginCredentials {
  username?: string;
  password?: string;
}
