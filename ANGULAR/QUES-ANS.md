
# Q1: What is Angular?
Ans: Angular is a TypeScript-based, component-driven framework maintained by Google for building single-page applications (SPAs).
Single page application is a web application that loads a single HTML page and dynamically updates the content as the user interacts with it.
SPA is angular is possible because of the use of routing and lazy loading.
 It provides first-class features like routing, reactive forms, dependency injection (DI), RxJS for async streams, and a robust CLI. Angular compiles templates to efficient JavaScript (AOT) and uses a hierarchical injector for scalable architectures.



# Q2: What is a Component?
Ans: A component is the smallest UI building block in Angular. It combines a template (HTML), styles (CSS), and a class (TypeScript) with metadata via @Component. Components get data via @Input() and emit events via @Output().


# Q3: What are Providers?
Providers in Angular in layman terms is a way to tell angular how to create a dependency for a given token. You can provide classes, values, or factories at different scopes (root, feature module, component).
Provider

# Q4: Explain Angular Lifecycle Hooks.
Ans: Hooks are callbacks that run at key points of a component/directive’s life:

ngOnChanges (on @Input changes)

ngOnInit (once after first ngOnChanges)

ngDoCheck (custom change detection)

ngAfterContentInit/Checked (content projection)

ngAfterViewInit/Checked (view children ready)

ngOnDestroy (cleanup: unsubscribe, detach)
Example:




export class ProfileComponent implements OnInit, OnDestroy {
  sub?: Subscription;
  ngOnInit(){ /* start timers, subscribe */ }
  ngOnDestroy(){ this.sub?.unsubscribe(); }
}

# Q5: Difference between TypeScript and JavaScript.
Ans: TypeScript is a superset of JavaScript that adds static typing, decorators, interfaces, and modern features compiled to JS. It improves tooling (intellisense, refactors) and catches errors at compile time. JavaScript is dynamically typed and executed as-is by browsers. Angular is written in TS and relies on its decorators and types.


# Q6: What are Directives in Angular?
Ans: Directives are classes that add behavior to elements:

Components (have a template)

Structural (*ngIf, *ngFor, *ngSwitch) — change DOM layout

Attribute ([ngClass], [ngStyle]) — change appearance/behavior

# Q7: What is NgModule?
Ans: An NgModule groups components, directives, pipes, and services into a cohesive block. It controls compilation scope and reusability via imports/exports. Root apps use AppModule; feature modules organize domains; CommonModule, RouterModule, HttpClientModule are common imports.
Example:


@NgModule({
  declarations: [ProductsComponent],
  imports: [CommonModule, RouterModule],
  exports: [ProductsComponent]
})
export class ProductsModule {}

# Q8: What is an Injector?
Ans: The injector is Angular’s DI container that resolves tokens to instances. Injectors form a hierarchy: child injectors can override parent providers, enabling per-feature or per-component service instances.


# Q9: What is a Decorator?
Ans: Decorators add metadata used by Angular. Key decorators: @Component, @Directive, @Pipe, @NgModule, @Injectable, @Input, @Output, @Inject.
Example:




@Injectable({ providedIn: 'root' })
export class AuthService {}

# Q10: What is ng-template?
Ans: ng-template defines a block of HTML that’s not rendered by default. Angular renders it only when referenced (e.g., with *ngIf, ngTemplateOutlet).
Example:

html


<ng-template 
#loading>Loading...</ng-template>
<div *ngIf="data; else loading">{{data}}</div>

# Q11: What is ng-content?
Ans: It marks a slot for content projection (pass HTML from parent to child). Supports multiple slots via select.
Example:

html


<!-- child -->
<ng-content select=".header"></ng-content>
<ng-content></ng-content>

# Q12: What is content projection?
Ans: The techni
# Que of injecting parent content into a child component’s template using <ng-content>, enabling reusable shell components and layout wrappers.


# Q13: What is ng-container?
Ans: A logical container that doesn’t render to the DOM. Useful to apply structural directives without adding extra elements.
Example:

html


<ng-container *ngIf="user">
  <span>{{user.name}}</span>
</ng-container>

