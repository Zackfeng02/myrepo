// Exporting an object that contains the MongoDB URI and the JWT secret key
// The MongoDB URI is stored in the MONGO_URI environment variable
require('dotenv').config();

module.exports = {
  mongoURI: process.env.MONGO_URI || "mongodb://localhost:27017/student_course_db",
  jwtSecret: process.env.JWT_SECRET || "my-32-character-ultra-secure-and-ultra-long-secret",
  port: process.env.PORT || 5000
};
