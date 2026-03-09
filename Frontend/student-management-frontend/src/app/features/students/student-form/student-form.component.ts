import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Student, CreateStudentDto, UpdateStudentDto } from '../../../core/models/models';
import { StudentService } from '../../../core/services/student.service';
 
@Component({
  selector: 'app-student-form',
  templateUrl: './student-form.component.html',
  styleUrls: ['./student-form.component.scss']
})
export class StudentFormComponent implements OnInit {
  @Input()  student?: Student;
  @Output() saved     = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();
 
  form!: FormGroup;
  submitting = false;
  errorMsg   = "";
 
  constructor(private fb: FormBuilder, private svc: StudentService) {}
 
  get isEditing(): boolean { return !!this.student; }
  get f() { return this.form.controls; }
 
  ngOnInit(): void {
    this.form = this.fb.group({
      firstName:      [this.student?.firstName      ?? '', [Validators.required, Validators.minLength(2)]],
      lastName:       [this.student?.lastName       ?? '', [Validators.required, Validators.minLength(2)]],
      email:          [this.student?.email          ?? '', [Validators.required, Validators.email]],
      documentNumber: [this.student?.documentNumber ?? '', [Validators.required, Validators.minLength(5)]],
      birthDate:      [this.student?.birthDate?.split('T')[0] ?? '', [Validators.required]],
      isActive:       [this.student?.isActive ?? true],
    });
  }
 
  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.submitting = true; this.errorMsg = "";
    const obs = this.isEditing
      ? this.svc.update(this.student!.id, this.form.value as UpdateStudentDto)
      : this.svc.create(this.form.value as CreateStudentDto);
    obs.subscribe({
      next:  () => { this.submitting = false; this.saved.emit(); },
      error: e  => { this.errorMsg = e.error?.message ?? e.message; this.submitting = false; }
    });
  }
}
