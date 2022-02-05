import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ChatComponent } from './routes/chat/chat.component';
import { ContactComponent } from './routes/contact/contact.component';
import { MatDialogModule } from '@angular/material/dialog';
import { StatsComponent } from './routes/stats/stats.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { LoginComponent } from './routes/login/login.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JWTInterceptor } from './helper/interceptor/jwt.interceptor';
import { ErrorInterceptor } from './helper/interceptor/error.interceptor';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { HomeComponent } from './routes/home/home.component';
import { MatInputModule } from '@angular/material/input';
import { RegistrationComponent } from './routes/registration/registration.component';
import { ElementScrollPercentageDirective } from './helper/directive/element-scroll-percentage.directive';
import { UserInfoComponent } from './routes/user-info/user-info.component';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [
    AppComponent,
    ChatComponent,
    ContactComponent,
    StatsComponent,
    LoginComponent,
    HomeComponent,
    RegistrationComponent,
    ElementScrollPercentageDirective,
    UserInfoComponent
  ],
  imports: [
    BrowserAnimationsModule,
    MatInputModule,
    MatButtonModule,
    HttpClientModule,
    BrowserModule,
    FormsModule,
    MatDialogModule,
    AppRoutingModule,
    MatCardModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatSnackBarModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JWTInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
