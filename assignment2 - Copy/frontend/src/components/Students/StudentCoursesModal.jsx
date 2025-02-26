import React from 'react';
import { useQuery, gql } from '@apollo/client';
import { Modal, Button, ListGroup, Spinner, Alert } from 'react-bootstrap';

const GET_STUDENT_COURSES = gql`
  query GetStudentCourses($id: ID!) {
    student(id: $id) {
      id
      firstName
      lastName
      courses {
        id
        courseCode
        courseName
        section
        semester
      }
    }
  }
`;

const StudentCoursesModal = ({ studentId, show, onHide }) => {
  const { loading, error, data } = useQuery(GET_STUDENT_COURSES, {
    variables: { id: studentId },
    skip: !studentId
  });

  return (
    <Modal show={show} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>
          {data && data.student
            ? `${data.student.firstName} ${data.student.lastName}'s Courses`
            : 'Student Courses'}
        </Modal.Title>
      </Modal.Header>
      <Modal.Body>
        {loading && <Spinner animation="border" />}
        {error && <Alert variant="danger">{error.message}</Alert>}
        {data && data.student && data.student.courses && data.student.courses.length > 0 ? (
          <ListGroup>
            {data.student.courses.map((course) => (
              <ListGroup.Item key={course.id}>
                <strong>{course.courseName}</strong> ({course.courseCode})<br />
                Section: {course.section} - Semester: {course.semester}
              </ListGroup.Item>
            ))}
          </ListGroup>
        ) : (
          !loading && <p>No courses enrolled.</p>
        )}
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>
          Close
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default StudentCoursesModal;
