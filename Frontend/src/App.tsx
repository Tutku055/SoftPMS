import React from "react";
import { ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import { lightTheme, darkTheme } from "./theme";
import { useThemeStore } from "./store/useThemeStore";
import { LoginPage } from "./features/auth/pages/LoginPage";

function App() {
  const isDarkMode = useThemeStore((state) => state.isDarkMode);

  return (
    <ThemeProvider theme={isDarkMode ? darkTheme : lightTheme}>
      {/* Resets browser defaults and applies theme background */}
      <CssBaseline />

      {/* Render Login page temporarily for testing */}
      <LoginPage />
    </ThemeProvider>
  );
}

export default App;
