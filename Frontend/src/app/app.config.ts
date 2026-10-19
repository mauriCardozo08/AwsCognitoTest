import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { authInterceptor, provideAuth } from 'angular-auth-oidc-client';
import { provideApiConfiguration } from './api/api-configuration';
import { authConfig } from './auth/auth.config';
import { apiBaseUrl } from './config';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptors([authInterceptor()])),
    provideAuth(authConfig),
    provideApiConfiguration(apiBaseUrl),
  ],
};
