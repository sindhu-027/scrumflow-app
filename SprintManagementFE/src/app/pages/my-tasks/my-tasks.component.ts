
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { DragDropModule, CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-my-tasks',
  standalone: true,
  imports: [CommonModule, RouterModule, DragDropModule],
  templateUrl: './my-tasks.component.html',
})
export class MyTasksComponent implements OnInit {

  logo = 'assets/ScrumLogo.png';

  tasks: any[] = [];

  todoTasks: any[] = [];
  inProgressTasks: any[] = [];
  completedTasks: any[] = [];

  role: string = '';
  isLoading: boolean = false;

  constructor(private auth: AuthService, private router: Router) {}

  // ================= INIT =================
  ngOnInit(): void {
    this.auth.getCurrentUser().subscribe({
      next: (res: any) => {
        this.role = res.role;
        this.loadMyTasks();
      },
      error: () => alert('Session expired. Please login again.')
    });
  }

  // ================= LOAD TASKS =================
  loadMyTasks() {
    this.isLoading = true;

    this.auth.getMyTasks().subscribe({
      next: (res: any[]) => {
        this.tasks = res || [];

        this.todoTasks = this.tasks.filter(t => t.status === 'To Do');
        this.inProgressTasks = this.tasks.filter(t => t.status === 'In Progress');
        this.completedTasks = this.tasks.filter(t => t.status === 'Completed');

        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  // ================= DRAG & DROP =================
  drop(event: CdkDragDrop<any[]>, status: string) {

    // SAME COLUMN (reorder)
    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }

    // DIFFERENT COLUMN (status change)
    else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );

      const task = event.container.data[event.currentIndex];

      const updatedTask = {
        title: task.title,
        description: task.description,
        priority: task.priority,
        storyName: task.storyName || '',
        storyPoints: task.storyPoints || 0,
        sprintId: task.sprintId,
        assigneeId: task.assigneeId,
        status: status
      };

      this.auth.updateTask(task.id, updatedTask).subscribe({
        next: () => {
          console.log('Task updated');
          this.loadMyTasks(); // ✅ refresh
        },
        error: (err) => {
          console.error('Update failed', err);
          this.loadMyTasks(); // rollback
        }
      });
    }
  }

  // ================= NAVIGATION =================
  viewTask(task: any) {
    this.router.navigate(['/task-details', task.id]);
  }

  // ================= HELPERS =================
  getSprintName(task: any) {
    return task.sprint?.name || 'No Sprint';
  }

  getDueDate(task: any) {
    return task.sprint?.endDate || null;
  }

  // ================= LOGOUT =================
  logout() {
    this.auth.logout().subscribe(() => {
      window.location.href = '/login';
    });
  }
}