import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminVehicleCardComponent } from './admin-vehicle-card.component';

describe('AdminVehicleCardComponent', () => {
  let component: AdminVehicleCardComponent;
  let fixture: ComponentFixture<AdminVehicleCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminVehicleCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminVehicleCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
