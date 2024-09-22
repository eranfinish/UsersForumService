// register.component.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { finalize } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { startLoading, stopLoading } from '../../../store/actions/loading.action';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  name: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  userName: string = '';
  mobile: string = '';

  constructor(private userService: UserService
    ,private store: Store<{ loading: boolean }>
    , private router: Router) { }

  register() {
    if (this.password !== this.confirmPassword) {
      alert("Passwords do not match.");
      return;
    }

    const user = {
      id: 0,
      name: this.name,
      email: this.email,
      password: this.password,
      userName: this.userName,
      token: '', // Server will handle token
      isLogedIn: true,
      lastEntrance: new Date().toISOString(),
      mobile: this.mobile
    };
    this.store.dispatch(startLoading());
    this.userService.register(user)
    .pipe(
      finalize(() => {
        this.store.dispatch(stopLoading());  // Start loading
      })
    )
    .subscribe({
      next: () => {
        console.log('Registration successful');
        this.router.navigate(['/forum']);
      },
      error: (error) => {
        alert(error.error.message);
        console.error('Registration failed', error);
      }
    });
  }
}
