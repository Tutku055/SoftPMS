export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  username: string;
  email: string;
}

export interface LoginCredentials {
  username?: string;
  password?: string;
}
