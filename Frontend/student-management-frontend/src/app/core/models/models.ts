export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data:    T;
}

export interface Student {
  id:              number;
  firstName:       string;
  lastName:        string;
  fullName:        string;
  email:           string;
  documentNumber:  string;
  birthDate:       string;
  createdAt:       string;
  isActive:        boolean;
  enrollmentCount: number;
}
export interface CreateStudentDto {
  firstName: string; lastName: string; email: string;
  documentNumber: string; birthDate: string;
}
export interface UpdateStudentDto extends CreateStudentDto { isActive: boolean; }

export interface Course {
  id: number; name: string; code: string; description: string;
  credits: number; maxStudents: number; enrolledCount: number;
  startDate: string; endDate: string; isActive: boolean;
}
export interface CreateCourseDto {
  name: string; code: string; description: string;
  credits: number; maxStudents: number;
  startDate: string; endDate: string;
}
export interface UpdateCourseDto extends CreateCourseDto { isActive: boolean; }
 
export interface EnrollmentDetail {
  id: number; courseId: number; courseName: string;
    courseCode: string; credits: number; grade?: number; status: string;
}
export interface Enrollment {
  id: number; studentId: number; studentName: string; studentEmail: string;
  enrollmentDate: string; status: string; notes?: string; createdAt: string;
  details: EnrollmentDetail[];
}
export interface CreateEnrollmentDto {
  studentId: number; notes?: string; courseIds: number[];
}
export interface UpdateEnrollmentDto {
  status: string; notes?: string;
  details: { courseId: number; grade?: number; status: string; }[];
}

export interface EnrolledCourseDto {
  enrollmentId: number; courseId: number; courseName: string;
  courseCode: string; credits: number; startDate: string;
  endDate: string; enrollmentStatus: string; grade?: number;
}
export interface StudentCoursesDto {
  studentId: number; studentName: string; email: string;
  courses: EnrolledCourseDto[];
}
