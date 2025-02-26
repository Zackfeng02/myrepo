import React, { useState } from 'react';
import { Container, Row, Col, Modal } from 'react-bootstrap';
import AddCourse from '../components/Courses/AddCourse';
import CourseList from '../components/Courses/CourseList';
import UpdateCourse from '../components/Courses/UpdateCourse';

const CoursesPage = () => {
  const [editingCourse, setEditingCourse] = useState(null);

  const handleEdit = (course) => {
    setEditingCourse(course);
  };

  const handleUpdated = (updatedCourse) => {
    // Optionally refresh course list here
    setEditingCourse(null);
  };

  const handleCancelEdit = () => {
    setEditingCourse(null);
  };

  const handleEnroll = (courseId) => {
    // Implement enrollment logic here
    console.log('Enroll in course:', courseId);
  };

  return (
    <Container>
      <Row>
        <Col>
          <h2 className="mb-4">Courses</h2>
          <AddCourse />
          <CourseList onEnroll={handleEnroll} onEdit={handleEdit} />
        </Col>
      </Row>
      <Modal show={!!editingCourse} onHide={handleCancelEdit}>
        <Modal.Header closeButton>
          <Modal.Title>Edit Course</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {editingCourse && (
            <UpdateCourse
              course={editingCourse}
              onUpdated={handleUpdated}
              onCancel={handleCancelEdit}
            />
          )}
        </Modal.Body>
      </Modal>
    </Container>
  );
};

export default CoursesPage;
