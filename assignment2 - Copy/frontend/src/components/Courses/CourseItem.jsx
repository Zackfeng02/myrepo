import React from 'react';
import { Card, Button } from 'react-bootstrap';

const CourseItem = ({ course, onEnroll, onEdit }) => {
  return (
    <Card className="mb-3">
      <Card.Body>
        <Card.Title>{course.courseName} ({course.courseCode})</Card.Title>
        <Card.Text>
          Section: {course.section} <br />
          Semester: {course.semester}
        </Card.Text>
        <Button variant="primary" onClick={() => onEnroll(course.id)} className="me-2">
          Enroll
        </Button>
        <Button variant="secondary" onClick={() => onEdit(course)}>
          Edit
        </Button>
      </Card.Body>
    </Card>
  );
};

export default CourseItem;
