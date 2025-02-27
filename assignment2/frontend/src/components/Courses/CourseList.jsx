import { useQuery } from '@apollo/client';
import { GET_COURSES } from '../../services/api';
import CourseItem from './CourseItem';  // Default import
import Spinner from 'react-bootstrap/Spinner';
import Alert from 'react-bootstrap/Alert';

const CourseList = () => {
  const { loading, error, data } = useQuery(GET_COURSES);

  if (loading) return <Spinner animation="border" variant="primary" />;
  if (error) return <Alert variant="danger">{error.message}</Alert>;

  return (
    <div className="course-list">
      {data.courses.map(course => (
        <CourseItem 
          key={course._id} 
          course={course} 
        />
      ))}
    </div>
  );
};

export default CourseList;