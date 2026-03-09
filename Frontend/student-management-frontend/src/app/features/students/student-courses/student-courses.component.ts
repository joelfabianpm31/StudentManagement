import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StudentCoursesDto } from '../../../core/models/models';
import { StudentService } from '../../../core/services/student.service';
 
@Component({
  selector: 'app-student-courses',
  templateUrl: './student-courses.component.html',
  styleUrls: ['./student-courses.component.scss']
})
export class StudentCoursesComponent implements OnInit {
  data?:   StudentCoursesDto;
  loading = true;
  error   = "";
 
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private studentSvc: StudentService
  ) {}
 
  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.studentSvc.getCourses(id).subscribe({
      next:  d => { this.data = d; this.loading = false; },
      error: e => { this.error = e.error?.message ?? e.message; this.loading = false; }
    });
  }
 
  goBack(): void { this.router.navigate(['/students']); }
}
