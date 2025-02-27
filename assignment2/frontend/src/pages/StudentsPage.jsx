import { useQuery } from '@apollo/client';
import { GET_ALL_STUDENTS } from '../services/api';
import StudentItem from '../components/Students/StudentItem';
import { Spinner, Alert } from 'react-bootstrap';

// Add default export
const StudentsPage = () => {
  const { loading, error, data } = useQuery(GET_ALL_STUDENTS);

  if (loading) return <Spinner animation="border" className="d-block mx-auto" />;
  if (error) return <Alert variant="danger">{error.message}</Alert>;

  return (
    <div className="students-container container mt-4">
      <h2>All Students</h2>
      {data.students.map(student => (
        <StudentItem key={student._id} student={student} />
      ))}
    </div>
  );
};

export default StudentsPage;