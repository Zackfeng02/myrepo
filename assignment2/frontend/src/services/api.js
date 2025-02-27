import { gql } from '@apollo/client';

export const SIGNUP_STUDENT = gql`
  mutation SignupStudent($studentInput: StudentInput!) {
    signup(studentInput: $studentInput) {
      token
      student {
        _id
        studentNumber
        firstName
        lastName
        email
        program
      }
    }
  }
`;

export const LOGIN_STUDENT = gql`
  mutation Login($email: String!, $password: String!) {
    login(email: $email, password: $password) {
      token
      student {
        _id
        firstName
        lastName
        email
        courses {
          _id
          courseCode
        }
      }
    }
  }
`;

export const GET_COURSES_WITH_STUDENTS = gql`
  query GetCoursesWithStudents {
    courses {
      _id
      courseCode
      courseName
      section
      semester
      students {
        _id
        firstName
        lastName
      }
    }
  }
`;

// Enrollment/Drop Mutations
export const ENROLL_COURSE = gql`
  mutation EnrollCourse($courseId: ID!) {
    enrollCourse(courseId: $courseId) {
      _id
      courses {
        _id
        courseCode
      }
    }
  }
`;

export const DROP_COURSE = gql`
  mutation DropCourse($courseId: ID!) {
    dropCourse(courseId: $courseId) {
      _id
      courses {
        _id
        courseCode
      }
    }
  }
`;

// Query all courses
export const GET_COURSES = gql`
  query {
    courses {
      _id
      courseCode
      courseName
      section
      semester
      students {
        _id
      }
    }
  }
`;

// Get all students query
export const GET_ALL_STUDENTS = gql`
  query GetAllStudents {
    students {
      _id
      studentNumber
      firstName
      lastName
      email
      program
      courses {
        _id
        courseCode
      }
    }
  }
`;

// Get student courses query
export const GET_STUDENT_COURSES = gql`
  query GetStudentCourses {
    student {
      _id
      courses {
        _id
        courseCode
        courseName
        section
        semester
      }
    }
  }
`;

