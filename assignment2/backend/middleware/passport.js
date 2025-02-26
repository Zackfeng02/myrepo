const { Strategy: JwtStrategy, ExtractJwt } = require('passport-jwt');
const config = require('../config/config');
const Student = require('../models/Student');

const opts = {
  jwtFromRequest: ExtractJwt.fromAuthHeaderAsBearerToken(),
  secretOrKey: config.jwtSecret
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
