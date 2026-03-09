import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  ApiResponse, Student, CreateStudentDto,
  UpdateStudentDto, StudentCoursesDto
} from '../models/models';
 
@Injectable({ providedIn: 'root' })
export class StudentService {
  private readonly url = '/api/students';
  constructor(private http: HttpClient) {}
 
  getAll(): Observable<Student[]> {
    return this.http
      .get<ApiResponse<Student[]>>(this.url)
      .pipe(map(r => r.data));
  }
  getById(id: number): Observable<Student> {
    return this.http
      .get<ApiResponse<Student>>(`${this.url}/${id}`)
      .pipe(map(r => r.data));
  }
  getCourses(id: number): Observable<StudentCoursesDto> {
    return this.http
      .get<ApiResponse<StudentCoursesDto>>(`${this.url}/${id}/courses`)
      .pipe(map(r => r.data));
  }
  create(dto: CreateStudentDto): Observable<Student> {
    return this.http
      .post<ApiResponse<Student>>(this.url, dto)
      .pipe(map(r => r.data));
  }
  update(id: number, dto: UpdateStudentDto): Observable<Student> {
    return this.http
      .put<ApiResponse<Student>>(`${this.url}/${id}`, dto)
      .pipe(map(r => r.data));
  }
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
