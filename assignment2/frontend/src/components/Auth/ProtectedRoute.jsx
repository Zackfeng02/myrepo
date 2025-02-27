import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const ProtectedRoute = ({ children }) => {
  const { currentStudent } = useAuth();
  return currentStudent ? children : <Navigate to="/login" replace />;
};

export default ProtectedRoute;