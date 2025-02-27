import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { Button, Alert } from 'react-bootstrap';

const HomePage = () => {
  const { currentStudent } = useAuth();

  return (
    <div className="text-center mt-5">
      <h1>Welcome to the Student Course System</h1>
      {!currentStudent && (
        <Alert variant="info" className="mt-4">
          Please <Link to="/login">login</Link> or <Link to="/signup">sign up</Link> to continue
        </Alert>
      )}
      {currentStudent && (
        <div className="mt-4">
          <Button as={Link} to="/dashboard" variant="primary" size="lg">
            Go to Dashboard
          </Button>
        </div>
      )}
    </div>
  );
};

export default HomePage;