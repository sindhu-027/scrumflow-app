
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // ================= USER =================

  login(email: string, password: string): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/Users/login`,
      { email, passwordHash: password },
      { withCredentials: true }
    );
  }

  register(name: string, email: string, password: string): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/Users`,
      { name, email, passwordHash: password },
      { withCredentials: true }
    );
  }

  getCurrentUser(): Observable<any> {
    return this.http.get(
      `${this.apiUrl}/Users/me`,
      { withCredentials: true }
    );
  }

  logout(): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/Users/logout`,
      {},
      { withCredentials: true }
    );
  }

  getUsers(): Observable<any> {
    return this.http.get(
      `${this.apiUrl}/Users`,
      { withCredentials: true }
    );
  }


  // ================= TASK =================

  getTask(id: string): Observable<any> {
    return this.http.get(
      `${this.apiUrl}/tasks/${id}`,
      { withCredentials: true }
    );
  }

  getAllTasks(): Observable<any> {
    return this.http.get(
      `${this.apiUrl}/tasks`,
      { withCredentials: true }
    );
  }

  createTask(task: any): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/tasks`,
      task,
      { withCredentials: true }
    );
  }

  updateTask(id: string, task: any): Observable<any> {
    return this.http.put(
      `${this.apiUrl}/tasks/${id}`,
      task,
      { withCredentials: true }
    );
  }

  getMyTasks(): Observable<any> {
    return this.http.get(
      `${this.apiUrl}/tasks/my`,
      { withCredentials: true }
    );
  }

  getTasksBySprint(sprintId: number): Observable<any> {
    return this.http.get(
      `${this.apiUrl}/sprints/${sprintId}/tasks`,
      { withCredentials: true }
    );
  }


  // ================= SPRINT =================

  getAllSprints(): Observable<any> {
    return this.http.get(
      `${this.apiUrl}/sprints`,
      { withCredentials: true }
    );
  }

  createSprint(sprint: any): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/sprints`,
      sprint,
      { withCredentials: true }
    );
  }

  updateSprint(id: string, sprint: any): Observable<any> {
    return this.http.put(
      `${this.apiUrl}/sprints/${id}`,
      sprint,
      { withCredentials: true }
    );
  }

  deleteSprint(id: string): Observable<any> {
    return this.http.delete(
      `${this.apiUrl}/sprints/${id}`,
      { withCredentials: true }
    );
  }

  updateUser(id: number, data: any) {
  return this.http.put(`${this.apiUrl}/users/${id}`, data);
}

  // 🔥 START SPRINT (NEW)
  startSprint(id: string): Observable<any> {
    return this.http.put(
      `${this.apiUrl}/sprints/${id}/start`,
      {},
      { withCredentials: true }
    );
  }

  // 🔥 END SPRINT (NEW)
  endSprint(id: string): Observable<any> {
    return this.http.put(
      `${this.apiUrl}/sprints/${id}/end`,
      {},
      { withCredentials: true }
    );
  }

}