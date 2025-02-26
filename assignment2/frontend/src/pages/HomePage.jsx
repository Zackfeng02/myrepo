import React from 'react';
import { Container } from 'react-bootstrap';

const HomePage = () => {
  return (
    <Container className="mt-4">
      <div className="p-5 mb-4 bg-light rounded-3">
        <div className="container-fluid py-5">
          <h1 className="display-5 fw-bold">Welcome to the Student Course System</h1>
          <p className="col-md-8 fs-4">
            Manage your courses and view student details seamlessly.
          </p>
        </div>
      </div>
    </Container>
  );
};

export default HomePage;
