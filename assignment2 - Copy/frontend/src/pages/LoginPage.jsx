import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import Login from '../components/Auth/Login';

const LoginPage = () => {
  return (
    <Container className="mt-4">
      <Row className="justify-content-md-center">
        <Col md={6}>
          <h2 className="mb-4">Login</h2>
          <Login />
        </Col>
      </Row>
    </Container>
  );
};

export default LoginPage;
