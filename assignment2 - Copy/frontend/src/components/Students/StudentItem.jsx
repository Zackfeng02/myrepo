import React from 'react';
import { Card, Button } from 'react-bootstrap';

const StudentItem = ({ student, onViewCourses }) => {
  return (
    <Card className="mb-3">
      <Card.Body>
        <Card.Title>
          {student.firstName} {student.lastName}
        </Card.Title>
        <Card.Subtitle className="mb-2 text-muted">
          Student Number: {student.studentNumber}
        </Card.Subtitle>
        <Card.Text>
          Email: {student.email} <br />
          Program: {student.program}
        </Card.Text>
        <Button variant="primary" onClick={() => onViewCourses(student.id)}>
          View Courses
        </Button>
      </Card.Body>
    </Card>
  );
};

export default StudentItem;
