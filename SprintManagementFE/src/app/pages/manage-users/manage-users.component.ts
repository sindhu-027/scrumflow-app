
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-manage-users',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './manage-users.component.html',
})
export class ManageUsersComponent implements OnInit {

  logo: string = 'assets/ScrumLogo.png';

  users: any[] = [];
  roles: string[] = ['ProductOwner', 'ScrumMaster', 'Developer'];
  role: string = '';

  constructor(
    private userService: UserService,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.loadCurrentUser();
    this.getUsers();
  }

  // 🔥 GET CURRENT USER ROLE
  loadCurrentUser() {
    this.auth.getCurrentUser().subscribe({
      next: (res: any) => {
        this.role = res.role;
      },
      error: (err) => {
        console.error('Auth error', err);
      }
    });
  }

  // 🔥 GET USERS
  getUsers() {
    this.userService.getUsers().subscribe({
      next: (res: any) => {
        this.users = res;
      },
      error: (err) => {
        console.error('Failed to load users', err);
        alert('Unable to load users');
      }
    });
  }

  // 🔥 ASSIGN ROLE
  assignRole(user: any) {
    this.userService.assignRole(user.id, user.role).subscribe({
      next: () => {
        alert('Role updated successfully');
        this.getUsers(); // refresh
      },
      error: (err) => {
        console.error(err);
        alert('Failed to assign role');
      }
    });
  }
}