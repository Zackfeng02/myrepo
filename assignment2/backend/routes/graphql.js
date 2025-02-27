const { ApolloServer } = require('apollo-server-express');
const schema = require('../graphql/schema');
const resolvers = require('../graphql/resolvers');
const authMiddleware = require('../middleware/auth');

const server = new ApolloServer({
  typeDefs: schema,
  resolvers,
  context: ({ req }) => {
    try {
      const { studentId } = authMiddleware(req);
      return { studentId };
    } catch (err) {
      return {};
    }
  }
});

module.exports = server;