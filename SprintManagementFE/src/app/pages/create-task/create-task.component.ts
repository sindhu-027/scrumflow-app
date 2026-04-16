
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-task',
  standalone: true,
  imports: [FormsModule, RouterModule, CommonModule],
  templateUrl: './create-task.component.html',
})
export class CreateTaskComponent implements OnInit {

  logo: string = 'assets/ScrumLogo.png';

  //role
  role: string = '';

  // 🔥 Sprint
  sprints: any[] = [];
  selectedSprintId: string = '';

  // 🔥 Users
  users: any[] = [];
  selectedUserId: string = '';

  // 🔥 Task model
  task: any = {
    title: '',
    description: '',
    status: 'To Do',
    priority: '',
    storyName: '',
    storyPoints: null
  };

  constructor(private auth: AuthService, private router: Router) {}

  ngOnInit(): void {

    // Load role
    this.auth.getCurrentUser().subscribe({
      next: (res: any) => {
        this.role = res.role;
      },
      error: () => {
        alert('Session expired. Please login again');
      }
    });

    // Load sprints
    this.auth.getAllSprints().subscribe({
      next: (res: any) => {
        this.sprints = res;

        if (this.sprints.length > 0) {
          this.selectedSprintId = this.sprints[0].id;
        }
      },
      error: (err) => console.error('Failed to load sprints', err)
    });

    // Load users
    this.auth.getUsers().subscribe({
      next: (res: any) => this.users = res,
      error: (err) => console.error('Failed to load users', err)
    });
  }

  // ================= CREATE TASK =================
  createTask() {

    // ✅ VALIDATION FIXED (IMPORTANT)
    if (
      !this.task.title ||
      !this.task.priority ||
      !this.task.status ||
      !this.task.storyName ||
      this.task.storyPoints == null ||
      !this.selectedSprintId ||
      !this.selectedUserId
    ) {
      alert('Please fill all required fields');
      return;
    }

    // Assign relations
    this.task.sprintId = this.selectedSprintId;
    this.task.assigneeId = this.selectedUserId;

    // API call
    this.auth.createTask(this.task).subscribe({
      next: () => {

        alert('Task created successfully!');

        // ✅ reset form (professional UX)
        this.task = {
          title: '',
          description: '',
          status: 'To Do',
          priority: '',
          storyName: '',
          storyPoints: null
        };

        this.selectedUserId = '';

        this.router.navigate(['/board']);
      },

      error: (err) => {
        console.error(err);
        alert('Failed to create task. Please try again.');
      }
    });
  }
}