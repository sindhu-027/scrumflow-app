
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './task-details.component.html',
})
export class TaskDetailsComponent implements OnInit {

  task: any = null;
  isLoading: boolean = true;
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.loadTask();
  }

  // ✅ Separate method (clean architecture)
  loadTask() {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.errorMessage = 'Invalid Task ID';
      this.isLoading = false;
      return;
    }

    this.auth.getTask(id).subscribe({
      next: (res: any) => {
        this.task = res;

        // ✅ SAFE FALLBACKS (important for UI)
        this.task.assignee = this.task.assignee || { name: 'Unassigned' };
        this.task.sprint = this.task.sprint || null;

        this.isLoading = false;
      },
      error: (err: any) => {
        console.error('Failed to load task', err);
        this.errorMessage = 'Failed to load task details';
        this.isLoading = false;
      }
    });
  }
}