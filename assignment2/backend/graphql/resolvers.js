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
      const token = jwt.sign({ studentId: student._id }, config.JWT_SECRET, { expiresIn: '1h' });
      return { token, student };
    },
    login: async (_, { email, password }) => {
      console.log('Login attempt for:', email);
      const student = await Student.findOne({ email });
      if (!student) {
        console.error('No student found for email:', email);
        throw new AuthenticationError('Invalid credentials');
      }
      const isValid = await bcrypt.compare(password, student.password);
      console.log('Password valid:', isValid);
      if (!isValid) {
        console.error('Password mismatch for email:', email);
        throw new AuthenticationError('Invalid credentials');
      }
      try {
        const token = jwt.sign({ studentId: student._id }, config.JWT_SECRET, { expiresIn: '1h' });
        console.log('Login successful for:', email);
        return { token, student };
      } catch (err) {
        console.error('Error during token signing:', err);
        throw new Error('Login failed');
      }
    },
  }
};

module.exports = resolvers;