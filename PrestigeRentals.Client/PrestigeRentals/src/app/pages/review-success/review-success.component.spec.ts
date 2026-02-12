import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewSuccessComponent } from './review-success.component';

describe('ReviewSuccessComponent', () => {
  let component: ReviewSuccessComponent;
  let fixture: ComponentFixture<ReviewSuccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReviewSuccessComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReviewSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
