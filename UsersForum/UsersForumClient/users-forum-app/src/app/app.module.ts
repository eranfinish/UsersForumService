// app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { LoginComponent }from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { ForumComponent } from './components/forum/forum/forum.component';
import { PostComponent } from './components/forum/post/post.component';
import { PostService } from './services/post.service';
import { UserService } from './services/user.service';
import { ResponseService } from './services/response.service';
import { AuthInterceptor } from '../app/interceptors/auth.interceptor';
import { HeaderComponent } from './components/user/header/header.component';
import { CreatePostComponent } from './components/forum/post/create-post/create-post.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { EditorModule } from '@tinymce/tinymce-angular';
import { ResponseComponent } from './components/forum/post/response/response.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { loadingReducer } from './store/redecers/loading.reducer';



@NgModule({
  declarations: [
    AppComponent,
    ForumComponent,
    LoginComponent,
    RegisterComponent,
    PostComponent,
    HeaderComponent,
    CreatePostComponent,
    ResponseComponent
  ],
  imports: [
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatButtonModule,
    EditorModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    StoreModule.forRoot({}, {}),
    EffectsModule.forRoot([]),
    StoreModule.forRoot({loading:loadingReducer}, {}),
    MatProgressSpinnerModule
  ],
  providers: [PostService, UserService, ResponseService,
     { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
    , ],
  bootstrap: [AppComponent]
})
export class AppModule { }
