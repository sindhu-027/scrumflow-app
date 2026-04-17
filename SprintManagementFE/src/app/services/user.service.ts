import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  apiUrl = environment.apiUrl + '/Users';

  constructor(private http: HttpClient) {}

  // ✅ GET ALL USERS
  getUsers(): Observable<any> {
    return this.http.get(this.apiUrl, {
      withCredentials: true
    });
  }

  // ✅ ASSIGN ROLE
  assignRole(id: number, role: string): Observable<any> {
    return this.http.put(
      `${this.apiUrl}/assign-role/${id}`,
      { role: role }, // 🔥 important (send as object)
      {
        withCredentials: true
      }
    );
  }

}