/* eslint-disable @typescript-eslint/no-explicit-any */
import { ErrorHandler, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AngularPlugin } from '@microsoft/applicationinsights-angularplugin-js';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { AppLogger } from './app-logger.service';
import { AppConfiguration } from '../../core/configuration/app-configuration';

@Injectable({
  providedIn: 'root',
})
export class InsightsService implements AppLogger {
  private angularPlugin = new AngularPlugin();
  private appInsights = new ApplicationInsights({
    config: {
      instrumentationKey: this.appConfig.appInsights.instrumentationKey,
      extensions: [this.angularPlugin],
      extensionConfig: {
        [this.angularPlugin.identifier]: {
          router: this.router,
          errorServices: [new ErrorHandler()],
        },
      },
    },
  });

  private internalContext: object = {};


  constructor(
    private router: Router,
    private appConfig: AppConfiguration,
  ) {}

  forContext(context: any): AppLogger {
    const logger = new InsightsService(this.router, this.appConfig);
    logger.internalContext = { ...this.internalContext, ...context };

    return logger;
  }

  init() {
    this.appInsights.loadAppInsights();
  }

  debug(message: string): void {
    if (this.appConfig.logging.logLevel === 'debug') {
      this.appInsights.trackTrace({ message: `Debug: ${message}` }, { ...this.internalContext });
    }
  }

  error(message: string, error: Error, context?: any): void {
    this.appInsights.trackException({
      exception: error,
      properties: { ...context, message, ...this.internalContext },
    });
  }
  warning(message: string, context?: any): void {
    this.appInsights.trackTrace({ message, properties: {...this.internalContext, ...context} });
  }
  info(message: string, context?: any): void {
    this.appInsights.trackTrace({ message, properties: {...this.internalContext, ...context} });
  }
  trace(message: string, context?: any): void {
    this.appInsights.trackTrace({ message, properties: {...this.internalContext, ...context} });
  }
  event(event: string, properties?: any): void {
    this.appInsights.trackEvent({ name: event, properties: {...this.internalContext, ...properties} });
  }

  setAuthenticatedUserId(userId: string): void {
    this.appInsights.setAuthenticatedUserContext(userId);
  }

  clearAuthenticatedUserId(): void {
    this.setAuthenticatedUserId('');
    this.appInsights.clearAuthenticatedUserContext();
  }
}
