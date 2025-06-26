import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleSuggestionComponent } from './vehicle-suggestion.component';

describe('VehicleSuggestionComponent', () => {
  let component: VehicleSuggestionComponent;
  let fixture: ComponentFixture<VehicleSuggestionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VehicleSuggestionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VehicleSuggestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
