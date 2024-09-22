// auth.interceptor.ts
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
//import { UserService } from '../services/user.service';  // Assume you have an AuthService to handle logout

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private userService: UserService, private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler):
   Observable<HttpEvent<any>> {

    const token = this.userService.getToken();
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        // Optionally handle HTTP errors like 401 Unauthorized
        if (error.status === 401) {
      // 401 Unauthorized - log the user out and redirect to the login page
      this.userService.logout();  // Assuming logout is implemented in your AuthService
      this.router.navigate(['/login']);  // Redirect to login page
        }
        return throwError(error);
      })
    );
  }
}
