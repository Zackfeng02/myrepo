// Desc: Login component for students
import { useAuth } from '../../context/AuthContext'; 

const Login = () => {
  const { login } = useAuth(); // Destructure from the hook
  
  // Your login logic here
  const handleLogin = async () => {
    try {
      // ... authentication logic
      login(token, studentData);
    } catch (error) {
      // Handle error
    }
  };

  return (
    <Form onSubmit={handleSubmit}>
      {error && <Alert variant="danger">{error}</Alert>}
      <Form.Group controlId="formBasicEmail">
        <Form.Label>Email address</Form.Label>
        <Form.Control
          type="email"
          placeholder="Enter email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
      </Form.Group>
      <Form.Group controlId="formBasicPassword" className="mt-3">
        <Form.Label>Password</Form.Label>
        <Form.Control
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
      </Form.Group>
      <Button variant="primary" type="submit" disabled={loading} className="mt-3">
        Login
      </Button>
    </Form>
  );
};

export default Login;
