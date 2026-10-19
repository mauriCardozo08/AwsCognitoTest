import { PassedInitialConfig } from 'angular-auth-oidc-client';
import { apiBaseUrl } from '../config';

export const authConfig: PassedInitialConfig = {
  config: {
    // The OIDC discovery document is read from {authority}/.well-known/openid-configuration.
    authority: 'https://cognito-idp.us-east-1.amazonaws.com/us-east-1_vvCLVeymY',
    redirectUrl: 'http://localhost:4200/callback',
    clientId: '3kkdbc6731dkv2hi3pnhid5pue',
    scope: 'openid email',
    responseType: 'code',
    silentRenew: true,
    useRefreshToken: true,
    autoUserInfo: false,
    // The access token is only attached to requests to these URLs.
    secureRoutes: [`${apiBaseUrl}/api`],
  },
};
