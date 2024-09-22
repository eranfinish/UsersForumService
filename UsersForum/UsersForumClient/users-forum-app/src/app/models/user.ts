import { Post } from './post';

export class User {
  id: number;
  name?: string;
  email: string;
  password: string;
  userName: string;
  token?: string;
  isLogedIn: boolean;
  lastEntrance: Date;
  mobile?: string;

  // Navigation Properties
  posts: Post[];  // Posts created by user
  responses: Response[];  // Responses by user

  constructor(
    id: number,
    email: string,
    password: string,
    userName: string,
    isLogedIn: boolean,
    lastEntrance: Date,
    posts: Post[] = [],
    responses: Response[] = [],
    name?: string,
    token?: string,
    mobile?: string
  ) {
    this.id = id;
    this.email = email;
    this.password = password;
    this.userName = userName;
    this.isLogedIn = isLogedIn;
    this.lastEntrance = lastEntrance;
    this.posts = posts;
    this.responses = responses;
    this.name = name;
    this.token = token;
    this.mobile = mobile;
  }
}

// Assuming Post and Response are already defined elsewhere in your project.
