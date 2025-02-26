import React, { useState } from 'react';
import { Form, Button, Alert } from 'react-bootstrap';
import { gql, useMutation } from '@apollo/client';
import { useNavigate } from 'react-router-dom';

const REGISTER_STUDENT = gql`
  mutation RegisterStudent($input: StudentInput!) {
    registerStudent(input: $input) {
      id
      studentNumber
      firstName
      lastName
      email
    }
  }
`;

const Signup = () => {
  const [studentNumber, setStudentNumber] = useState('');
  const [password, setPassword] = useState('');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [address, setAddress] = useState('');
  const [city, setCity] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [email, setEmail] = useState('');
  const [program, setProgram] = useState('');
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const [registerStudent, { loading }] = useMutation(REGISTER_STUDENT, {
    onCompleted: () => navigate('/login'),
    onError: (err) => setError(err.message)
  });

  const handleSubmit = (e) => {
    e.preventDefault();
    registerStudent({
      variables: {
        input: {
          studentNumber,
          password,
          firstName,
          lastName,
          address,
          city,
          phoneNumber,
          email,
          program
        }
      }
    });
  };

  return (
    <Form onSubmit={handleSubmit}>
      {error && <Alert variant="danger">{error}</Alert>}
      <Form.Group controlId="studentNumber">
        <Form.Label>Student Number</Form.Label>
        <Form.Control
          type="text"
          value={studentNumber}
          onChange={(e) => setStudentNumber(e.target.value)}
          required
        />
      </Form.Group>
      <Form.Group controlId="firstName" className="mt-3">
        <Form.Label>First Name</Form.Label>
        <Form.Control
          type="text"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
          required
        />
      </Form.Group>
      <Form.Group controlId="lastName" className="mt-3">
        <Form.Label>Last Name</Form.Label>
        <Form.Control
          type="text"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
          required
        />
      </Form.Group>
      <Form.Group controlId="email" className="mt-3">
        <Form.Label>Email</Form.Label>
        <Form.Control
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
      </Form.Group>
      <Form.Group controlId="password" className="mt-3">
        <Form.Label>Password</Form.Label>
        <Form.Control
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
      </Form.Group>
      <Form.Group controlId="address" className="mt-3">
        <Form.Label>Address</Form.Label>
        <Form.Control
          type="text"
          value={address}
          onChange={(e) => setAddress(e.target.value)}
          required
        />
      </Form.Group>
      <Form.Group controlId="city" className="mt-3">
        <Form.Label>City</Form.Label>
        <Form.Control
          type="text"
          value={city}
          onChange={(e) => setCity(e.target.value)}
          required
        />
      </Form.Group>
      <Form.Group controlId="phoneNumber" className="mt-3">
        <Form.Label>Phone Number</Form.Label>
        <Form.Control
          type="text"
          value={phoneNumber}
          onChange={(e) => setPhoneNumber(e.target.value)}
          required
        />
      </Form.Group>
      <Form.Group controlId="program" className="mt-3">
        <Form.Label>Program</Form.Label>
        <Form.Control
          type="text"
          value={program}
          onChange={(e) => setProgram(e.target.value)}
          required
        />
      </Form.Group>
      <Button variant="primary" type="submit" disabled={loading} className="mt-3">
        Sign Up
      </Button>
    </Form>
  );
};

export default Signup;
