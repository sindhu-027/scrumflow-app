
import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit, AfterViewInit {

  logo: string = 'assets/ScrumLogo.png';

  role: string = '';
  userId: number = 0;
  user: any = {};

  // GLOBAL
  myTasks = 0;
  inProgress = 0;
  completed = 0;

  backlog = 0;
  highPriority = 0;

  // SPRINT
  sprintTotal = 0;
  sprintCompleted = 0;

  activeSprint: string = 'No Active Sprint';
  activeSprintId: any = '';

  allSprints: any[] = [];
  selectedSprintId: any = '';

  // CHART
  statusChart: any;
  priorityChart: any;
  burndownChart: any;

  viewReady = false;

  constructor(private auth: AuthService) {}

  ngOnInit(): void {
    this.loadUser();
  }

  ngAfterViewInit(): void {
    this.viewReady = true;
  }

  // ================= USER =================
  loadUser() {
    this.auth.getCurrentUser().subscribe({
      next: (res: any) => {
        this.role = res.role;
        this.userId = res.id;
        this.user = res;
        this.loadSprints();
      },
      error: () => alert('Session expired'),
    });
  }

  // ================= SPRINT =================
  loadSprints() {
    this.auth.getAllSprints().subscribe({
      next: (sprints: any[]) => {
        this.allSprints = sprints;

        const today = new Date();

        const active = sprints.find(s => {
          const start = new Date(s.startDate);
          const end = new Date(s.endDate);
          return today >= start && today <= end;
        });

        if (active) {
          this.activeSprint = active.name;
          this.activeSprintId = active.id;
          this.selectedSprintId = active.id;
        } else {
          this.activeSprint = 'No Active Sprint';
          this.activeSprintId = '';
          this.selectedSprintId = sprints[0]?.id;
        }

        this.loadTasks();
      }
    });
  }

  onSprintChange() {
    this.activeSprintId = this.selectedSprintId;

    const sprint = this.allSprints.find(s => s.id == this.selectedSprintId);
    this.activeSprint = sprint ? sprint.name : 'No Active Sprint';

    this.loadTasks();
  }

  // ================= TASKS =================
  loadTasks() {
    this.auth.getAllTasks().subscribe({
      next: (tasks: any[]) => {

        // DEV
        const myTasksList = tasks.filter(t => t.assigneeId === this.userId);

        this.myTasks = myTasksList.length;
        this.inProgress = myTasksList.filter(t => t.status === 'In Progress').length;
       // this.completed = myTasksList.filter(t => t.status === 'Completed').length;

        // 🔥 FIX → different logic per role
        if (this.role === 'Developer') {
          this.completed = myTasksList.filter(t => t.status === 'Completed').length;
        } else {
          // PO & SM → count ALL completed tasks
          this.completed = tasks.filter(t => t.status === 'Completed').length;
        }

        // GLOBAL
        this.backlog = tasks.length;
        this.highPriority = tasks.filter(t => t.priority === 'High').length;

        // SPRINT
        const sprintTasks = tasks.filter(t => t.sprintId == this.activeSprintId);

        this.sprintTotal = sprintTasks.length;
        this.sprintCompleted = sprintTasks.filter(t => t.status === 'Completed').length;

        const burndownData = this.generateBurndown(tasks);

        setTimeout(() => this.renderCharts(burndownData), 0);
      }
    });
  }

  // ================= BURNDOWN =================
  generateBurndown(tasks: any[]) {

    if (!this.activeSprintId) return { labels: [], data: [] };

    const sprintTasks = tasks.filter(t => t.sprintId == this.activeSprintId);
    if (!sprintTasks.length) return { labels: [], data: [] };

    const total = sprintTasks.length;

    const sprint = this.allSprints.find(s => s.id == this.activeSprintId);
    if (!sprint) return { labels: [], data: [] };

    const start = new Date(sprint.startDate);
    const end = new Date(sprint.endDate);

    let current = new Date(start);

    const labels: string[] = [];
    const remaining: number[] = [];

    while (current <= end) {

      labels.push(current.toLocaleDateString());

      const completed = sprintTasks.filter(t => {
        const d = new Date(t.updatedAt || t.createdAt || new Date());
        return t.status === 'Completed' && d <= current;
      }).length;

      remaining.push(total - completed);

      current.setDate(current.getDate() + 1);
    }

    return { labels, data: remaining };
  }

  // ================= CHART =================
  renderCharts(burndown: any) {

    if (!this.viewReady) return;

    const statusCanvas = document.getElementById('statusChart') as HTMLCanvasElement;
    const priorityCanvas = document.getElementById('priorityChart') as HTMLCanvasElement;
    const burndownCanvas = document.getElementById('burndownChart') as HTMLCanvasElement;

    if (this.statusChart) this.statusChart.destroy();
    if (this.priorityChart) this.priorityChart.destroy();
    if (this.burndownChart) this.burndownChart.destroy();

    // DEV CHART
    if (this.role === 'Developer' && statusCanvas) {
      this.statusChart = new Chart(statusCanvas, {
        type: 'doughnut',
        data: {
          labels: ['In Progress', 'Completed'],
          datasets: [{
            data: [this.inProgress, this.completed],
            backgroundColor: ['#facc15', '#22c55e']
          }]
        },
        options: {
          maintainAspectRatio: false
        }
      });
    }

    // PRIORITY
    if (priorityCanvas) {
      this.priorityChart = new Chart(priorityCanvas, {
        type: 'bar',
        data: {
          labels: ['High', 'Others'],
          datasets: [{
            data: [this.highPriority, this.backlog - this.highPriority],
            backgroundColor: ['#ef4444', '#3b82f6']
          }]
        },
        options: { maintainAspectRatio: false }
      });
    }

    // BURNDOWN
    if ((this.role === 'ScrumMaster' || this.role === 'ProductOwner')
      && burndownCanvas && burndown.labels.length) {

      this.burndownChart = new Chart(burndownCanvas, {
        type: 'line',
        data: {
          labels: burndown.labels,
          datasets: [{
            label: 'Remaining Work',
            data: burndown.data,
            borderColor: '#3b82f6',
            tension: 0.3
          }]
        },
        options: { maintainAspectRatio: false }
      });
    }
  }

  // ================= ACTIONS =================
  startSprint() {
    const next = this.allSprints.find(s => s.id == this.selectedSprintId);

    if (!next) return alert('Select sprint');

    this.auth.startSprint(next.id).subscribe(() => {
      alert('Sprint Started');
      this.loadSprints();
    });
  }

  endSprint() {
    if (this.sprintCompleted === this.sprintTotal) return;

    this.auth.endSprint(this.activeSprintId).subscribe(() => {
      alert('Sprint Ended');
      this.loadSprints();
    });
  }

  openProfile() {}

  logout() {
    this.auth.logout().subscribe(() => {
      window.location.href = '/login';
    });
  }
}