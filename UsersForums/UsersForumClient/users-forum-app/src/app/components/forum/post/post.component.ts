import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { ActivatedRoute } from '@angular/router';
import { PostService } from '../../../services/post.service';  // Replace with your actual service
import { UserService } from '../../../services/user.service';  // Replace with your actual auth service
import { User } from '../../../models/user';  // Corrected import path
import { Post } from '../../../models/post';  // Corrected import path
import { Response } from '../../../models/response';  // Corrected import path
import { ResponseService } from '../../../services/response.service';
import { finalize } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { startLoading, stopLoading } from '../../../store/actions/loading.action';
import { environment } from '../../../../environments/environment';
import { Router } from '@angular/router';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {
  private hubConnection!: signalR.HubConnection;
  loggedInUserId: number;

  post: Post = {} as Post;
  responses: any[] = [];
  newResponseContent: string = '';  // Hold the new response content
  showAddResponse: boolean = false;  // Toggle add response form visibility
user: User = {} as User;

  constructor(private postService: PostService, private router: Router
    ,private store: Store<{ loading: boolean }>
    ,private userService: UserService, private responseService: ResponseService,
    private route: ActivatedRoute) {
    this.user = this.userService.getUser()!;

    this.loggedInUserId = this.user.id ?? 0; // Retrieve the logged-in user's ID or default to 0
  }
  ngOnInit() {
    this.startSignalRConnection();
    this.store.dispatch(startLoading());  // Start loading

    const postId = this.route.snapshot.params['id'];
    this.postService.getPostById(postId)
    .pipe(
        finalize(() => {
          this.store.dispatch(stopLoading());  // Start loading
        })
      ).subscribe(post => {
      this.post = post;
    });

    this.responseService.getAllResponses(postId).subscribe(responses => {
      this.responses = responses;
    });
  }



  enableEditPost(post: Post): void {
    post.isEditing = true;  // Enable editing mode
  }

  savePost(post: Post): void {
    this.store.dispatch(startLoading());  // Start loading
    this.postService.updatePost(post)
    .pipe(
      finalize(() => {
        this.store.dispatch(stopLoading());  // Start loading
      })
    )
    .subscribe({
      next: () => {
        post.isEditing = false;  // Exit editing mode after saving
      },
      error: (err) => {
        console.error('Failed to update post', err);
      }
    });
  }

  deletePost(post: Post): void {
    const confirmation = confirm('Are you sure?');
    if (confirmation) {
      this.postService.deletePost(post.id).subscribe({
        next: () => {
          this.router.navigate(['/forum']);
        },
        error: (err) => {
          console.error('Failed to update post', err);
        }

      });
    }
  }

  cancelEdit(post: Post): void {
    post.isEditing = false;  // Exit editing mode without saving
  }

  toggleAddResponse(): void {
    this.showAddResponse = !this.showAddResponse;  // Toggle the response form

  }

    // Handle response updates from ResponseComponent
    onResponseUpdated(updatedResponse: Response): void {
      let index = -1;
      index = this.post.responses?.findIndex(response =>
        response.id === updatedResponse.id)?? index;
      if (index !== -1) {
        if (this.post.responses) {
          this.post.responses[index] = updatedResponse;  // Update the response in the list
        }
        // Optionally, call the API to save the updated response
        this.responseService.updateResponse(updatedResponse).subscribe();
      }
    }
    // Method to handle response submission
    submitResponse(): void {
      const newResponse: Response = {
        id: 0,  // Assuming 0 or a placeholder value, as it will be set by the backend
        responseMessage: this.newResponseContent,
        userId: this.loggedInUserId,
        postId: this.post.id,  // Attach response to the current post
        name: this.user.name ?? 'Anonymous',  // Use the user's name or default to 'Anonymous'

      };
       // Call the response service to add the new response
    this.responseService.addResponse(newResponse).subscribe({
      next: (response: Response) => {
         if (!this.post.responses) {
            this.post.responses = [];
          }
        this.post.responses?.push(response);
         // Add the new response to the post's response list
        this.newResponseContent = '';  // Clear the editor after submission
        this.showAddResponse = false;  // Hide the form after submission
        this.hubConnection.invoke('SendNewResponse', newResponse.postId, newResponse.userId, newResponse.name, newResponse.responseMessage);
      },
      error: (err) => {
        console.error('Error adding response', err);
      }
    });
  }

  private startSignalRConnection(): void {
    const token = this.userService.getToken();
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubUrl, {
        accessTokenFactory: () => token ? token : ''
      }).build();

    this.hubConnection.start().then(() => {
      console.log('SignalR Connected!');
    }).catch(err => console.error('Error while starting SignalR connection: ' + err));



    // Listen for new responses from the server
    this.hubConnection.on('ReceiveNewResponse', (postId, userId, name, responseMessage) => {
      const newResponse: Response = {
        id: 0,
        postId: postId,
        userId: userId,
        name: name,
        responseMessage: responseMessage
        //name: 'New User'  // Handle user information properly in your real scenario
      };
      console.log('New response received: ', newResponse);
      if (!this.post.responses) {
        this.post.responses = [];
      }
      if (this.post.id === newResponse.postId) {
        this.post.responses?.push(newResponse);
      }
      //this.responses.push(newResponse);  // Update the response list
    });
  }

}
