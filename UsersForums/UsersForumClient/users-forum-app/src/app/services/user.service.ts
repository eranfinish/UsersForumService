// user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, lastValueFrom } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { Store } from '@ngrx/store';
import { User } from '../models/user';
import { startLoading, stopLoading } from '../store/actions/loading.action';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = `${environment.apiUrl}/user`;
  private token: string | null = null;
loading: boolean = false;

  constructor(private http: HttpClient, private store: Store<{ loading: boolean }>) { }

  register(user: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/register`, user).pipe(
      tap(res => {
        this.setToken(res.token),
        this.setUser(res.user)
      })  // Assume the token is sent back with the registration response
    );
  }

  login(credentials: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/login`, credentials).pipe(
      tap(res => {
        this.setToken(res.token),
        this.setUser(res.user)
      })  // Save token and user upon login

    );
  }

  logout() {
    this.store.dispatch(startLoading());  // Start loading
    this.loading = true;  // Show loading spinner
 let user = localStorage.getItem('user');
lastValueFrom(this.http.post<any>(`${this.baseUrl}/Logout`, user))
    .then(response => {
        console.log('Logout successful', response);
    })
    .catch(error => {
        console.error('Logout failed', error);
    })
    .finally(() => {
        this.loading = false;  // Hide loading spinner
         this.token = null;
    localStorage.removeItem('jwtToken');  // Clear token from local storage
    localStorage.removeItem('user');  // Clear token from local storage
    this.store.dispatch(stopLoading());  // Stop loading
  });




  }

  setToken(token: string) {
    this.token = token;
    localStorage.setItem('jwtToken', token);  // Store token in local storage
  }


  getToken(): string | null {
    return localStorage.getItem('jwtToken');
  }

  setUser(user: User  | null) {
  localStorage.setItem('user', JSON.stringify(user));
  }

  getUser(): User | null {
    let user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }

  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }
}
