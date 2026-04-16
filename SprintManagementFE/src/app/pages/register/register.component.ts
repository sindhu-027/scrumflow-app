import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterModule],
  templateUrl: './register.component.html',
})
export class RegisterComponent {

  logo: string = 'assets/ScrumLogo.png';

  name: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';

  constructor(private auth: AuthService, private router: Router) {}

  register() {

    // ✅ Password validation
    if (!this.name || !this.email || !this.password || !this.confirmPassword) {
      alert('All fields are required!');
      return;
    }

    if (this.password !== this.confirmPassword) {
      alert('Passwords do not match!');
      return;
    }

    // ✅ Call API with cookies enabled
    this.auth.register(this.name, this.email, this.password).subscribe({
      next: () => {
        alert('Registration successful! Please login.');
        this.router.navigate(['/login']);
      },
      error: (err: any) => {
        alert('Registration failed!');
        console.error(err);
      },
    });
  }
}





