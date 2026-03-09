import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Student, Course, CreateEnrollmentDto } from '../../../core/models/models';
import { StudentService }    from '../../../core/services/student.service';
import { CourseService }     from '../../../core/services/course.service';
import { EnrollmentService } from '../../../core/services/enrollment.service';
 
@Component({
  selector: 'app-enrollment-form',
  templateUrl: './enrollment-form.component.html',
  styleUrls: ['./enrollment-form.component.scss']
})
export class EnrollmentFormComponent implements OnInit {
  @Output() saved     = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();
 
  students:         Student[] = [];
  courses:          Course[]  = [];
  selectedCourseIds = new Set<number>();
  form!: FormGroup;
  submitting = false;
  errorMsg   = "";
 
  constructor(
    private fb:             FormBuilder,
    private studentSvc:     StudentService,
    private courseSvc:      CourseService,
    private enrollmentSvc:  EnrollmentService
  ) {}
 
  ngOnInit(): void {
    this.form = this.fb.group({
      studentId: ['', Validators.required],
      notes:     ['']
    });
    this.studentSvc.getAll().subscribe(d => this.students = d);
    this.courseSvc.getAll().subscribe(d =>
      this.courses = d.filter(c => c.isActive));
  }
 
  toggleCourse(id: number): void {
    this.selectedCourseIds.has(id)
      ? this.selectedCourseIds.delete(id)
      : this.selectedCourseIds.add(id);
  }
  isSelected(id: number): boolean {
    return this.selectedCourseIds.has(id);
  }
 
  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    if (this.selectedCourseIds.size === 0) {
      this.errorMsg = 'Debes seleccionar al menos un curso.'; return;
    }
    this.submitting = true; this.errorMsg = "";
    const dto: CreateEnrollmentDto = {
      ...this.form.value,
      courseIds: Array.from(this.selectedCourseIds)
    };
    this.enrollmentSvc.create(dto).subscribe({
      next:  () => { this.submitting = false; this.saved.emit(); },
      error: e  => { this.errorMsg = e.error?.message ?? e.message; this.submitting = false; }
    });
  }
}
