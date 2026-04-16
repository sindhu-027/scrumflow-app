
import { Routes } from '@angular/router';

import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { BoardComponent } from './pages/board/board.component';
import { ManageUsersComponent } from './pages/manage-users/manage-users.component';
import { CreateTaskComponent } from './pages/create-task/create-task.component';
import { MyTasksComponent } from './pages/my-tasks/my-tasks.component';
import { SprintManagementComponent } from './pages/sprint-management/sprint-management.component';
import { EditTaskComponent } from './pages/edit-task/edit-task.component';
import { BacklogComponent } from './pages/backlog/backlog.component';
import { TaskDetailsComponent } from './pages/task-details/task-details.component';

export const routes: Routes = [

  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: 'dashboard', component: DashboardComponent },
  { path: 'board', component: BoardComponent },
  { path: 'backlog', component: BacklogComponent },

  { path: 'create-task', component: CreateTaskComponent },
  { path: 'edit-task/:id', component: EditTaskComponent },   // ✅ FIXED

  { path: 'my-tasks', component: MyTasksComponent },

  { path: 'manage-users', component: ManageUsersComponent }, // ✅ FIXED typo

  { path: 'sprint-management', component: SprintManagementComponent },

  { path: 'task-details/:id', component: TaskDetailsComponent }, // ✅ keep only this

];