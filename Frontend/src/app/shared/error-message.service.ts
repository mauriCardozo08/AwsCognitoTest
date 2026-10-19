import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ErrorMessageService {
  toMessage(error: unknown): string {
    if (!(error instanceof HttpErrorResponse)) {
      return 'Ocurrió un error inesperado.';
    }

    // Every API error (including 401/403) is a ProblemDetails. Status 0 means no response at all.
    return error.status === 0
      ? 'No se pudo conectar con la API.'
      : (error.error?.detail ?? error.error?.title ?? 'Ocurrió un error.');
  }
}
