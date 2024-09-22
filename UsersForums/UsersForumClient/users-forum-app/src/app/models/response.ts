import { Post } from './post';
import{ User } from './user';

export class Response {
  id: number;
  postId: number;  // Related post
  userId: number;  // User who posted the response
  name: string; // his name
  responseMessage?: string;
isEditing?: boolean;
  // Navigation Properties
  //post: Post;  // Related post
  //user: User;  // Responder user

  constructor(
    id: number,
    postId: number,
    userId: number,
    name:string,
    //post: Post,
   // user: User,
    responseMessage?: string
  ) {
    this.id = id;
    this.postId = postId;
    this.userId = userId;
    this.name = name;
    // this.post = post;
    //this.user = user;
    this.responseMessage = responseMessage;
  }
}
