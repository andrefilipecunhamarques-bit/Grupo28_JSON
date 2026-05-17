import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment';

interface AppConfig {
  apiUrl?: string;
}

@Injectable({ providedIn: 'root' })
export class AppConfigService {
  private config: AppConfig = {};

  constructor(private http: HttpClient) {}

  async load(): Promise<void> {
    try {
      this.config = await firstValueFrom(this.http.get<AppConfig>('assets/app-config.json'));
    } catch {
      this.config = {};
    }
  }

  get apiUrl(): string {
    return this.config.apiUrl ?? environment.apiUrl;
  }
}
