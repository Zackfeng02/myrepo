import { useQuery } from '@apollo/client';
import { GET_ALL_STUDENTS } from '../services/api';
import StudentItem from '../components/Students/StudentItem';

const StudentsPage = () => {
  const { loading, error, data } = useQuery(GET_ALL_STUDENTS);

  if (loading) return <div>Loading students...</div>;
  if (error) return <div>Error loading students: {error.message}</div>;

  return (
    <div className="students-container">
      <h2>All Students</h2>
      {data.students.map(student => (
        <StudentItem key={student._id} student={student} />
      ))}
    </div>
  );
};