const { AuthenticationError } = require('apollo-server-express');
const bcrypt = require('bcryptjs');
const jwt = require('jsonwebtoken');
const Student = require('../models/Student');
const Course = require('../models/Course');
const config = require('../config/config');

const resolvers = {
  Query: {
    students: async () => await Student.find().populate('courses'),
    courses: async () => await Course.find().populate('students'),
    studentsInCourse: async (_, { courseId }) => {
      const course = await Course.findById(courseId).populate('students');
      return course.students;
    }
  },
  Mutation: {
    signup: async (_, { studentInput }) => {
      const hashedPassword = await bcrypt.hash(studentInput.password, 12);
      const student = new Student({ ...studentInput, password: hashedPassword });
      await student.save();
      const token = jwt.sign({ id: student._id }, config.jwtSecret, { expiresIn: '1h' });
      return { token, student };
    },
    login: async (_, { email, password }) => {
      const student = await Student.findOne({ email });
      if (!student) {
        console.error('No student found for email:', email);
        throw new AuthenticationError('Invalid credentials');
      }
      const passwordValid = await bcrypt.compare(password, student.password);
      if (!passwordValid) {
        console.error('Password does not match for:', email);
        throw new AuthenticationError('Invalid credentials');
      }
      try {
        const token = jwt.sign({ id: student._id }, config.jwtSecret, { expiresIn: '1h' });
        if (!token) {
          console.error('Token signing failed');
          throw new Error('Token signing failed');
        }
        return { token, student };
      } catch (err) {
        console.error('Error during token signing:', err);
        throw new Error('Login failed');
      }
    },
  }
};

module.exports = resolvers;