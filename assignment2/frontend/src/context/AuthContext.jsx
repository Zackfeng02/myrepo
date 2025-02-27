import { createContext, useContext, useState, useEffect } from 'react';

// 1. Create the context (not exported)
const AuthContext = createContext();

// 2. Create provider component
export const AuthProvider = ({ children }) => {
  const [currentStudent, setCurrentStudent] = useState(null);

  // Add your auth logic here
  const login = (token, studentData) => {
    localStorage.setItem('token', token);
    setCurrentStudent(studentData);
  };

  const logout = () => {
    localStorage.removeItem('token');
    setCurrentStudent(null);
  };

  return (
    <AuthContext.Provider value={{ currentStudent, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

// 3. Create custom hook (this is what you should import)
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};