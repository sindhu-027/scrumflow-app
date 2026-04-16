
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-task',
  standalone: true,
  imports: [FormsModule, RouterModule, CommonModule],
  templateUrl: './edit-task.component.html'
})
export class EditTaskComponent implements OnInit {

  logo: string = 'assets/ScrumLogo.png';
  role: string = '';
  taskId: string = '';

  // ✅ Task object with Story fields
  task: any = {
    title: '',
    description: '',
    priority: 'Medium',
    status: 'To Do',
    storyName: '',
    storyPoints: null
  };

  constructor(
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    // ✅ Get current user role
    this.auth.getCurrentUser().subscribe({
      next: (res: any) => this.role = res.role,
      error: () => {
        alert('Session expired. Please login again');
        this.router.navigate(['/login']);
      }
    });

    // ✅ Get task by ID
    this.taskId = this.route.snapshot.params['id'];
    this.auth.getTask(this.taskId).subscribe({
      next: (res: any) => {
        // Ensure Story fields exist
        this.task = {
          ...res,
          storyName: res.storyName || '',
          storyPoints: res.storyPoints || null
        };
      },
      error: () => {
        alert('Failed to load task');
        this.router.navigate(['/backlog']);
      }
    });
  }

  updateTask() {
    // ✅ Validation including story fields
    if (!this.task.title || !this.task.description || !this.task.storyName || !this.task.storyPoints) {
      alert('Please fill all required fields including Story Name and Story Points');
      return;
    }

    this.auth.updateTask(this.taskId, this.task).subscribe({
      next: () => {
        alert('Task updated successfully!');
        this.router.navigate(['/backlog']); // professional workflow
      },
      error: () => {
        alert('Failed to update task');
      }
    });
  }
}