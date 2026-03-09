import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent }          from './shared/navbar/navbar.component';
import { StudentsListComponent }    from './features/students/students-list/students-list.component';
import { StudentFormComponent }     from './features/students/student-form/student-form.component';
import { StudentCoursesComponent }  from './features/students/student-courses/student-courses.component';
import { CoursesListComponent }     from './features/courses/courses-list/courses-list.component';
import { CourseFormComponent }      from './features/courses/course-form/course-form.component';
import { EnrollmentsListComponent } from './features/enrollments/enrollments-list/enrollments-list.component';
import { EnrollmentFormComponent }  from './features/enrollments/enrollment-form/enrollment-form.component';
 
@NgModule({
  declarations: [
    AppComponent, NavbarComponent,
    StudentsListComponent, StudentFormComponent, StudentCoursesComponent,
    CoursesListComponent, CourseFormComponent,
    EnrollmentsListComponent, EnrollmentFormComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    AppRoutingModule,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
