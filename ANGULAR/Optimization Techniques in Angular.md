# Lazy Loading
Lazy loading is a technique where a resource (like a component, module, image, or data) is loaded only when it’s actually needed, rather than when the application first loads.

The idea is simple: 
# Without lazy loading 
Everything loads upfront, which can make the initial load slow.

# With lazy loading
 Only the parts of the app that are needed right away are loaded; other parts load later, on demand.

 How to Achieve this in Angular
 1.See the Data which is required Upfront and when calling an api fetch only required data instead of full JSON Like we did in Matrix for Board view, Issues page.
 

OPTIMIZATION TECHNIQUES IN ANGULAR
1.Lazy Loading
2.OnPush Change Detection Strategy
3.Avoiding Change Detection
4.Avoiding Expression Change
5.Avoiding Unnecessary Rendering
6.Avoiding Unnecessary API Calls
7.Avoiding Unnecessary DOM Manipulation
8.Avoiding Unnecessary Data Binding
9.Avoiding Unnecessary Event Listeners
10.Avoiding Unnecessary Pipes

OnPush Change Detection is a change detection strategy in Angular that can be used to improve performance by reducing the number of times change detection is run.
Avoiding Chanege Detection is a technique where we avoid using change detection at all. This can be done by using immutable data structures and avoiding mutable data.
Avoiding Expression change is a technique where we avoid using expressions in our templates. This can be done by using pipes and avoiding complex expressions.
Expressions in angular is {{}}.AN example of avoiding expression change is by using pipes is by using {{data | json}} instead of {{data}}.


Avoiding Unnecessary Rendering is a technique where we avoid rendering components that are not needed. This can be done by using *ngIf and *ngFor directives.
Avoiding Unnecessary API Calls is a technique where we avoid making API calls that are not needed. This can be done by using caching and avoiding unnecessary API calls.

Avoiding Unnecessary DOM Manipulation is a technique where we avoid manipulating the DOM unnecessarily. This can be done by using OnPush Change Detection Strategy and Avoiding Change Detection.
Avoiding Unnecessary Data Binding is a technique where we avoid binding data to the view that is not needed. This can be done by using OnPush Change Detection Strategy and Avoiding Change Detection.

Avoiding Unnecessary Event Listeners is a technique where we avoid listening to events that are not needed. This can be done by using takeUntil operator and avoiding unnecessary event listeners.
EXAPLE OF takeUntil operator is 
private destroy$ = new Subject<void>();
ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnInit(): void {
    this.someEvent$.pipe(takeUntil(this.destroy$)).subscribe(data => {
      // do something with data
    });
  }

Avoiding unnecesary pipes is a technique where we avoid using pipes that are not needed. This can be done by using pure pipes and avoiding impure pipes.
Pure and impure pipes are the two types of pipes in angular. Pure pipes are the pipes that are used by default in angular. Impure pipes are the pipes that are used when we want to avoid using change detection.example of pure and impure pipes is 
pure pipe {{data | json}}
impure pipe {{data | json : true}}


standalone components are the components that are not part of any module. They are used when we want to avoid using modules.
standalone modules are the modules that are not part of any other module. They are used when we want to avoid using modules.
angular 18 vs angular 14

# Module-to-Module Data Transfer Implementation

Module-to-module data transfer in Angular refers to sharing data between different feature modules in an application. This is essential for building scalable applications where modules need to communicate with each other.

## Common Approaches for Module-to-Module Data Transfer

### 1. Shared Services with Dependency Injection

The most common and recommended approach is using shared services that are provided at the root level or in a shared module.

**Implementation:**

```typescript
// shared.service.ts
@Injectable({
  providedIn: 'root'
})
export class SharedDataService {
  private dataSubject = new BehaviorSubject<any>(null);
  public data$ = this.dataSubject.asObservable();

  updateData(data: any): void {
    this.dataSubject.next(data);
  }

  getData(): any {
    return this.dataSubject.value;
  }
}
```

**Usage in Module A:**
```typescript
// module-a.component.ts
export class ModuleAComponent {
  constructor(private sharedService: SharedDataService) {}

  sendData(): void {
    const data = { message: 'Hello from Module A', timestamp: Date.now() };
    this.sharedService.updateData(data);
  }
}
```

**Usage in Module B:**
```typescript
// module-b.component.ts
export class ModuleBComponent implements OnInit {
  receivedData: any;

  constructor(private sharedService: SharedDataService) {}

  ngOnInit(): void {
    this.sharedService.data$.subscribe(data => {
      this.receivedData = data;
    });
  }
}
```

### 2. State Management with NgRx

For complex applications, NgRx provides a robust solution for module-to-module data transfer.

**Implementation:**

```typescript
// app.state.ts
export interface AppState {
  sharedData: any;
}

// shared.actions.ts
export const updateSharedData = createAction(
  '[Shared] Update Data',
  props<{ data: any }>()
);

// shared.reducer.ts
const initialState = null;

export const sharedDataReducer = createReducer(
  initialState,
  on(updateSharedData, (state, { data }) => data)
);

// Usage in components
export class ModuleAComponent {
  constructor(private store: Store<AppState>) {}

  sendData(): void {
    const data = { message: 'Hello from Module A' };
    this.store.dispatch(updateSharedData({ data }));
  }
}

export class ModuleBComponent implements OnInit {
  receivedData$ = this.store.select(state => state.sharedData);
}
```

