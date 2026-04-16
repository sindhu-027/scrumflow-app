
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-backlog',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './backlog.component.html',
})
export class BacklogComponent implements OnInit {

  role: string = '';
  tasks: any[] = [];
  logo: string = 'assets/ScrumLogo.png';

  loading = false;
  errorMsg = '';

  constructor(
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadUser();
  }

  // ================= USER =================
  loadUser() {
    this.auth.getCurrentUser().subscribe({
      next: (res: any) => {
        this.role = res.role;
        this.loadTasks();
      },
      error: () => {
        this.errorMsg = 'Session expired. Please login again';
      }
    });
  }

  // ================= BACKLOG TASKS =================
  loadTasks() {
    this.loading = true;

    this.auth.getAllTasks().subscribe({
      next: (res: any) => {

        // Backlog = only "To Do"
        this.tasks = (res || []).filter(
          (t: any) => t.status === 'To Do'
        );

        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load tasks', err);
        this.errorMsg = 'Failed to load backlog tasks';
        this.loading = false;
      }
    });
  }

  // ================= ACTIONS =================

  viewTask(task: any) {
    this.router.navigate(['/task-details', task.id]);
  }

  editTask(task: any) {
    this.router.navigate(['/edit-task', task.id]);
  }

  // optional (if you add delete later)
  deleteTask(taskId: string) {
    //this.auth.deleteTask(taskId).subscribe(() => this.loadTasks());
  }
}