import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerticalShowcaseComponent } from './vertical-showcase.component';

describe('VerticalShowcaseComponent', () => {
  let component: VerticalShowcaseComponent;
  let fixture: ComponentFixture<VerticalShowcaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VerticalShowcaseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerticalShowcaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
