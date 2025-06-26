import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BannedAccountDialogComponent } from './banned-account-dialog.component';

describe('BannedAccountDialogComponent', () => {
  let component: BannedAccountDialogComponent;
  let fixture: ComponentFixture<BannedAccountDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BannedAccountDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BannedAccountDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
