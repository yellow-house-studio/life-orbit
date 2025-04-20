import { AppConfig } from './app-config';

export function initializeApp(appConfig: AppConfig): Promise<void> {
  return appConfig.load();
} 