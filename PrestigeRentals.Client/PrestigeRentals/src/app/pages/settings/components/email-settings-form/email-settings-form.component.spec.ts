import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailSettingsFormComponent } from './email-settings-form.component';

describe('EmailSettingsFormComponent', () => {
  let component: EmailSettingsFormComponent;
  let fixture: ComponentFixture<EmailSettingsFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmailSettingsFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmailSettingsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
