import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiResponse, Course, CreateCourseDto, UpdateCourseDto } from '../models/models';
 
@Injectable({ providedIn: 'root' })
export class CourseService {
  private readonly url = '/api/courses';
  constructor(private http: HttpClient) {}
 
  getAll(): Observable<Course[]> {
    return this.http.get<ApiResponse<Course[]>>(this.url).pipe(map(r => r.data));
  }
  getById(id: number): Observable<Course> {
    return this.http
      .get<ApiResponse<Course>>(`${this.url}/${id}`).pipe(map(r => r.data));
  }
  create(dto: CreateCourseDto): Observable<Course> {
    return this.http
      .post<ApiResponse<Course>>(this.url, dto).pipe(map(r => r.data));
  }
  update(id: number, dto: UpdateCourseDto): Observable<Course> {
    return this.http
      .put<ApiResponse<Course>>(`${this.url}/${id}`, dto).pipe(map(r => r.data));
  }
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
