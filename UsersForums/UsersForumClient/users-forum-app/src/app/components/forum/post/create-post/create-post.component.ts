
import { Component, AfterViewInit, OnDestroy, OnInit   } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Router } from '@angular/router';
import { PostService } from '../../../../services/post.service';
import { UserService } from 'src/app/services/user.service';
import { finalize } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { startLoading, stopLoading } from '../../../../store/actions/loading.action';
import { environment } from '../../../../../environments/environment';
import { Post } from '../../../../models/post';

declare var tinymce: any;  // Declare tinymce globally
@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})

export class CreatePostComponent implements AfterViewInit ,OnDestroy, OnInit   {
  private hubConnection!: signalR.HubConnection;
  userId =0; // This should be set based on the logged-in user's ID
  title = '';
  content = '';
name = '';
  constructor(private postService: PostService,
    private store: Store<{ loading: boolean }>,
    private userService: UserService, private router: Router) {
          this.userId = userService.getUser()?.id ?? 0;
          this.name = userService.getUser()?.name ?? '';
  }

  ngOnInit() {
    this.startSignalRConnection();
  }
  // Start SignalR connection and listen for real-time updates
  private startSignalRConnection(): void {
    const token = this.userService.getToken();
  this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.hubUrl, {
      accessTokenFactory: () => token ? token : ''
    })  // Replace with your backend SignalR URL.
    .build();

  this.hubConnection.start().then(() => {
    console.log('SignalR Connected!');
  }).catch(err => console.error('Error while starting SignalR connection: ' + err));

  // Listen for new posts from the server
  this.hubConnection.on('ReceiveNewPost', (postId, userId, title, content, name) => {
    const newPost: Post = {
      id: postId,
    userId:userId,
      title: title,
      content: content,
      name: name// Assuming user has a userName property
    };
    console.log('New post received: ', newPost);
    // Handle new post (e.g., update post list or refresh view)
  });


}

  ngAfterViewInit() {
    tinymce.init({
      selector: '#tinymce-editor',
      plugins: [
        'anchor', 'autolink', 'charmap', 'codesample', 'emoticons', 'image', 'link', 'lists', 'media', 'searchreplace', 'table', 'visualblocks', 'wordcount',
        'checklist', 'mediaembed', 'casechange', 'export', 'formatpainter', 'pageembed', 'a11ychecker', 'tinymcespellchecker', 'permanentpen', 'powerpaste',
        'advtable', 'advcode', 'editimage', 'advtemplate', 'ai', 'mentions', 'tinycomments', 'tableofcontents', 'footnotes', 'mergetags', 'autocorrect', 'typography',
      ],
      toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
      tinycomments_mode: 'embedded',
      tinycomments_author: 'Author name',
      mergetags_list: [
        { value: 'First.Name', title: 'First Name' },
        { value: 'Email', title: 'Email' },
      ],
      setup: (editor: any) => {
        editor.on('change', () => {
          this.content = editor.getContent(); // Capture content from TinyMCE editor
        });
      }
    });
  }
  ngOnDestroy() {
    if (tinymce) {
      tinymce.remove('#tinymce-editor');
    }
  }
  addPost() {
    const postData = {
      id: 0, // ID is usually assigned by the backend
      userId: this.userId,
name: this.name,
      title: this.title,
      content: this.content,
      responsersIDs: [],
      user: null, // Additional user data should be populated server-side or fetched from user service
      responses: []
    };

    this.postService.addNewPost(postData).subscribe({
      next: (result) => {
        // Broadcast the new post to all clients via SignalR
        this.hubConnection.invoke('SendNewPost', postData.id, postData.userId, postData.title, postData.content, postData.name);

        console.log('Post added successfully', result);
        this.router.navigate(['/forum']); // Navigate back to the forum list
      },
      error: (error) => {
        console.error('Failed to add post', error);
      }
    });


  }

}
