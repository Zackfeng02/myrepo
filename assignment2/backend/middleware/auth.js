const { Strategy: JwtStrategy, ExtractJwt } = require('passport-jwt');
const jwt = require('jsonwebtoken');
const config = require('../config/config');
const Student = require('../models/Student');

const opts = {
  jwtFromRequest: ExtractJwt.fromAuthHeaderAsBearerToken(),
  secretOrKey: config.jwtSecret, // using config.jwtSecret consistently
};

// Initialize Passport JWT strategy
const initializePassport = (passport) => {
  passport.use(
    new JwtStrategy(opts, async (jwt_payload, done) => {
      try {
        // Assuming token payload contains an 'id' field; update if needed.
        const student = await Student.findById(jwt_payload.id);
        if (student) {
          return done(null, student);
        }
        return done(null, false);
      } catch (error) {
        return done(error, false);
      }
    })
  );
};

// Helper function to verify token manually from request
const verifyToken = (req) => {
  const authHeader = req.headers.authorization;
  if (!authHeader) {
    throw new Error('Not authenticated!');
  }
  const token = authHeader.split(' ')[1];
  // Use the same secret from config
  const decoded = jwt.verify(token, config.jwtSecret);
  // Return the student's id from the token payload (assumed to be 'id')
  return { studentId: decoded.id };
};

module.exports = {
  initializePassport,
  verifyToken,
};
