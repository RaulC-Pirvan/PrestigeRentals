import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarousellShowcaseComponent } from './carousell-showcase.component';

describe('CarousellShowcaseComponent', () => {
  let component: CarousellShowcaseComponent;
  let fixture: ComponentFixture<CarousellShowcaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CarousellShowcaseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CarousellShowcaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
