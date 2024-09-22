import { User } from './user';
import {Response} from './response';

export class Post {
  id: number;
  userId: number;
  name: string;
  title?: string;
  content?: string;
  responsersIDs?: number[];
  isEditing?: boolean = false;

  // Navigation Properties
  user?: User;  // Post creator
  responses?: Response[];  // List of responses

  constructor(
    id: number,
    userId: number,
    name: string,
    user: User,
    title?: string,
    content?: string,
    responsersIDs?: number[],
    responses?: Response[]
  ) {
    this.id = id;
    this.userId = userId;
    this.name = name;
    this.title = title;
    this.content = content;
    this.responsersIDs = responsersIDs;
    this.user = user;
    this.responses = responses;
  }
}
