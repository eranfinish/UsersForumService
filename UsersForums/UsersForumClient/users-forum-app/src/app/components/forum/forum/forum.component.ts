// forum.component.ts
import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { PostService } from '../../../services/post.service';
import { UserService } from '../../../services/user.service';
import { finalize } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { startLoading, stopLoading } from '../../../store/actions/loading.action';
import { environment } from '../../../../environments/environment';
import { Post } from '../../../models/post';

@Component({
  selector: 'app-forum',
  templateUrl: './forum.component.html',
  styleUrls: ['./forum.component.css']
})
export class ForumComponent implements OnInit {
  private hubConnection!: signalR.HubConnection;
  posts: any[] = [];

  constructor(private postService: PostService, private userService: UserService,
    private store: Store<{ loading: boolean }>
  ) { }

  ngOnInit() {
    this.startSignalRConnection();
    this.store.dispatch(startLoading());  // Start loading
    this.postService.getAllPosts().pipe(
      finalize(() => {
        this.store.dispatch(stopLoading());  // Stop loading
      })
    ).subscribe(posts => {
      this.posts = posts;
    });
  }

  private startSignalRConnection(): void {
    console.log("hubUrl", environment.hubUrl);
    const token = this.userService.getToken();
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubUrl, {
        accessTokenFactory: () => token ? token : ''
      })  // Replace with your backend SignalR URL
      .build();

    this.hubConnection.start().then(() => {
      console.log('SignalR Connected!');
    }).catch(err => console.error('Error while starting SignalR connection: ' + err));

    // Listen for new posts from the server
    this.hubConnection.on('ReceiveNewPost', (postId, userId, title, content, name) => {
      const newPost: Post = {
        id: postId,
        userId: userId,
        title: title,
        content: content,
        name: name// Assuming user has a userName property
      };
      this.posts.push(newPost);
      console.log('New post received: ', newPost);
      // Handle new post (e.g., update post list or refresh view)
    });


  }

}
