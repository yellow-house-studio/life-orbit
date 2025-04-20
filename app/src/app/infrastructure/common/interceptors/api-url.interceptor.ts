import { HttpInterceptorFn } from '@angular/common/http';
import { AppConfig } from '../../core/configuration/app-config';

export const apiUrlInterceptor: HttpInterceptorFn = (request, next) => {
  // Skip if requesting the config file itself or any assets
  if (request.url.startsWith('/assets/')) {
    return next(request);
  }

  // Get the API URL from AppConfig
  const apiUrl = AppConfig.settings.apiUrl;

  // Check if the request URL already starts with http or https
  if (request.url.match(/^https?:\/\//)) {
    return next(request);
  }

  // Clone the request and update the URL
  const apiRequest = request.clone({
    url: `${apiUrl}${request.url}`
  });

  return next(apiRequest);
}; 