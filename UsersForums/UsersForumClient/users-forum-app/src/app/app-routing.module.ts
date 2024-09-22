
import { NgModule } from '@angular/core';
import { LoginComponent }from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { ForumComponent } from './components/forum/forum/forum.component';
import { PostComponent } from './components/forum/post/post.component';
import { RouterModule, Routes } from '@angular/router';
import { CreatePostComponent } from './components/forum/post/create-post/create-post.component';

const routes: Routes = [
  { path: '', redirectTo: '/forum', pathMatch: 'full' },
  { path: 'forum', component: ForumComponent },
  { path: 'post/:id', component: PostComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
{ path: 'create-post', component: CreatePostComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
