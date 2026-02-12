import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextShowcaseComponent } from './text-showcase.component';

describe('TextShowcaseComponent', () => {
  let component: TextShowcaseComponent;
  let fixture: ComponentFixture<TextShowcaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TextShowcaseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TextShowcaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
