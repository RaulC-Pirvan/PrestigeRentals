import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailsSettingsFormComponent } from './details-settings-form.component';

describe('DetailsSettingsFormComponent', () => {
  let component: DetailsSettingsFormComponent;
  let fixture: ComponentFixture<DetailsSettingsFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetailsSettingsFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetailsSettingsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
