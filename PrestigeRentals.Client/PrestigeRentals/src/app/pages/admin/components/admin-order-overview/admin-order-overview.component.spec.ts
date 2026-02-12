import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminOrderOverviewComponent } from './admin-order-overview.component';

describe('AdminOrderOverviewComponent', () => {
  let component: AdminOrderOverviewComponent;
  let fixture: ComponentFixture<AdminOrderOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminOrderOverviewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminOrderOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
