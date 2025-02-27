const { buildSchema } = require('graphql');

const schema = buildSchema(`
  type Student {
    _id: ID!
    studentNumber: String!
    firstName: String!
    lastName: String!
    email: String!
    program: String!
    courses: [Course!]
  }

  type Course {
    _id: ID!
    courseCode: String!
    courseName: String!
    section: String!
    semester: String!
    students: [Student!]
  }

  type AuthPayload {
    token: String!
    student: Student!
  }

  input StudentInput {
    studentNumber: String!
    password: String!
    firstName: String!
    lastName: String!
    email: String!
    program: String!
    address: String!
    city: String!
    phone: String!
  }

  input CourseInput {
    courseCode: String!
    courseName: String!
    section: String!
    semester: String!
  }

  type Query {
    student: Student        # Added field for current student
    students: [Student!]
    courses: [Course!]
    studentsInCourse(courseId: ID!): [Student!]
  }

  type Mutation {
    signup(studentInput: StudentInput!): AuthPayload!
    login(email: String!, password: String!): AuthPayload!
    addCourse(courseInput: CourseInput!): Course!
    updateCourse(courseId: ID!, section: String!): Course!
    enrollCourse(courseId: ID!): Student!
    dropCourse(courseId: ID!): Student!
  }
`);

module.exports = schema;
