
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterModule],
  templateUrl: './login.component.html',
})
export class LoginComponent {

  logo: string = 'assets/ScrumLogo.png';

  email: string = '';
  password: string = '';

  constructor(private auth: AuthService, private router: Router) {}

  login() {
    this.auth.login(this.email, this.password).subscribe({
      next: (res: any) => {

        // ❌ REMOVE token storage (cookie handles it)
        // localStorage.setItem('token', res.token);

        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        alert('Login failed!');
        console.error(err);
      }
    });
  }
}