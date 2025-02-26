// Description: Main entry point for the server. Initializes Express, MongoDB, Passport, and Apollo Server.
// The server listens on the specified port and connects to the MongoDB database.
require('dotenv').config();
const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
const passport = require('passport');
const { ApolloServer } = require('apollo-server-express');

const config = require('./config/config');
const typeDefs = require('./graphql/typeDefs');
const resolvers = require('./graphql/resolvers');

// Initialize Express
const app = express();

// Middleware
app.use(cors());
app.use(express.json());

// Initialize Passport and configure JWT strategy
app.use(passport.initialize());
require('./middleware/passport')(passport);

// Connect to MongoDB
mongoose.connect(config.mongoURI, { useNewUrlParser: true, useUnifiedTopology: true })
  .then(() => console.log('MongoDB connected'))
  .catch(err => console.error(err));

// Create Apollo Server
async function startApolloServer() {
  const server = new ApolloServer({
    typeDefs,
    resolvers,
    context: ({ req }) => {
      // Passport attaches user to req if authenticated
      return { user: req.user };
    }
  });
  await server.start();
  server.applyMiddleware({ app, path: '/graphql' });
}

startApolloServer();

// Start Express server
app.listen(config.port, () => {
  console.log(`Server running on port ${config.port}`);
});
