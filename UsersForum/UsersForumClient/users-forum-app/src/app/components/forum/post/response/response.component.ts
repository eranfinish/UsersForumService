import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Response } from '../../../../models/response';

@Component({
  selector: 'app-response',
  templateUrl: './response.component.html',
  styleUrls: ['./response.component.css']
})
export class ResponseComponent   {
  private hubConnection!: signalR.HubConnection;
  @Input() response!: Response;  // Input to receive the response from the parent (PostComponent)
  @Input() loggedInUserId?: number;  // Logged-in user's ID

  @Output() responseUpdated = new EventEmitter<Response>();  // Output to emit when the response is updated

  editing: boolean = false;
  newResponseContent: string = '';  // Hold the content during editing
 


  // Toggle edit mode
  enableEdit(): void {
    this.editing = true;
    this.newResponseContent = this.response.responseMessage || '';  // Pre-fill with the existing response or default to an empty string
  }

  // Emit the updated response
  saveResponse(): void {
    this.response.responseMessage = this.newResponseContent;
    this.responseUpdated.emit(this.response);  // Emit the updated response to the parent component
    this.editing = false;  // Close editing mode
  }

  cancelEdit(): void {
    this.editing = false;
  }

}

