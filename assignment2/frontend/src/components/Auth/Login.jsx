// Desc: Login component for students
import React, { useState, useContext } from 'react';
import { Form, Button, Alert } from 'react-bootstrap';
import { useMutation, gql } from '@apollo/client';
import { AuthContext } from '../../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const LOGIN_STUDENT = gql`
  mutation LoginStudent($email: String!, $password: String!) {
    loginStudent(email: $email, password: $password)
  }
`;

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(null);
  const { setAuthData } = useContext(AuthContext);
  const navigate = useNavigate();

  const [loginStudent, { loading }] = useMutation(LOGIN_STUDENT, {
    onCompleted: (data) => {
      setAuthData({ token: data.loginStudent });
      navigate('/');
    },
    onError: (err) => {
      setError(err.message);
    }
  });

  const handleSubmit = (e) => {
    e.preventDefault();
    loginStudent({ variables: { email, password } });
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
