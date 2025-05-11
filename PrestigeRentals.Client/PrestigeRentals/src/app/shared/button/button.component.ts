import { Component, Input, Output, EventEmitter} from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-button',
  imports: [CommonModule],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss'
})

export class ButtonComponent {
  // Input to dynamically set the button's text
  @Input() buttonText: string = 'Default Button'; 
  
  // Input to define the button's type (e.g., "primary", "secondary")
  @Input() buttonType: string = 'primary'; 
  
  // Output event when the button is clicked
  @Output() buttonClick = new EventEmitter<void>();

  // Emit the button click event
  onClick() {
    this.buttonClick.emit();
  }
}
