# 1. Install NgRx packages
ng add @ngrx/store
ng add @ngrx/effects
ng add @ngrx/store-devtools

# 2. Create Task Model
📄 state/task.model.ts

export interface Task {
  id: number;
  title: string;
  status: 'board' | 'backlog';
}