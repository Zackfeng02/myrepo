// Description: Contains all the validation functions.
// The isValidEmail function checks if the email is valid.
module.exports = {
    isValidEmail: function(email) {
      const re = /\S+@\S+\.\S+/;
      return re.test(email);
    }
  };
  