import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PasswordSettingsFormComponent } from './password-settings-form.component';

describe('PasswordSettingsFormComponent', () => {
  let component: PasswordSettingsFormComponent;
  let fixture: ComponentFixture<PasswordSettingsFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PasswordSettingsFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PasswordSettingsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
