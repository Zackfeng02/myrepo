// Desc: Route for GraphQL API
const express = require('express');
const router = express.Router();
const { graphqlHTTP } = require('express-graphql');
const schema = require('../graphql/typeDefs');
const authMiddleware = require('../middleware/passport');

// Apply authentication to all GraphQL requests
router.use(authMiddleware);

router.use('/', graphqlHTTP({
  schema,
  graphiql: true
}));

module.exports = router;
