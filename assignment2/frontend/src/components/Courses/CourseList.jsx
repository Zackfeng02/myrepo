import React from 'react';
import { useQuery, gql } from '@apollo/client';
import { Spinner, Alert } from 'react-bootstrap';
import CourseItem from './CourseItem';

const GET_COURSES = gql`
  query GetCourses {
    courses {
      id
      courseCode
      courseName
      section
      semester
    }
  }
`;

const CourseList = ({ onEnroll, onEdit }) => {
  const { loading, error, data } = useQuery(GET_COURSES);

  if (loading) return <Spinner animation="border" />;
  if (error) return <Alert variant="danger">{error.message}</Alert>;

  return (
    <div>
      {data.courses.map((course) => (
        <CourseItem key={course.id} course={course} onEnroll={onEnroll} onEdit={onEdit} />
      ))}
    </div>
  );
};

export default CourseList;
