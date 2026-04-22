import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-sprint-management',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './sprint-management.component.html',
})
export class SprintManagementComponent implements OnInit {

  role: string = '';
  sprints: any[] = [];

  // Form
  newSprintName = '';
  newSprintStart = '';
  newSprintEnd = '';
  newSprintDescription = '';

  // UI States
  loading = false;
  errorMsg = '';
  successMsg = '';

  constructor(
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadSprints();

    // this.auth.getCurrentUser().subscribe({
    //   next: (res: any) => this.role = res.role,
    //   error: () => alert('Session expired. Please login again.')
    // });

    this.auth.getCurrentUser().subscribe({
      next: (res: any) => {
        console.log("USER OK", res);
        this.role = res.role;
      },
      error: (err) => {
        console.error("USER ERROR", err);
        this.role = '';
      }
    });
  }

  // ================= LOAD =================
  // loadSprints() {
  //   this.loading = true;

  //   this.auth.getAllSprints().subscribe({
  //     next: (res: any) => {

  //       this.sprints = (res || []).map((s: any) => ({
  //         ...s,
  //         status: s.status || 'Planned'
  //       }));

  //       this.loading = false;
  //     },
  //     error: () => {
  //       this.errorMsg = 'Failed to load sprints';
  //       this.loading = false;
  //     }
  //   });
  // }

  loadSprints() {
  console.log("🔥 loadSprints called");

  this.loading = true;

  this.auth.getAllSprints().subscribe({
    next: (res: any) => {
      console.log("✅ API success", res);

      this.sprints = (res || []).map((s: any) => ({
        ...s,
        status: s.status || 'Planned'
      }));

      this.loading = false;
    },
    error: (err) => {
      console.error("❌ API error", err);  // 🔥 IMPORTANT
      this.errorMsg = 'Failed to load sprints';
      this.loading = false;
    }
  });
}

  // ================= CREATE =================
  createSprint() {

    if (!this.newSprintName || !this.newSprintStart || !this.newSprintEnd) {
      this.errorMsg = 'All fields are required!';
      return;
    }

    const sprint = {
      name: this.newSprintName,
      startDate: new Date(this.newSprintStart).toISOString(),  // ✅ FIX
      endDate: new Date(this.newSprintEnd).toISOString(),      // ✅ FIX
      description: this.newSprintDescription
    };

    // const sprint = {
    //   name: this.newSprintName,
    //   startDate: this.newSprintStart,
    //   endDate: this.newSprintEnd,
    //   description: this.newSprintDescription,
    //  // status: 'Planned'
    // };

    this.auth.createSprint(sprint).subscribe({
      next: () => {
        this.successMsg = 'Sprint created successfully!';
        this.errorMsg = '';

        this.newSprintName = '';
        this.newSprintStart = '';
        this.newSprintEnd = '';
        this.newSprintDescription = '';

        this.loadSprints();
      },
      error: () => this.errorMsg = 'Failed to create sprint'
    });
  }

  // ================= DELETE =================
  deleteSprint(id: string) {
    this.auth.deleteSprint(id).subscribe({
      next: () => {
        this.successMsg = 'Sprint deleted';
        this.errorMsg = '';
        this.loadSprints();
      },
      error: () => this.errorMsg = 'Delete failed'
    });
  }

  // ================= VIEW (NAVIGATION) =================
  viewSprint(sprint: any) {
    // If later you want sprint-based task page
    this.router.navigate(['/task-details', sprint.id]);
  }

  // ================= START SPRINT =================
  startSprint(sprint: any) {

    if (this.sprints.some(s => s.status === 'Active')) {
      this.errorMsg = 'Another sprint is already active!';
      return;
    }

    const updated = {
      ...sprint,
      status: 'Active'
    };

    this.auth.updateSprint(sprint.id, updated).subscribe({
      next: () => {
        this.successMsg = 'Sprint started 🚀';
        this.errorMsg = '';
        this.loadSprints();
      },
      error: () => this.errorMsg = 'Failed to start sprint'
    });
  }

  // ================= END SPRINT =================
  endSprint(sprint: any) {

    const updated = {
      ...sprint,
      status: 'Completed'
    };

    this.auth.updateSprint(sprint.id, updated).subscribe({
      next: () => {
        this.successMsg = 'Sprint completed ✅';
        this.errorMsg = '';
        this.loadSprints();
      },
      error: () => this.errorMsg = 'Failed to end sprint'
    });
  }

  // ================= HELPERS =================
  isActiveSprint(): boolean {
    return this.sprints.some(s => s.status === 'Active');
  }
}