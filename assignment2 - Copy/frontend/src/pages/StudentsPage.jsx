import React, { useState } from 'react';
import { Container } from 'react-bootstrap';
import StudentList from '../components/Students/StudentList';
import StudentCoursesModal from '../components/Students/StudentCoursesModal';

const StudentsPage = () => {
  const [selectedStudentId, setSelectedStudentId] = useState(null);
  const [showCourses, setShowCourses] = useState(false);

  const handleViewCourses = (studentId) => {
    setSelectedStudentId(studentId);
    setShowCourses(true);
  };

  const handleCloseCourses = () => {
    setShowCourses(false);
    setSelectedStudentId(null);
  };

  return (
    <Container className="mt-4">
      <h2 className="mb-4">Students</h2>
      <StudentList onViewCourses={handleViewCourses} />
      <StudentCoursesModal
        studentId={selectedStudentId}
        show={showCourses}
        onHide={handleCloseCourses}
      />
    </Container>
  );
};

export default StudentsPage;
