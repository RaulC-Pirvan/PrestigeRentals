import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeactivateAccountFormComponent } from './deactivate-account-form.component';

describe('DeactivateAccountFormComponent', () => {
  let component: DeactivateAccountFormComponent;
  let fixture: ComponentFixture<DeactivateAccountFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeactivateAccountFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeactivateAccountFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