# Q14: What are @Input and @Output in Angular?
Ans: @Input() receives data from parent to child; @Output() emits events/data to parent via EventEmitter.
Example:




@Component({selector:'counter', template:`<button (click)="inc()">+</button>`})
export class Counter {
  @Input() value = 0;
  @Output() valueChange = new EventEmitter<number>();
  inc(){ this.valueChange.emit(this.value + 1); }
}

# Q15: Difference between ng serve and npm start.
Ans: ng serve is an Angular CLI command that runs the dev server with live reload. npm start simply runs the script defined in package.json (often "ng serve" but can be anything).


# Q16: What is RxJS?
Ans: RxJS is a library for reactive programming using Observables. Angular uses RxJS for HTTP, router events, form valueChanges, and more. Operators (e.g., map, switchMap, catchError) transform streams.


# Q17: What is an Observable?
Ans: An Observable represents a lazy stream of values over time (0..n). Consumers subscribe to receive next/error/complete. Subscriptions can be cancelled (unsubscribe).
Example:




this.http.get<User[]>('/api/users')
  .subscribe(users => console.log(users));

# Q18: What is a Promise?
Ans: A Promise represents a single eventual result or error. It is eager, not cancellable, and resolves once.
Example:




fetch('/api/users').then(r => r.json()).then(console.log);

# Q19: Difference between Observable and Promise.
Ans: Observables are lazy, cancellable, can emit multiple values, and have rich operators; Promises are eager, non-cancellable, and emit one value.


# Q20: What is map in Angular?
Ans: map is an RxJS pipeable operator that transforms each emitted value.
Example:




this.http.get<User[]>('/api/users').pipe(
  map(users => users.length)
).subscribe(count => console.log(count));

# Q21: What is subscribe?
Ans: subscribe attaches an observer to an Observable to receive values, handle errors, and completion. It returns a Subscription you should clean up (e.g., in ngOnDestroy) if the stream does not complete automatically.
Example:




const sub = timer(0, 1000).subscribe(v => console.log(v));
// later: sub.unsubscribe();

# Q22: What is a Pipe in Angular?
Ans: Pipes transform values for display in templates (e.g., date, currency, uppercase, async). You can create custom pipes with @Pipe.
Example:

html


<p>{{ price | currency:'INR' }}</p>

# Q23: What are Pure and Impure pipes?
Ans: Pure pipes (default) run only when the input reference changes—performant. Impure pipes (pure:false) run on every change detection cycle; use sparingly (e.g., when mutating arrays).
Example:




@Pipe({ name:'impureFilter', pure:false })
export class ImpureFilterPipe { /* ... */ }

# Q24: What is a Service in Angular?
Ans: A service is a class that encapsulates business logic, data access, or shared state. Mark with @Injectable and provide it so components can inject and reuse it.
Example:




@Injectable({ providedIn:'root' })
export class TodoService {
  getTodos(){ return this.http.get<Todo[]>('/api/todos'); }
  constructor(private http: HttpClient) {}
}

# Q25: What is Dependency Injection in Angular?
Ans: DI lets classes declare what they need (tokens) instead of creating instances. The injector constructs and supplies those dependencies based on configured providers, improving testability and modularity.
Example (testable):




@Component({/*...*/})
export class ProfileComponent {
  constructor(public auth: AuthService) {}
}

# Q26: What is Lazy Loading?
Ans: Lazy loading defers loading of feature modules until their routes are visited, reducing initial bundle size and speeding up first paint.
Example (standalone or module-based):




const routes: Routes = [
  { path: 'admin', loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule) }
];

# Q27: What is Interpolation?
Ans: Interpolation {{ expr }} inserts the stringified result of a component expression into the template. It’s one-way (component → view).
Example: <h3>Hello {{user?.name}}</h3>


# Q28: What is @ViewChild?
Ans: @ViewChild 
# Queries the component’s template for a child element/directive/component instance. Safe to use after ngAfterViewInit.
Example:




@ViewChild('inp') input!: ElementRef<HTMLInputElement>;
ngAfterViewInit(){ this.input.nativeElement.focus(); }

