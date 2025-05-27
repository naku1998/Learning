# Directive
In Angular, directives are classes that extend HTML elements with new behaviors or functionalities, allowing you to manipulate the DOM, control rendering, and encapsulate reusable logic. They come in two main types: structural and attribute directives. 
Here's a more detailed breakdown:

# Types of Directives:

1. Structural Directives:
These directives modify the DOM layout by adding or removing elements. 
Examples: *ngIf (conditionally renders elements), *ngFor (iterates over arrays), *ngSwitch (renders different views based on a condition). 

2. Attribute Directives:
These directives modify the appearance or behavior of existing elements, attributes, properties, or components. 
Examples: ngClass (adds and removes CSS classes), ngStyle (applies inline styles), ngModel (two-way data binding). 

3. Custom Directive:
We can also make custom directive
using cli


# Pipes
Pipes are a special operator in Angular template expressions that allows you to transform data declaratively in your template. Pipes let you declare a transformation function once and then use that transformation across multiple templates. Angular pipes use the vertical bar character (|)

# ? How to make custom pipe

# step 1 Generate a pipe in same module using cli or create this file

import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'lowercaseText'
})
export class LowercaseTextPipe implements PipeTransform {
  transform(value: string): string {
    if (!value) return '';
    return value.toLowerCase();
  }
}

# step 2 declare pipe in current module in declerations

 import { LowercaseTextPipe } from './lowercase-text.pipe';

@NgModule({
  declarations: [
    // other components/pipes
    LowercaseTextPipe
  ],
  // ...
})
export class AppModule { }

# step 3 use the pipe in component
<p>{{ 'Hello WORLD' | lowercaseText }}</p>


# In case of angular >16 you can make pipe standalone and directly implement in the component

# step 1
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'smallcase',
  standalone: true
})
export class SmallcasePipe implements PipeTransform {

  transform(value: any): string {
    return value.toString().toUpperCase();
  }

}

# step 2 import in component you want to use and add in imports array
import { Component } from '@angular/core';
import { SmallcasePipe } from '../../app/smallcase.pipe';

@Component({
  selector: 'app-home',
  imports: [SmallcasePipe],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  standalone: true
})
export class HomeComponent {
title = 'edgdg 19';
}

# step 3 use
<p>{{title|smallcase}}</p>

# ? How to make custom Directive

# step 1 create a directive using cli : ng g d 

import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[appHover]',
  standalone:true
})

<!-- 
@Directive(...): This decorator marks the class as an Angular directive.

selector: '[appHover]': This makes the directive usable as an attribute like [appHover]="'yellow'".

standalone: true: Means this directive doesn’t need to be declared in any NgModule, you can directly import it into components. 
-->

export class HoverDirective {

  @Input() appHoverHighlight = 'yellow';

<!-- 
        This allows the directive to accept an input color value.

        Example usage in template:

        html
        Copy
        Edit
        <div [appHoverHighlight]="'lightgreen'">Hover me</div> 
  -->


  constructor(private el: ElementRef) {}

  @HostListener('mouseenter') onMouseEnter() {
    this.highlight(this.appHoverHighlight);
  }

  <!-- As we have used directive tag on element html so hostlistner will listen for mouse enter event  Calls highlight() with the color you passed (or default 'yellow'). -->

  @HostListener('mouseleave') onMouseLeave() {
    this.highlight('');
  }
  <!-- As we have used directive tag on element html so hostlistner will listen for mouse Leave event  -->

  private highlight(color: string) {
    this.el.nativeElement.style.backgroundColor = color;
  }

  <!-- Directly changes the DOM element's background color via ElementRef. -->
}



# step 2 import in component where u want to use and add in imports array

import { Component } from '@angular/core';
import { SmallcasePipe } from '../../app/smallcase.pipe';
import {HoverDirective} from '../../app/hover.directive';

@Component({
  selector: 'app-home',
  imports: [SmallcasePipe,HoverDirective],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  standalone: true
})
export class HomeComponent {
title = 'edgdg 19';
}

# step 3 use like this or without giving color
<h2 [appHoverHighlight]="'lightblue'">Hover over me!</h2>
                       OR
<h2 appHoverHighlight>Hover over me!</h2>

