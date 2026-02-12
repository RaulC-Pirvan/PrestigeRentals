import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThreeItemShowcaseComponent } from './three-item-showcase.component';

describe('ThreeItemShowcaseComponent', () => {
  let component: ThreeItemShowcaseComponent;
  let fixture: ComponentFixture<ThreeItemShowcaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ThreeItemShowcaseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ThreeItemShowcaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
