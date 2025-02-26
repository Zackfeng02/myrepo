import React, { useState } from 'react';
import { Form, Button, Alert } from 'react-bootstrap';
import { gql, useMutation } from '@apollo/client';

const UPDATE_COURSE = gql`
  mutation UpdateCourse($id: ID!, $input: CourseInput!) {
    updateCourse(id: $id, input: $input) {
      id
      courseCode
      courseName
      section
      semester
    }
  }
`;

const UpdateCourse = ({ course, onUpdated, onCancel }) => {
  const [courseCode, setCourseCode] = useState(course.courseCode);
  const [courseName, setCourseName] = useState(course.courseName);
  const [section, setSection] = useState(course.section);
  const [semester, setSemester] = useState(course.semester);
  const [error, setError] = useState(null);

  const [updateCourse, { loading }] = useMutation(UPDATE_COURSE, {
    onCompleted: (data) => {
      onUpdated(data.updateCourse);
    },
    onError: (err) => setError(err.message)
  });

  const handleSubmit = (e) => {
    e.preventDefault();
    updateCourse({
      variables: {
        id: course.id,
        input: { courseCode, courseName, section, semester }
      }
    });
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
        Update Course
      </Button>
      <Button variant="secondary" onClick={onCancel} className="mt-3 ms-2">
        Cancel
      </Button>
    </Form>
  );
};

export default UpdateCourse;