# Q29: What is a “Session” in Angular (state persistence)?
Ans: Angular itself doesn’t have server sessions. Persist client state with localStorage, sessionStorage, IndexedDB, or a state store (NgRx). For auth, store tokens securely (prefer HttpOnly cookies on server; or localStorage with care).
Example:




localStorage.setItem('theme', 'dark');
const theme = localStorage.getItem('theme');

# Q30: What are Reactive Forms?
Ans: Reactive Forms are model-driven: form state is built in code using FormControl, FormGroup, FormArray. They provide immutable updates, explicit validation, and great testability.
Example:




form = new FormGroup({
  email: new FormControl('', [Validators.re
# Quired, Validators.email]),
  age: new FormControl(18, Validators.min(18))
});

# Q31: What is FormBuilder?
Ans: FormBuilder reduces boilerplate when creating controls/groups/arrays.
Example:




constructor(private fb: FormBuilder) {}
form = this.fb.group({
  name: this.fb.control(''),
  skills: this.fb.array([this.fb.control('Angular')])
});

# Q32: What are FormGroup and FormControl?
Ans: FormControl represents a single input with value, validation state, and status. FormGroup aggregates controls (and/or arrays) and computes their combined validity.
Example:




const name = new FormControl('Nakul');
const group = new FormGroup({ name });

# Q33: What is an Auth Guard in Angular?
Ans: An Auth Guard enforces authentication/authorization rules before navigating to a route. Typically checks for a token/role and returns true, false, or a UrlTree to redirect.
Example:




@Injectable({providedIn:'root'})
export class AuthGuard implements CanActivate {
  constructor(private auth:AuthService, private router:Router){}
  canActivate(): boolean|UrlTree {
    return this.auth.loggedIn ? true : this.router.parseUrl('/login');
  }
}

# Q34: What is a Route Guard?
Ans: Route guards are interfaces to control navigation:

CanActivate, CanActivateChild, CanDeactivate, Resolve, CanMatch.
They can return boolean | UrlTree | Observable | Promise. Use to block, redirect, prefetch data, or check unsaved changes.


# Q35: What is canActivate?
Ans: It’s a guard method that decides if a route can be activated. Return true to allow, false or a UrlTree to block/redirect; can be async.
Example: see AuthGuard above.


# Q36: What is NgRx?
Ans: NgRx is a Redux-inspired state management library for Angular. It centralizes state in a store, updates state via actions handled by reducers, and performs side effects with effects.
Example (minimal):




createAction('[Cart] Add Item'); // action
createReducer(initialState, on(addItem, (s,a)=>({...s, items:[...s.items, a.item]})));

# Q37: What is a Web Worker in Angular?
Ans: Web Workers run heavy CPU tasks on a background thread so the UI remains smooth. Communicate via postMessage and onmessage. Angular CLI can scaffold workers.
Example (worker.):




addEventListener('message', ({ data }) => {
  const result = expensiveCalc(data);
  postMessage(result);
});

# Q38: What is a Wildcard route?
Ans: A route with path ** that matches any URL not matched by previous routes—used for 404 pages or redirects.
Example:




{ path: '**', component: NotFoundComponent }

# Q39: What is Ivy?
Ans: Ivy is Angular’s modern rendering and compilation engine that enables smaller bundles, faster compilation, better debugging, and advanced features (like standalone components and more efficient change detection).


# Q40: What is Bazel in the Angular ecosystem?
Ans: Bazel is a high-performance build system that supports large monorepos and incremental/remote builds. In Angular contexts, it can speed up CI and complex workspace builds (though not re
# Quired for typical CLI apps).


# Q41: What are HTTP Interceptors in Angular?
Ans: Interceptors are services that sit between your app and the HttpClient. They can read/modify outgoing re
# Quests and incoming responses—useful for auth tokens, logging, error handling, retries.
Example:




@Injectable()
export class AuthInterceptor implemen HttpInterceptor {
  intercept(re
# Q: HttpRe
# Quest<any>, next: HttpHandler) {
    const cloned = re
# Q.clone({ setHeaders: { Authorization: `Bearer ${token}` }});
    return next.handle(cloned);
  }
}

@NgModule({
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }]
})
export class CoreModule {}