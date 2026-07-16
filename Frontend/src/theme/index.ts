import { createTheme } from '@mui/material/styles';
import type { ThemeOptions } from '@mui/material/styles';

const typography = {
  fontFamily: '"Inter", "Roboto", "Helvetica", "Arial", sans-serif',
  h1: { fontWeight: 700, fontSize: '2.5rem' },
  h2: { fontWeight: 600, fontSize: '2rem' },
  h3: { fontWeight: 600, fontSize: '1.75rem' },
  h4: { fontWeight: 600, fontSize: '1.5rem' },
  h5: { fontWeight: 500, fontSize: '1.25rem' },
  h6: { fontWeight: 500, fontSize: '1rem' },
  button: { textTransform: 'none' as const, fontWeight: 500 },
};

const lightPalette = {
  mode: 'light' as const,
  primary: {
    main: '#4F46E5', // Indigo 600
    light: '#818CF8',
    dark: '#3730A3',
    contrastText: '#ffffff',
  },
  secondary: {
    main: '#10B981', // Emerald 500
    light: '#34D399',
    dark: '#059669',
    contrastText: '#ffffff',
  },
  background: {
    default: '#F3F4F6', // Gray 100
    paper: '#ffffff',
  },
  text: {
    primary: '#111827', // Gray 900
    secondary: '#4B5563', // Gray 600
  },
  divider: '#E5E7EB',
};

const darkPalette = {
  mode: 'dark' as const,
  primary: {
    main: '#6366F1', // Indigo 500
    light: '#818CF8',
    dark: '#4338CA',
    contrastText: '#ffffff',
  },
  secondary: {
    main: '#34D399', // Emerald 400
    light: '#6EE7B7',
    dark: '#10B981',
    contrastText: '#111827',
  },
  background: {
    default: '#111827', // Gray 900
    paper: '#1F2937', // Gray 800
  },
  text: {
    primary: '#F9FAFB', // Gray 50
    secondary: '#9CA3AF', // Gray 400
  },
  divider: '#374151',
};

const components = {
  MuiButton: {
    styleOverrides: {
      root: {
        borderRadius: 8,
        boxShadow: 'none',
        '&:hover': {
          boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1)',
        },
      },
    },
  },
  MuiCard: {
    styleOverrides: {
      root: {
        borderRadius: 12,
        boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1)',
        backgroundImage: 'none', // Remove default MUI dark mode overlay
      },
    },
  },
  MuiPaper: {
    styleOverrides: {
      root: {
        borderRadius: 12,
      },
    },
  },
};

export const getTheme = (mode: 'light' | 'dark') => {
  const palette = mode === 'light' ? lightPalette : darkPalette;

  return createTheme({
    palette,
    typography,
    components,
    shape: {
      borderRadius: 8,
    },
  } as ThemeOptions);
};
