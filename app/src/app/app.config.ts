import { ApplicationConfig, inject, provideAppInitializer, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { AppLogger } from './infrastructure/common/logging/app-logger.service';
import { ConsoleLogger } from './infrastructure/common/logging/console-logger';
import { apiUrlInterceptor } from './infrastructure/common/interceptors/api-url.interceptor';
import { AppConfig } from './infrastructure/core/configuration/app-config';
import { initializeApp } from './infrastructure/core/configuration/app-config.factory';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes, withComponentInputBinding()), 
    provideHttpClient(
      withInterceptors([apiUrlInterceptor]),
      withFetch()
    ),
    {
      provide: AppLogger, 
      useClass: ConsoleLogger
    },
    AppConfig,
    provideAppInitializer(() => initializeApp(inject(AppConfig)))
  ]
};
