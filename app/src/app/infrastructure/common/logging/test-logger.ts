/* eslint-disable @typescript-eslint/no-explicit-any */
import { AppLogger } from './app-logger.service';

export class TestLogger implements AppLogger {
  constructor(private shouldPrint: boolean = false) {
  }

  error(msg: string, error?: any, context?: any) {
    this.print(msg, { error, context });
  }

  warning(msg: string, context?: any) {
    this.print(msg, context);
  }

  info(msg: string, context?: any) {
    this.print(msg, context);
  }

  trace(msg: string, context?: any) {
    this.print(msg, context);
  }

  event(msg: string, context?: any) {
    this.print(msg, context);
  }

  debug(msg: string, context?: any) {
    this.print(msg, context);
  }

  forContext(): TestLogger {
    return this;
  }

  private print(msg: string, context?: any) {
    if (context !== undefined) {
      console.log(`Debug: ${msg}`, context);
    } else {
      console.log(`Debug: ${msg}`);
    }
  }
}

export const provideTestLogger = (shouldPrint: boolean = false) => {
  return {
    provide: AppLogger,
    useValue: new TestLogger(shouldPrint)
  };
}
