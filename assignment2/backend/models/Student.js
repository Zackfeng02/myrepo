const mongoose = require('mongoose');

const studentSchema = new mongoose.Schema({
  studentNumber: { type: String, required: true, unique: true },
  password: { type: String, required: true },
  firstName: { type: String, required: true },
  lastName: { type: String, required: true },
  email: { type: String, required: true, unique: true },
  program: { type: String, required: true },
  courses: [{ 
    type: mongoose.Schema.Types.ObjectId, 
    ref: 'Course' ,
    validate: {
      validator: async function(courseId) {
        const course = await mongoose.model('Course').findById(courseId);
        return !!course;
      },
      message: 'Course does not exist'
    }
  }]
});

module.exports = mongoose.model('Student', studentSchema);