import React from 'react';
import { useQuery, gql } from '@apollo/client';
import { Spinner, Alert } from 'react-bootstrap';
import StudentItem from './StudentItem';

const GET_STUDENTS = gql`
  query GetStudents {
    students {
      id
      studentNumber
      firstName
      lastName
      email
      program
    }
  }
`;

const StudentList = ({ onViewCourses }) => {
  const { loading, error, data } = useQuery(GET_STUDENTS);

  if (loading) return <Spinner animation="border" />;
  if (error) return <Alert variant="danger">{error.message}</Alert>;

  return (
    <div>
      {data.students.map(student => (
        <StudentItem key={student.id} student={student} onViewCourses={onViewCourses} />
      ))}
    </div>
  );
};

export default StudentList;
