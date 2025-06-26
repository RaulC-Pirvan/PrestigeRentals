import { Component, Input, Output, EventEmitter} from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-button',
  imports: [CommonModule],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss'
})

export class ButtonComponent {
  @Input() buttonText: string = 'Default Button'; 
  
  @Input() buttonType: string = 'primary'; 
  
  @Output() buttonClick = new EventEmitter<void>();

  @Input() disabled: boolean = false;

  onClick() {
    this.buttonClick.emit();
  }
}
