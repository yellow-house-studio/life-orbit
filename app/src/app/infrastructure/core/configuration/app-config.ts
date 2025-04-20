import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

export interface IAppConfig {
  apiUrl: string;
  production: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AppConfig {
  private static config: IAppConfig = {
    apiUrl: 'http://localhost:5050', // Default value while loading
    production: false
  };
  private static isLoaded = false;

  constructor(private http: HttpClient) {}

  static get settings(): IAppConfig {
    return AppConfig.config;
  }

  static get loaded(): boolean {
    return AppConfig.isLoaded;
  }

  async load(): Promise<void> {
    try {
      // Load the configuration file
      const loadedConfig = await firstValueFrom(
        this.http.get<IAppConfig>('/assets/config/app-config.json')
      );
      
      AppConfig.config = loadedConfig;
      AppConfig.isLoaded = true;
    } catch (error) {
      console.error('Error loading app config:', error);
      // Keep using default values if loading fails
      AppConfig.isLoaded = true;
    }
  }
} 