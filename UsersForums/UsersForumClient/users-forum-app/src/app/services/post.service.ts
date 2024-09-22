// post.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  private baseUrl = `${environment.apiUrl}/post`;

  constructor(private http: HttpClient) { }

  getAllPosts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/getAllPosts`);
  }

  getPostById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/getPostById/${id}`);
  }

  addNewPost(post: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/addNewPost`, post);
  }

  updatePost( post: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/editPost`, post);
  }

  deletePost(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/deletePost/${id}`);
  }


}
