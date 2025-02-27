import React, { useState } from 'react';
import { useMutation, gql } from '@apollo/client';
import { Form, Button, Alert } from 'react-bootstrap';

const ADD_COURSE_MUTATION = gql`
  mutation AddCourse($courseInput: CourseInput!) {
    addCourse(courseInput: $courseInput) {
      _id
      courseCode
      courseName
      section
      semester
    }
  }
`;

const AddCourse = () => {
  const [courseCode, setCourseCode] = useState('');
  const [courseName, setCourseName] = useState('');
  const [section, setSection] = useState('');
  const [semester, setSemester] = useState('');
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState('');

  const [addCourse, { loading }] = useMutation(ADD_COURSE_MUTATION, {
    onError: (err) => {
      setError(err.message);
      setSuccess('');
    },
    onCompleted: (data) => {
      // Clear form fields after successful addition
      setCourseCode('');
      setCourseName('');
      setSection('');
      setSemester('');
      setSuccess('Course added successfully!');
    },
  });

  const handleSubmit = (e) => {
    e.preventDefault();
    setError(null);
    setSuccess('');

    // Retrieve the JWT token from localStorage
    const token = localStorage.getItem('token');
    console.log("Token:", token); // Debug: Verify token is present

    if (!token) {
      setError('Authentication token missing. Please log in again.');
      return;
    }

    addCourse({
      variables: {
        courseInput: {
          courseCode,
          courseName,
          section,
          semester,
        },
      },
      context: {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      },
    });
  };

  return (
    <Form onSubmit={handleSubmit}>
      {error && <Alert variant="danger">{error}</Alert>}
      {success && <Alert variant="success">{success}</Alert>}

      <Form.Group controlId="courseCode">
        <Form.Label>Course Code</Form.Label>
        <Form.Control
          type="text"
          placeholder="Enter course code"
          value={courseCode}
          onChange={(e) => setCourseCode(e.target.value)}
          required
        />
      </Form.Group>

      <Form.Group controlId="courseName" className="mt-3">
        <Form.Label>Course Name</Form.Label>
        <Form.Control
          type="text"
          placeholder="Enter course name"
          value={courseName}
          onChange={(e) => setCourseName(e.target.value)}
          required
        />
      </Form.Group>

      <Form.Group controlId="section" className="mt-3">
        <Form.Label>Section</Form.Label>
        <Form.Control
          type="text"
          placeholder="Enter section"
          value={section}
          onChange={(e) => setSection(e.target.value)}
          required
        />
      </Form.Group>

      <Form.Group controlId="semester" className="mt-3">
        <Form.Label>Semester</Form.Label>
        <Form.Control
          type="text"
          placeholder="Enter semester"
          value={semester}
          onChange={(e) => setSemester(e.target.value)}
          required
        />
      </Form.Group>

      <Button variant="primary" type="submit" disabled={loading} className="mt-3">
        {loading ? 'Adding...' : 'Add Course'}
      </Button>
    </Form>
  );
};

export default AddCourse;
