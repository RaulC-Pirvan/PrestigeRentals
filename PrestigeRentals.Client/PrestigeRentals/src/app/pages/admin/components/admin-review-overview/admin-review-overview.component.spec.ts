import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminReviewOverviewComponent } from './admin-review-overview.component';

describe('AdminReviewOverviewComponent', () => {
  let component: AdminReviewOverviewComponent;
  let fixture: ComponentFixture<AdminReviewOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminReviewOverviewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminReviewOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
