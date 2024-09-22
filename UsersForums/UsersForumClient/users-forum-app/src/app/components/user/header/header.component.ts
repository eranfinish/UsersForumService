// header.component.ts
import { Component } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  constructor(public userService: UserService, private router: Router) {}
loading: boolean = false;
  logout() {
    this.userService.logout();
    this.router.navigate(['/login']);  // Redirect to login page after logout
  }
}