### 3. Event Bus Pattern

Create a centralized event bus for loose coupling between modules.

**Implementation:**

```typescript
// event-bus.service.ts
@Injectable({
  providedIn: 'root'
})
export class EventBusService {
  private eventSubject = new Subject<{ type: string; payload: any }>();
  public events$ = this.eventSubject.asObservable();

  emit(type: string, payload: any): void {
    this.eventSubject.next({ type, payload });
  }

  on(eventType: string): Observable<any> {
    return this.events$.pipe(
      filter(event => event.type === eventType),
      map(event => event.payload)
    );
  }
}
```

**Usage:**
```typescript
// Module A - Emitting event
export class ModuleAComponent {
  constructor(private eventBus: EventBusService) {}

  sendData(): void {
    this.eventBus.emit('DATA_UPDATED', { message: 'Hello from Module A' });
  }
}

// Module B - Listening to event
export class ModuleBComponent implements OnInit {
  constructor(private eventBus: EventBusService) {}

  ngOnInit(): void {
    this.eventBus.on('DATA_UPDATED').subscribe(data => {
      console.log('Received data:', data);
    });
  }
}
```

### 4. Router State and Query Parameters

Transfer data through routing when navigating between modules.

**Implementation:**

```typescript
// Module A - Sending data via router
export class ModuleAComponent {
  constructor(private router: Router) {}

  navigateWithData(): void {
    this.router.navigate(['/module-b'], {
      queryParams: { data: JSON.stringify({ message: 'Hello from Module A' }) }
    });
  }
}

// Module B - Receiving data from route
export class ModuleBComponent implements OnInit {
  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['data']) {
        const receivedData = JSON.parse(params['data']);
        console.log('Received data:', receivedData);
      }
    });
  }
}
```

### 5. Local Storage / Session Storage

For persistent data that needs to survive page refreshes.

**Implementation:**

```typescript
// storage.service.ts
@Injectable({
  providedIn: 'root'
})
export class StorageService {
  setData(key: string, data: any): void {
    localStorage.setItem(key, JSON.stringify(data));
  }

  getData(key: string): any {
    const data = localStorage.getItem(key);
    return data ? JSON.parse(data) : null;
  }

  removeData(key: string): void {
    localStorage.removeItem(key);
  }
}
```

## Best Practices for Module-to-Module Data Transfer

### 1. Use TypeScript Interfaces
Define clear interfaces for data being transferred:

```typescript
export interface SharedData {
  id: string;
  message: string;
  timestamp: number;
  metadata?: any;
}
```

### 2. Implement Proper Error Handling

```typescript
@Injectable({
  providedIn: 'root'
})
export class SharedDataService {
  private dataSubject = new BehaviorSubject<SharedData | null>(null);
  public data$ = this.dataSubject.asObservable();

  updateData(data: SharedData): void {
    try {
      this.dataSubject.next(data);
    } catch (error) {
      console.error('Error updating shared data:', error);
    }
  }
}
```

### 3. Memory Management
Always unsubscribe to prevent memory leaks:

```typescript
export class ModuleBComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  ngOnInit(): void {
    this.sharedService.data$
      .pipe(takeUntil(this.destroy$))
      .subscribe(data => {
        // Handle data
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
```

### 4. Use Async Pipe When Possible
Reduces boilerplate and handles subscription management:

```html
<!-- module-b.component.html -->
<div *ngIf="sharedService.data$ | async as data">
  {{ data.message }}
</div>
```

## Performance Considerations

1. **Lazy Loading Compatibility**: Ensure shared services are properly provided when using lazy-loaded modules
2. **Change Detection**: Use OnPush strategy with observables for better performance
3. **Data Serialization**: Be mindful of data size when using localStorage or router params
4. **Subscription Management**: Always clean up subscriptions to prevent memory leaks

## When to Use Each Approach

- **Shared Services**: Best for real-time data sharing and simple state management
- **NgRx**: Ideal for complex state management and large applications
- **Event Bus**: Good for loose coupling and event-driven architecture
- **Router State**: Perfect for navigation-based data transfer
- **Local Storage**: Use for persistent data that survives page refreshes

Choose the approach based on your application's complexity, data persistence requirements, and architectural preferences.


Interceptor in angular is a technique where we can intercept the http request and response. This can be done by using HttpInterceptor.
The most comon use of interceptor is to add headers to the request.Headers are used to add authentication token to the request.

Here is how to add interceptor in angular.

1. Create a new file called auth.interceptor.ts
2. Import the following
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

3. Create a class called AuthInterceptor and implement HttpInterceptor
@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('token');
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }
    return next.handle(request);
  }
}

4. Add the interceptor to the providers array in app.module.ts
providers: [
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
]

5. Now whenever we make a http request the interceptor will add the token to the request.

Difference between promice and observable is that promise is used to handle async operations and observable is used to handle multiple async operations.
another difference is that promise is used to handle single event and observable is used to handle multiple events.
another is that promise is used in vanilla js and observable is used in rxjs.
OBSERVABLE IS LIKE A STREAM OF DATA AND PROMISE IS LIKE A SINGLE EVENT.

