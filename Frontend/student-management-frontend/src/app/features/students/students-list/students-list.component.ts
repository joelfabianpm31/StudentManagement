import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Student } from '../../../core/models/models';
import { StudentService } from '../../../core/services/student.service';
 
@Component({
  selector: 'app-students-list',
  templateUrl: './students-list.component.html',
  styleUrls: ['./students-list.component.scss']
})

export class StudentsListComponent implements OnInit {
  students:   Student[] = [];
  loading   = false;
  error     = "";
  showForm  = false;
  selected?: Student;
  search    = "";
 
  constructor(
    private studentSvc: StudentService,
    private router: Router
  ) {}
 
  ngOnInit(): void { this.load(); }
 
  get filtered(): Student[] {
    if (!this.search.trim()) return this.students;
    const q = this.search.toLowerCase();
    return this.students.filter(s =>
      s.fullName.toLowerCase().includes(q) ||
      s.email.toLowerCase().includes(q) ||
      s.documentNumber.includes(q)
    );
  }
 
  load(): void {
    this.loading = true; this.error = "";
    this.studentSvc.getAll().subscribe({
      next:  d => { this.students = d; this.loading = false; },
      error: e => { this.error = e.error?.message ?? e.message; this.loading = false; }
    });
  }
 
  openCreate(): void  { this.selected = undefined; this.showForm = true; }
  openEdit(s: Student): void { this.selected = s; this.showForm = true; }
  onSaved(): void     { this.showForm = false; this.load(); }
  onCancel(): void    { this.showForm = false; }
 
  viewCourses(id: number): void {
    this.router.navigate(['/students', id, 'courses']);
  }
 
  delete(s: Student): void {
    if (!confirm(`Eliminar a ${s.fullName}? Esta accion no se puede deshacer.`)) return;
    this.studentSvc.delete(s.id).subscribe({
      next:  () => this.load(),
      error: e  => alert(e.error?.message ?? e.message)
    });
  }
}
