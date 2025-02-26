// Code for defining the GraphQL schema
// The schema defines the types and operations that the API supports
const { gql } = require('apollo-server-express');

const typeDefs = gql`
  type Course {
    id: ID!
    courseCode: String!
    courseName: String!
    section: String!
    semester: String!
  }
  
  type Student {
    id: ID!
    studentNumber: String!
    firstName: String!
    lastName: String!
    address: String!
    city: String!
    phoneNumber: String!
    email: String!
    program: String!
    courses: [Course]
  }
  
  input StudentInput {
    studentNumber: String!
    password: String!
    firstName: String!
    lastName: String!
    address: String!
    city: String!
    phoneNumber: String!
    email: String!
    program: String!
  }
  
  input CourseInput {
    courseCode: String!
    courseName: String!
    section: String!
    semester: String!
  }
  
  type Query {
    student(id: ID!): Student
    students: [Student]
    course(id: ID!): Course
    courses: [Course]
    studentsByCourse(courseId: ID!): [Student]
  }
  
  type Mutation {
    registerStudent(input: StudentInput!): Student
    loginStudent(email: String!, password: String!): String
    addCourse(input: CourseInput!): Course
    updateCourse(id: ID!, input: CourseInput!): Course
    dropCourse(studentId: ID!, courseId: ID!): Student
    enrollCourse(studentId: ID!, courseId: ID!): Student
  }
`;

module.exports = typeDefs;
