import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTicketOverviewComponent } from './admin-ticket-overview.component';

describe('AdminTicketOverviewComponent', () => {
  let component: AdminTicketOverviewComponent;
  let fixture: ComponentFixture<AdminTicketOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminTicketOverviewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminTicketOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
