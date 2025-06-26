import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HalfPhotoShowcaseComponent } from './half-photo-showcase.component';

describe('HalfPhotoShowcaseComponent', () => {
  let component: HalfPhotoShowcaseComponent;
  let fixture: ComponentFixture<HalfPhotoShowcaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HalfPhotoShowcaseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HalfPhotoShowcaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
