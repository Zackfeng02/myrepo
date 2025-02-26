import React, { createContext, useState } from 'react';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const storedToken = localStorage.getItem('token');
  const [authData, setAuthData] = useState({ token: storedToken || null });

  const setAuth = (data) => {
    setAuthData(data);
    localStorage.setItem('token', data.token);
  };

  return (
    <AuthContext.Provider value={{ authData, setAuthData: setAuth }}>
      {children}
    </AuthContext.Provider>
  );
};
