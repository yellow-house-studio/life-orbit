/* eslint-disable no-console */
import { Injectable } from '@angular/core';
import { AppLogger } from './app-logger.service';

@Injectable({
  providedIn: 'root',
})
export class ConsoleLogger implements AppLogger {
  private context: string | undefined;

  debug(message: string, context?: unknown): void {
    console.info(this.createMessage(message, context));
  }
  
  error(message: string, error?: unknown, context?: unknown): void {
    // Format the error based on its type
    const formattedError = this.formatError(error);
    
    console.error(this.createMessage(message, {
      error: formattedError,
      context: context
    }));
  }

  warning(message: string, context?: unknown): void {
    console.warn(this.createMessage(message, context));
  }

  info(message: string, context?: unknown): void {
    console.info(this.createMessage(message, context));
  }

  trace(message: string, context?: unknown): void {
    console.info(this.createMessage(message, context));
  }

  event(message: string, context?: unknown): void {
    console.info(this.createMessage(message, context));
  }

  forContext(context: unknown): AppLogger {
    const logger = new ConsoleLogger();

    let contextString = JSON.stringify(context).replace(/"/g, '');

    if(this.context !== undefined) {
      contextString = `${this.context} > ${contextString}`;
    }

    logger.context = contextString;

    return logger;
  }

  private createMessage(message: string, context?: unknown): string {
    const currentTime = new Date().toISOString();
    let messageString = `${currentTime}: ${message}`;

    if(this.context !== undefined) {
      messageString = `${currentTime}: ${this.context} > ${message}`;
    }

    if(context !== undefined) {
      messageString = `${messageString} ${JSON.stringify(context)}`;
    }

    return messageString;
  }

  private formatError(error: unknown): unknown {
    if (error instanceof Error) {
      return {
        name: error.name,
        message: error.message,
        stack: error.stack,
      };
    }
    
    if (error === null || error === undefined) {
      return 'No error details provided';
    }

    if (typeof error === 'object') {
      return error;
    }

    return String(error);
  }
}
