/* eslint-disable @typescript-eslint/no-explicit-any */


export abstract class AppLogger {
  abstract error(message: string, error?: any, context?: any): void;
  abstract warning(message: string, context?: any): void;
  abstract info(message: string, context?: any): void;
  abstract trace(message: string, context?: any): void;
  abstract event(event: string, properties?: any): void;
  abstract debug(message: string): void;
  abstract debug(message: string, properties?: any): void;

  abstract forContext(context: any): AppLogger;
}
