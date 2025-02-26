import React, { useState } from 'react';
import { Form, Button, Alert } from 'react-bootstrap';
import { gql, useMutation } from '@apollo/client';

const ADD_COURSE = gql`
  mutation AddCourse($input: CourseInput!) {
    addCourse(input: $input) {
      id
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

  const [addCourse, { loading }] = useMutation(ADD_COURSE, {
    onCompleted: () => {
      setCourseCode('');
      setCourseName('');
      setSection('');
      setSemester('');
    },
    onError: (err) => setError(err.message)
  });

  const handleSubmit = (e) => {
    e.preventDefault();
    addCourse({ variables: { input: { courseCode, courseName, section, semester } } });
  };

  return (
    <Form onSubmit={handleSubmit}>
      {error && <Alert variant="danger">{error}</Alert>}
      <Form.Group controlId="courseCode">
        <Form.Label>Course Code</Form.Label>
        <Form.Control
          type="text"
          value={courseCode}
          onChange={(e) => setCourseCode(e.target.value)}
          required
        />
      </Form.Group>
      <Form.Group controlId="courseName" className="mt-3">
        <Form.Label>Course Name</Form.Label>
        <Form.Control
          type="text"
          value={courseName}
          onChange={(e) => setCourseName(e.target.value)}
          required
        />
      </Form.Group>
      <Form.Group controlId="section" className="mt-3">
        <Form.Label>Section</Form.Label>
        <Form.Control
          type="text"
          value={section}
          onChange={(e) => setSection(e.target.value)}
          required
        />
      </Form.Group>
      <Form.Group controlId="semester" className="mt-3">
        <Form.Label>Semester</Form.Label>
        <Form.Control
          type="text"
          value={semester}
          onChange={(e) => setSemester(e.target.value)}
          required
        />
      </Form.Group>
      <Button variant="primary" type="submit" disabled={loading} className="mt-3">
        Add Course
      </Button>
    </Form>
  );
};

export default AddCourse;
