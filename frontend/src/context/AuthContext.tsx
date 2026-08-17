import { createContext, useContext, useEffect, useMemo, useState, type ReactNode } from 'react';
import { loginUser } from '../lib/api';
import type { UserDto } from '../types';

interface AuthContextValue {
  accessToken: string | null;
  user: UserDto | null;
  isAuthenticated: boolean;
  login: (username: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

const STORAGE_KEY = 'sentinelops-auth';

export function AuthProvider({ children }: { children: ReactNode }) {
  const [accessToken, setAccessToken] = useState<string | null>(() => {
    const saved = localStorage.getItem(STORAGE_KEY);
    if (!saved) return null;

    try {
      const parsed = JSON.parse(saved) as { accessToken?: string };
      return parsed.accessToken ?? null;
    } catch {
      return null;
    }
  });

  const [user, setUser] = useState<UserDto | null>(() => {
    const saved = localStorage.getItem(STORAGE_KEY);
    if (!saved) return null;

    try {
      const parsed = JSON.parse(saved) as { user?: UserDto };
      return parsed.user ?? null;
    } catch {
      return null;
    }
  });

  useEffect(() => {
    if (!accessToken || !user) {
      localStorage.removeItem(STORAGE_KEY);
      return;
    }

    localStorage.setItem(STORAGE_KEY, JSON.stringify({ accessToken, user }));
  }, [accessToken, user]);

  const login = async (username: string, password: string) => {
    const response = await loginUser(username, password);
    setAccessToken(response.accessToken);
    setUser(response.user);
  };

  const logout = () => {
    setAccessToken(null);
    setUser(null);
    localStorage.removeItem(STORAGE_KEY);
  };

  const value = useMemo<AuthContextValue>(
    () => ({
      accessToken,
      user,
      isAuthenticated: Boolean(accessToken && user),
      login,
      logout,
    }),
    [accessToken, user],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider.');
  }

  return context;
}
