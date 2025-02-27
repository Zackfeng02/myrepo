// Exporting an object that contains the MongoDB URI and the JWT secret key
// The MongoDB URI is stored in the MONGO_URI environment variable
require('dotenv').config();

module.exports = {
  MONGODB_URI: process.env.MONGODB_URI || 'mongodb://localhost:27017/student-courses',
  JWT_SECRET: process.env.JWT_SECRET || 'supersecretkey',
  PORT: process.env.PORT || 4000
};
