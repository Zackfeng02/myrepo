import { useMutation } from '@apollo/client';
import { ENROLL_COURSE, DROP_COURSE } from '../../services/api';
import Button from 'react-bootstrap/Button';
import { useAuth } from '../../context/AuthContext';

const CourseItem = ({ course }) => {
  const [enroll] = useMutation(ENROLL_COURSE);
  const [drop] = useMutation(DROP_COURSE);
  const { currentStudent } = useAuth();

  const isEnrolled = currentStudent?.courses?.includes(course._id);

  const handleAction = async () => {
    try {
      const mutation = isEnrolled ? drop : enroll;
      await mutation({ variables: { courseId: course._id } });
    } catch (err) {
      console.error('Operation failed:', err.message);
    }
  };

  return (
    <div className="course-item card mb-3">
      <div className="card-body">
        <h5 className="card-title">{course.courseCode} - {course.courseName}</h5>
        <p className="card-text">
          Section: {course.section}<br />
          Semester: {course.semester}
        </p>
        <Button 
          variant={isEnrolled ? 'danger' : 'success'}
          onClick={handleAction}
        >
          {isEnrolled ? 'Drop Course' : 'Enroll'}
        </Button>
      </div>
    </div>
  );
};

export default CourseItem;  // Default export