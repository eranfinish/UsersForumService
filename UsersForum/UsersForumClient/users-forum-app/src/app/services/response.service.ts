// response.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {Response} from '../models/response';
@Injectable({
  providedIn: 'root'
})
export class ResponseService {
  private baseUrl = `${environment.apiUrl}/response`;

  constructor(private http: HttpClient) { }

  getAllResponses(postId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/for-post/${postId}`);
  }

  addResponse(response: Response): Observable<any> {
    return this.http.post(`${this.baseUrl}/addNewResponse`, response);
  }

  updateResponse( response: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/editResponse`, response);
  }

  deleteResponse(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
