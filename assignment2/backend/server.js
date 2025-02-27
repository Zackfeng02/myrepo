const express = require('express');
const mongoose = require('mongoose');
const { ApolloServer } = require('apollo-server-express');
const config = require('./config/config');
const schema = require('./graphql/schema');
const resolvers = require('./graphql/resolvers');
const authMiddleware = require('./middleware/auth');
const { verifyToken } = require('./middleware/auth');

async function startServer() {
  const app = express();
  
  // Connect to MongoDB
  await mongoose.connect(config.MONGODB_URI, { 
    useNewUrlParser: true, 
    useUnifiedTopology: true 
  });

  // Create Apollo Server
  const apolloServer = new ApolloServer({
    schema,
    resolvers,
    context: ({ req }) => {
      try {
        const { studentId } = verifyToken(req);
        return { studentId };
      } catch (err) {
        return {};
      }
    }
  });

  // Start Apollo Server
  await apolloServer.start();

  // Apply middleware
  apolloServer.applyMiddleware({ app });

  // Start Express server
  app.listen(config.PORT, () => {
    console.log(`Server ready at http://localhost:${config.PORT}${apolloServer.graphqlPath}`);
  });
}

startServer().catch(err => console.error(err));