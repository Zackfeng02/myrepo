import { useQuery } from '@apollo/client';
import { GET_STUDENT_COURSES } from '../services/api';
import { useAuth } from '../context/AuthContext';
import { Card, Button, Spinner, Alert, Container, Row, Col } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const StudentDashboard = () => {
  const { currentStudent, logout } = useAuth();
  const { loading, error, data, refetch } = useQuery(GET_STUDENT_COURSES, {
    fetchPolicy: 'network-and-cache'
  });

  const handleDropCourse = async (courseId) => {
    // Implement drop course mutation
  };

  const handleSectionChange = (courseId) => {
    // Implement section change logic
  };

  if (!currentStudent) {
    return (
      <Container className="mt-5">
        <Alert variant="warning">
          Please <Link to="/login">login</Link> to view your dashboard
        </Alert>
      </Container>
    );
  }

  if (loading) return <Spinner animation="border" className="d-block mx-auto mt-5" />;
  if (error) return <Alert variant="danger" className="mt-5">{error.message}</Alert>;

  const enrolledCourses = data?.student?.courses || [];

  return (
    <Container className="mt-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>Welcome, {currentStudent.firstName}!</h2>
        <Button variant="outline-danger" onClick={logout}>Logout</Button>
      </div>

      <h4 className="mb-3">Your Enrolled Courses</h4>
      
      {enrolledCourses.length === 0 ? (
        <Alert variant="info">
          You're not enrolled in any courses yet. <Link to="/courses">Browse courses</Link>
        </Alert>
      ) : (
        <Row xs={1} md={2} lg={3} className="g-4">
          {enrolledCourses.map(course => (
            <Col key={course._id}>
              <Card className="h-100 shadow-sm">
                <Card.Body>
                  <Card.Title>{course.courseCode} - {course.courseName}</Card.Title>
                  <Card.Subtitle className="mb-2 text-muted">
                    Section {course.section} | {course.semester}
                  </Card.Subtitle>
                  <div className="mt-3 d-grid gap-2">
                    <Button 
                      variant="outline-danger" 
                      size="sm"
                      onClick={() => handleDropCourse(course._id)}
                    >
                      Drop Course
                    </Button>
                    <Button
                      variant="outline-primary"
                      size="sm"
                      onClick={() => handleSectionChange(course._id)}
                    >
                      Change Section
                    </Button>
                  </div>
                </Card.Body>
              </Card>
            </Col>
          ))}
        </Row>
      )}
    </Container>
  );
};

export default StudentDashboard;