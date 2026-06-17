import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { PrivateComponent } from './private/private.component';
import {ReactiveFormsModule} from "@angular/forms";

@NgModule({ declarations: [
        AppComponent,
        RegisterComponent,
        LoginComponent,
        NotFoundComponent,
        PrivateComponent
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        AppRoutingModule, ReactiveFormsModule], providers: [provideHttpClient(withInterceptorsFromDi())] })
export class AppModule { }
