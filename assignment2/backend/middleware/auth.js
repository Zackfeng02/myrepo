const { Strategy: JwtStrategy, ExtractJwt } = require('passport-jwt');
const config = require('../config/config');
const Student = require('../models/Student');

const opts = {
  jwtFromRequest: ExtractJwt.fromAuthHeaderAsBearerToken(),
  secretOrKey: config.jwtSecret
};

const jwt = require('jsonwebtoken');
module.exports = (req) => {
  const authHeader = req.headers.authorization;
  if (!authHeader) throw new Error('Not authenticated!');
  const token = authHeader.split(' ')[1];
  const decoded = jwt.verify(token, process.env.JWT_SECRET);
  return { studentId: decoded.studentId };
};

module.exports = (passport) => {
  passport.use(new JwtStrategy(opts, async (jwt_payload, done) => {
    try {
      const student = await Student.findById(jwt_payload.id);
      if (student) {
        return done(null, student);
      }
      return done(null, false);
    } catch (error) {
      return done(error, false);
    }
  }));
};
