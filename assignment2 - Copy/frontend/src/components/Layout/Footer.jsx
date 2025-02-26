import React from 'react';
import { Container } from 'react-bootstrap';

const Footer = () => {
  return (
    <footer className="bg-light py-3 mt-4">
      <Container>
        <p className="text-center mb-0">
          &copy; {new Date().getFullYear()} Student Course System. All rights reserved.
        </p>
      </Container>
    </footer>
  );
};

export default Footer;
