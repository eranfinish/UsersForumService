import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-global-loading',
  templateUrl: './global-loading.component.html',
  styleUrls: ['./global-loading.component.css']
})

export class GlobalLoadingComponent {
  loading$: Observable<boolean>;

  constructor(private store: Store<{ loading: boolean }>) {
    this.loading$ = this.store.select('loading');  // Select the loading state from the store
  }
}
