import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import Signup from '../components/Auth/Signup';

const SignupPage = () => {
  return (
    <Container className="mt-4">
      <Row className="justify-content-md-center">
        <Col md={6}>
          <h2 className="mb-4">Sign Up</h2>
          <Signup />
        </Col>
      </Row>
    </Container>
  );
};

export default SignupPage;
