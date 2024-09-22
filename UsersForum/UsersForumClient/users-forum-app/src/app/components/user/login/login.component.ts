// login.component.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { finalize } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { startLoading, stopLoading } from '../../../store/actions/loading.action';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';

  constructor(private userService: UserService
    ,private store: Store<{ loading: boolean }>
    ,private router: Router) { }

  login() {
    this.store.dispatch(startLoading());  // Start loading
    this.userService.login({ username: this.username, password: this.password })
    .pipe(
      finalize(() => {
        this.store.dispatch(stopLoading());  // Stop loading
      })
    )
    .subscribe({
      next: () => {
        console.log('Login successful');
        this.router.navigate(['/forum']);  // Navigate to the forum page after login
      },
      error: (error) => {
        console.error('Login failed', error);
      }
    });
  }
}
