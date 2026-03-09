import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  ApiResponse, Enrollment, CreateEnrollmentDto, UpdateEnrollmentDto
} from '../models/models';
 
@Injectable({ providedIn: 'root' })
export class EnrollmentService {
  private readonly url = '/api/enrollments';
  constructor(private http: HttpClient) {}
 
  getAll(): Observable<Enrollment[]> {
    return this.http.get<ApiResponse<Enrollment[]>>(this.url).pipe(map(r => r.data));
  }
  getById(id: number): Observable<Enrollment> {
    return this.http
      .get<ApiResponse<Enrollment>>(`${this.url}/${id}`).pipe(map(r => r.data));
  }
  getByStudent(studentId: number): Observable<Enrollment[]> {
    return this.http
      .get<ApiResponse<Enrollment[]>>(`${this.url}/student/${studentId}`)
      .pipe(map(r => r.data));
  }
  create(dto: CreateEnrollmentDto): Observable<Enrollment> {
    return this.http
      .post<ApiResponse<Enrollment>>(this.url, dto).pipe(map(r => r.data));
  }
  update(id: number, dto: UpdateEnrollmentDto): Observable<Enrollment> {
    return this.http
      .put<ApiResponse<Enrollment>>(`${this.url}/${id}`, dto).pipe(map(r => r.data));
  }
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
