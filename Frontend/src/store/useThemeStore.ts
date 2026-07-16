import { create } from "zustand";

interface ThemeState {
  isDarkMode: boolean;
  toggleTheme: () => void;
}

export const useThemeStore = create<ThemeState>((set) => {
  // Retrieve the saved theme from localStorage
  const savedTheme = localStorage.getItem("app-theme");
  const initialDark = savedTheme === "dark";

  return {
    isDarkMode: initialDark,
    toggleTheme: () =>
      set((state) => {
        const newMode = !state.isDarkMode;
        // Save the new theme to localStorage
        localStorage.setItem("app-theme", newMode ? "dark" : "light");
        return { isDarkMode: newMode };
      }),
  };
});
