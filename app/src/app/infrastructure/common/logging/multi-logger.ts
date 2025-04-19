import { AppLogger } from './app-logger.service';


export class MultiLogger implements AppLogger {

  constructor(private loggers: AppLogger[]){}

  debug(message: string): void {
    this.loggers.forEach(logger => logger.debug(message));
  }
  error(message: string, error: Error): void {
    this.loggers.forEach(logger => logger.error(message, error));
  }
  warning(message: string): void {
    this.loggers.forEach(logger => logger.warning(message));
  }
  info(message: string): void {
    this.loggers.forEach(logger => logger.info(message));
  }
  trace(message: string): void {
    this.loggers.forEach(logger => logger.trace(message));
  }
  event(message: string): void {
    this.loggers.forEach(logger => logger.event(message));
  }

  forContext(context: unknown): AppLogger {
    const loggers = this.loggers.map(logger => logger.forContext(context));
    return new MultiLogger(loggers);
  }
}
