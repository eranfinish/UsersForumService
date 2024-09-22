import { createReducer, on } from '@ngrx/store';
import { startLoading, stopLoading } from '../actions/loading.action';

export const initialState = false;  // Initial loading state is false

const _loadingReducer = createReducer(
  initialState,
  on(startLoading, () => true),  // Set loading to true when startLoading action is dispatched
  on(stopLoading, () => false)   // Set loading to false when stopLoading action is dispatched
);

import { Action } from '@ngrx/store';

export function loadingReducer(state: boolean = initialState, action: Action) {
  return _loadingReducer(state, action);
}
