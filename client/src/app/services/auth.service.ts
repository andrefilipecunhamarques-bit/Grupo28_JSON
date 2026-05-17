import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { AppConfigService } from './app-config.service';

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  success: boolean;
  message: string;
  code: string;
}

export interface RegisterRequest {
  username: string;
  password: string;
  email: string;
}

export interface RegisterResponse {
  success: boolean;
  message: string;
  code: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private _loggedInUsername: string | null = null;

  constructor(private http: HttpClient, private config: AppConfigService) {}

  private get baseUrl(): string {
    return `${this.config.apiUrl}/auth`;
  }

  get loggedInUsername(): string | null {
    return this._loggedInUsername;
  }

  get isLoggedIn(): boolean {
    return this._loggedInUsername !== null;
  }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/login`, request).pipe(
      tap(res => {
        if (res.success) {
          this._loggedInUsername = request.username;
        }
      })
    );
  }

  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.baseUrl}/register`, request);
  }

  logout(): void {
    this._loggedInUsername = null;
  }
}

