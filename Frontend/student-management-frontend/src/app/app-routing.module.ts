import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StudentsListComponent }    from './features/students/students-list/students-list.component';
import { StudentCoursesComponent }  from './features/students/student-courses/student-courses.component';
import { CoursesListComponent }     from './features/courses/courses-list/courses-list.component';
import { EnrollmentsListComponent } from './features/enrollments/enrollments-list/enrollments-list.component';
 
const routes: Routes = [
  { path: '',                     redirectTo: 'students', pathMatch: 'full' },
  { path: 'students',             component: StudentsListComponent },
  { path: 'students/:id/courses', component: StudentCoursesComponent },
  { path: 'courses',              component: CoursesListComponent },
  { path: 'enrollments',          component: EnrollmentsListComponent },
];
 
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
