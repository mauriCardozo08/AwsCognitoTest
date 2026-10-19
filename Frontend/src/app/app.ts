import { Component, inject, signal } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Api } from './api/api';
import { apiSamplesPost$Json } from './api/fn/samples/api-samples-post-json';
import { ErrorMessageService } from './shared/error-message.service';

@Component({
  imports: [],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  private readonly oidc = inject(OidcSecurityService);
  private readonly api = inject(Api);
  private readonly errorMessages = inject(ErrorMessageService);

  protected readonly isAuthenticated = this.oidc.authenticated;
  protected readonly isLoading = signal(true);

  protected readonly exampleName = signal('');
  protected readonly isSaving = signal(false);
  protected readonly errorMessage = signal<string | null>(null);
  protected readonly successMessage = signal<string | null>(null);

  constructor() {
    // Processes the ?code=... in the URL when Cognito redirects back, or restores an existing session.
    this.oidc.checkAuth().subscribe(() => this.isLoading.set(false));
  }

  protected login(): void {
    this.oidc.authorize();
  }

  protected async addExample(): Promise<void> {
    const name = this.exampleName().trim();
    if (!name) {
      return;
    }

    this.isSaving.set(true);
    this.errorMessage.set(null);
    this.successMessage.set(null);

    try {
      const created = await this.api.invoke(apiSamplesPost$Json, { body: { name } });
      this.successMessage.set(`Example "${created.name}" agregado.`);
      this.exampleName.set('');
    } catch (error) {
      this.errorMessage.set(this.errorMessages.toMessage(error));
    } finally {
      this.isSaving.set(false);
    }
  }
}
