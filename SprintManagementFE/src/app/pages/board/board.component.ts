
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import {
  DragDropModule,
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem
} from '@angular/cdk/drag-drop';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-board',
  standalone: true,
  imports: [CommonModule, RouterModule, DragDropModule, FormsModule],
  templateUrl: './board.component.html',
})
export class BoardComponent implements OnInit {

  role: string = '';

  sprints: any[] = [];
  selectedSprintId: number = 0;

  // ✅ Priority filter
  selectedPriority: string = 'All';

  // ✅ Columns
  todo: any[] = [];
  inProgress: any[] = [];
  done: any[] = [];

  isLoading = false;

  constructor(private auth: AuthService) {}

  ngOnInit(): void {
    this.loadUser();
  }

  // ================= USER =================
  loadUser() {
    this.auth.getCurrentUser().subscribe({
      next: (res: any) => {
        this.role = res.role;
        this.loadSprints();
      },
      error: () => alert('Session expired. Please login again.')
    });
  }

  // ================= SPRINT =================
  loadSprints() {
    this.auth.getAllSprints().subscribe({
      next: (res: any[]) => {
        this.sprints = res || [];

        if (this.sprints.length > 0) {
          this.selectedSprintId = Number(this.sprints[0].id);
          this.loadTasks(this.selectedSprintId);
        }
      },
      error: (err) => console.error('Failed to load sprints', err)
    });
  }

  // ================= FILTER TRIGGER =================
  applyFilter() {
    if (this.selectedSprintId) {
      this.loadTasks(this.selectedSprintId);
    }
  }

  // ================= TASK LOAD =================
  loadTasks(sprintId: number) {

    if (!sprintId) return;

    this.isLoading = true;
    this.selectedSprintId = sprintId;

    this.auth.getTasksBySprint(sprintId).subscribe({
      next: (res: any[]) => {

        console.log("Tasks:", res);

        // ✅ APPLY PRIORITY FILTER
        const filtered = this.selectedPriority === 'All'
          ? res
          : res.filter(t => t.priority?.trim() === this.selectedPriority);

        // ✅ RESET COLUMNS
        this.todo = [];
        this.inProgress = [];
        this.done = [];

        // ✅ MAP STATUS (IMPORTANT: backend uses "Completed")
        this.todo = filtered.filter(t => t.status?.trim() === 'To Do');
        this.inProgress = filtered.filter(t => t.status?.trim() === 'In Progress');
        this.done = filtered.filter(t => t.status?.trim() === 'Completed');

        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load tasks', err);
        this.isLoading = false;
      }
    });
  }

  // ================= DRAG & DROP =================
  drop(event: CdkDragDrop<any[]>, targetStatus: string) {

    // ❌ only Developer can drag
    if (this.role !== 'Developer') return;

    // 🔁 reorder in same column
    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
      return;
    }

    // 🔁 move between columns
    transferArrayItem(
      event.previousContainer.data,
      event.container.data,
      event.previousIndex,
      event.currentIndex
    );

    const task = event.container.data[event.currentIndex];

    // ✅ SAFE STATUS MAP (aligned with backend)
    const statusMap: any = {
      'To Do': 'To Do',
      'In Progress': 'In Progress',
      'Done': 'Completed'
    };

    const updatedTask = {
      ...task,
      status: statusMap[targetStatus],
      sprintId: this.selectedSprintId
    };

    // ✅ API UPDATE
    this.auth.updateTask(task.id, updatedTask).subscribe({
      next: () => {
        console.log(`✅ Task "${task.title}" → ${statusMap[targetStatus]}`);
      },
      error: (err) => {
        console.error('❌ Failed to update task', err);

        // 🔄 rollback
        this.loadTasks(this.selectedSprintId);
      }
    });
  }
}