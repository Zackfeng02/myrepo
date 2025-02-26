// Code for the resolvers of the GraphQL API
// Description: Resolvers for the GraphQL API. Defines the logic for each query and mutation. 
const Student = require('../models/Student');
const Course = require('../models/Course');
const bcrypt = require('bcryptjs');
const jwt = require('jsonwebtoken');
const config = require('../config/config');

const resolvers = {
  Query: {
    student: async (_, { id }) => {
      return await Student.findById(id);
    },
    students: async () => {
      return await Student.find({});
    },
    course: async (_, { id }) => {
      return await Course.findById(id);
    },
    courses: async () => {
      return await Course.find({});
    },
    studentsByCourse: async (_, { courseId }) => {
      return await Student.find({ courses: courseId });
    }
  },
  Mutation: {
    registerStudent: async (_, { input }) => {
      const { password } = input;
      const hashedPassword = await bcrypt.hash(password, 12);
      const student = new Student({ ...input, password: hashedPassword });
      return await student.save();
    },
    loginStudent: async (_, { email, password }) => {
      const student = await Student.findOne({ email });
      if (!student) throw new Error("Student not found");
      const isMatch = await bcrypt.compare(password, student.password);
      if (!isMatch) throw new Error("Invalid credentials");
      const token = jwt.sign({ id: student.id }, config.jwtSecret, { expiresIn: '1h' });
      return token;
    },
    addCourse: async (_, { input }) => {
      const course = new Course(input);
      return await course.save();
    },
    updateCourse: async (_, { id, input }) => {
      return await Course.findByIdAndUpdate(id, input, { new: true });
    },
    dropCourse: async (_, { studentId, courseId }) => {
      return await Student.findByIdAndUpdate(
        studentId,
        { $pull: { courses: courseId } },
        { new: true }
      );
    },
    enrollCourse: async (_, { studentId, courseId }) => {
      return await Student.findByIdAndUpdate(
        studentId,
        { $addToSet: { courses: courseId } },
        { new: true }
      );
    }
  },
  Student: {
    courses: async (parent) => {
      return await Course.find({ _id: { $in: parent.courses } });
    }
  }
};

module.exports = resolvers;
