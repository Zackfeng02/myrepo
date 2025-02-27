import { useMutation } from '@apollo/client';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { Button, Alert, Card } from 'react-bootstrap';
import { Link, useNavigate } from 'react-router-dom';
import { SIGNUP_STUDENT } from '../../services/api';
import { useAuth } from '../../context/AuthContext';

const SignupSchema = Yup.object().shape({
  studentNumber: Yup.string().required('Required'),
  password: Yup.string().min(6, 'Too short!').required('Required'),
  firstName: Yup.string().required('Required'),
  lastName: Yup.string().required('Required'),
  email: Yup.string().email('Invalid email').required('Required'),
  program: Yup.string().required('Required'),
  address: Yup.string().required('Required'),
  city: Yup.string().required('Required'),
  phone: Yup.string().matches(/^[0-9]{10}$/, 'Invalid phone number').required('Required')
});

const Signup = () => {
  const [signup] = useMutation(SIGNUP_STUDENT);
  const { login } = useAuth();
  const navigate = useNavigate();

  return (
    <Card className="mx-auto mt-5" style={{ maxWidth: '500px' }}>
      <Card.Body>
        <h2 className="text-center mb-4">Student Signup</h2>
        <Formik
          initialValues={{
            studentNumber: '',
            password: '',
            firstName: '',
            lastName: '',
            email: '',
            program: '',
            address: '',
            city: '',
            phone: ''
          }}
          validationSchema={SignupSchema}
          onSubmit={async (values, { setSubmitting, setErrors }) => {
            try {
              const { data } = await signup({ variables: { studentInput: values } });
              login(data.signup.token, data.signup.student);
              navigate('/dashboard');
            } catch (err) {
              setErrors({ general: err.message });
            }
            setSubmitting(false);
          }}
        >
          {({ isSubmitting, errors }) => (
            <Form>
              {errors.general && <Alert variant="danger">{errors.general}</Alert>}

              <div className="mb-3">
                <label htmlFor="studentNumber" className="form-label">Student Number</label>
                <Field name="studentNumber" type="text" className="form-control" />
                <ErrorMessage name="studentNumber" component="div" className="text-danger" />
              </div>

              <div className="mb-3">
                <label htmlFor="password" className="form-label">Password</label>
                <Field name="password" type="password" className="form-control" />
                <ErrorMessage name="password" component="div" className="text-danger" />
              </div>

              <div className="row g-3 mb-3">
                <div className="col">
                  <label htmlFor="firstName" className="form-label">First Name</label>
                  <Field name="firstName" type="text" className="form-control" />
                  <ErrorMessage name="firstName" component="div" className="text-danger" />
                </div>
                <div className="col">
                  <label htmlFor="lastName" className="form-label">Last Name</label>
                  <Field name="lastName" type="text" className="form-control" />
                  <ErrorMessage name="lastName" component="div" className="text-danger" />
                </div>
              </div>

              <div className="mb-3">
                <label htmlFor="email" className="form-label">Email</label>
                <Field name="email" type="email" className="form-control" />
                <ErrorMessage name="email" component="div" className="text-danger" />
              </div>

              <div className="mb-3">
                <label htmlFor="program" className="form-label">Program</label>
                <Field name="program" as="select" className="form-select">
                  <option value="">Select Program</option>
                  <option value="Computer Science">Computer Science</option>
                  <option value="Information Technology">Information Technology</option>
                  <option value="Software Engineering">Software Engineering</option>
                </Field>
                <ErrorMessage name="program" component="div" className="text-danger" />
              </div>

              <div className="mb-3">
                <label htmlFor="address" className="form-label">Address</label>
                <Field name="address" type="text" className="form-control" />
                <ErrorMessage name="address" component="div" className="text-danger" />
              </div>

              <div className="row g-3 mb-3">
                <div className="col">
                  <label htmlFor="city" className="form-label">City</label>
                  <Field name="city" type="text" className="form-control" />
                  <ErrorMessage name="city" component="div" className="text-danger" />
                </div>
                <div className="col">
                  <label htmlFor="phone" className="form-label">Phone</label>
                  <Field name="phone" type="tel" className="form-control" />
                  <ErrorMessage name="phone" component="div" className="text-danger" />
                </div>
              </div>

              <Button 
                type="submit" 
                disabled={isSubmitting} 
                className="w-100 mt-3"
                variant="primary"
              >
                {isSubmitting ? 'Creating Account...' : 'Sign Up'}
              </Button>

              <div className="mt-3 text-center">
                Already have an account? <Link to="/login">Login here</Link>
              </div>
            </Form>
          )}
        </Formik>
      </Card.Body>
    </Card>
  );
};

export default Signup;