// src/features/auth/components/Login/Login.tsx
import { useState } from 'react';
import { 
  TextField, 
  Button, 
  CircularProgress, 
  Alert, 
  useTheme 
} from '@mui/material';
import { CheckCircleOutlined } from '@mui/icons-material';
import { useLogin } from '../../hooks/useLogin';
import styles from './Login.module.css';
import darkBg from '../../../../assets/images/DarkThemeLoginBackground.png';
import lightBg from '../../../../assets/images/LightThemeLoginBackground.png';
import logoImg from '../../../../assets/images/SoftPMSLogo.png';

export const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const { mutate, isPending, isError, error } = useLogin();
  
  const theme = useTheme();
  const isDark = theme.palette.mode === 'dark';
  const wrapperClass = isDark ? styles.wrapperDark : styles.wrapperLight;

  const getErrorMessage = () => {
    if (!error) return 'Invalid credentials. Please try again.';
    const anyErr = error as any;
    const responseData = anyErr.response?.data;
    const serverMsg =
      responseData?.errors?.message ||
      (typeof responseData?.errors === 'string' ? responseData.errors : null) ||
      responseData?.detail ||
      responseData?.message;

    if (typeof serverMsg === 'string' && serverMsg.trim()) {
      return serverMsg;
    }
    return 'Invalid credentials. Please try again.';
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (username && password) {
      mutate({ username, password });
    }
  };

  const bgImage = isDark ? darkBg : lightBg;

  return (
    <div 
      className={`${styles.wrapper} ${wrapperClass}`}
      style={{ 
        backgroundImage: `url(${bgImage})`,
        backgroundSize: 'cover',
        backgroundPosition: 'center',
        backgroundRepeat: 'no-repeat'
      }}
    >
      <div className={styles.authCard}>
        
        <div className={styles.formSection}>
          <div className={styles.brand}>
            <img src={logoImg} alt="SoftPMS Logo" className={styles.brandLogo} />
            <span style={{ color: isDark ? '#fafafa' : '#09090b', fontSize: '1.5rem', fontWeight: 800 }}>SoftPMS</span>
          </div>

          <div className={styles.formWrapper}>
            <h1 className={styles.headerText}>Sign in</h1>
            <p className={styles.subHeaderText}>Enter your credentials to continue.</p>

            {isError && (
              <Alert 
                severity="error" 
                sx={{ 
                  width: '100%', 
                  mb: 3, 
                  borderRadius: '8px',
                  border: `1px solid ${isDark ? '#7f1d1d' : '#fecaca'}`,
                  backgroundColor: isDark ? '#450a0a' : '#fef2f2',
                  color: isDark ? '#fca5a5' : '#991b1b',
                  '& .MuiAlert-icon': {
                    color: isDark ? '#f87171' : '#dc2626'
                  }
                }}
              >
                {getErrorMessage()}
              </Alert>
            )}

            <form onSubmit={handleSubmit} style={{ width: '100%' }}>
              <TextField
                fullWidth
                placeholder="Username"
                type="text"
                variant="outlined"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                autoComplete="username"
                required
                sx={{
                  mb: 2,
                  '& .MuiOutlinedInput-root': {
                    borderRadius: '8px',
                    backgroundColor: isDark ? '#18181b' : '#ffffff',
                    '& fieldset': {
                      borderColor: isDark ? '#27272a' : '#e4e4e7',
                      transition: 'border-color 0.2s ease',
                    },
                    '&:hover fieldset': {
                      borderColor: isDark ? '#3f3f46' : '#d4d4d8',
                    },
                    '&.Mui-focused fieldset': {
                      borderColor: isDark ? '#fafafa' : '#18181b',
                      borderWidth: '1px',
                    },
                    '& input': {
                      color: isDark ? '#fafafa' : '#09090b',
                      padding: '12px 14px',
                    }
                  }
                }}
              />
              <TextField
                fullWidth
                placeholder="Password"
                type="password"
                variant="outlined"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                autoComplete="current-password"
                required
                sx={{ 
                  mb: 4,
                  '& .MuiOutlinedInput-root': {
                    borderRadius: '8px',
                    backgroundColor: isDark ? '#18181b' : '#ffffff',
                    '& fieldset': {
                      borderColor: isDark ? '#27272a' : '#e4e4e7',
                      transition: 'border-color 0.2s ease',
                    },
                    '&:hover fieldset': {
                      borderColor: isDark ? '#3f3f46' : '#d4d4d8',
                    },
                    '&.Mui-focused fieldset': {
                      borderColor: isDark ? '#fafafa' : '#18181b',
                      borderWidth: '1px',
                    },
                    '& input': {
                      color: isDark ? '#fafafa' : '#09090b',
                      padding: '12px 14px',
                    }
                  }
                }}
              />
              
              <Button
                fullWidth
                variant="contained"
                type="submit"
                disabled={isPending}
                disableElevation
                sx={{ 
                  py: 1.2, 
                  borderRadius: '8px', 
                  fontWeight: 500,
                  textTransform: 'none',
                  fontSize: '0.95rem',
                  backgroundColor: isDark ? '#fafafa' : '#18181b',
                  color: isDark ? '#09090b' : '#ffffff',
                  transition: 'all 0.2s ease',
                  '&:hover': {
                    backgroundColor: isDark ? '#e4e4e7' : '#27272a',
                  },
                  '&.Mui-disabled': {
                    backgroundColor: isDark ? '#27272a' : '#e4e4e7',
                    color: isDark ? '#52525b' : '#a1a1aa',
                  }
                }}
              >
                {isPending ? <CircularProgress size={24} color="inherit" /> : 'Sign in'}
              </Button>
            </form>
          </div>
          
          {/* Spacer for flex layout */}
          <div style={{ marginTop: 'auto' }}></div>
        </div>

        <div className={styles.visualSection}>
          <div className={styles.visualContent}>
            <div className={styles.textContent}>
              <h2 className={styles.premiumTitle}>Elevate Your Workforce</h2>
              <ul className={styles.featureList}>
                <li className={styles.featureItem}>
                  <CheckCircleOutlined className={styles.featureIcon} /> Next-Gen Analytics
                </li>
                <li className={styles.featureItem}>
                  <CheckCircleOutlined className={styles.featureIcon} /> Seamless Onboarding
                </li>
                <li className={styles.featureItem}>
                  <CheckCircleOutlined className={styles.featureIcon} /> Intelligent Tracking
                </li>
              </ul>
            </div>
          </div>
        </div>

      </div>
    </div>
  );
};