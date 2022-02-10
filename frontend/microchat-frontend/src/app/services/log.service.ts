import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { LogLevel } from 'src/model/logLevel';

@Injectable({
  providedIn: 'root'
})
export class LogService {

  readonly maxLogInProduction = LogLevel.All;
  private isProduction = false;
  private errorTimestamp: number = 0;

  constructor(
    private snackBar: MatSnackBar,
    private router: Router
  ) {
    if (environment.production) {
      this.isProduction = true;
    }
  }

  // tslint:disable-next-line: no-any
  log(message: string, severity: LogLevel, withDate: boolean = false, ...data: any[]): void {
    if (this.isProduction && severity > this.maxLogInProduction) {
      // skip this log in production
      return;
    }
    let value = '';
    if (withDate) {
      value = `${new Date()} - `;
    }
    value = value.concat(`[${severity.toString()}]: ${message}`);
    if (data) {
      console.log(value, ...data);
    } else {
      console.log(value);
    }
  }

  messageSnackBar(message: string, duration: number = 3000): void {
    if (this.ableToInfoOrMessage()) {
      this.formatStringAndOpen(message, undefined, { duration, panelClass: 'snackBarMessage' });
    }
  }

  infoSnackBar(message: string, duration: number = 10000): void {
    if (this.ableToInfoOrMessage()) {
      this.formatStringAndOpen(message, 'OK', { duration, panelClass: 'snackBarInfo' });
    }
  }

  private formatStringAndOpen(message: string, action?: string, config?: MatSnackBarConfig): void {
    message = message.charAt(0).toUpperCase() + message.slice(1);
    this.snackBar.open(message, action, config);
  }

  private ableToInfoOrMessage(): boolean {
    return this.errorTimestamp < Date.now();
  }

  errorSnackBar(error: string | Error, duration: number = 10000): void {
    this.errorTimestamp = Date.now() + duration; 
    try {
      if (typeof error === "string") {
        this.formatStringAndOpen(error, undefined, { duration, panelClass: 'snackBarError' });
      } else {
        this.formatStringAndOpen(error.message, undefined, { duration, panelClass: 'snackBarError' });
      }
    } catch(ex) {
      this.formatStringAndOpen("unknown error", undefined, { duration, panelClass: 'snackBarError' });
    }
  }

  recommendALink(message: string, url: string, duration: number = 6000): void {
    message = message.charAt(0).toUpperCase() + message.slice(1);
    this.snackBar.open(message, 'CLick Here', { duration, panelClass: 'snackBarInfo' })
      .onAction()
      .pipe(take(1))
      .subscribe(() => {
        this.router.navigate([url]);
      });
  }
}
