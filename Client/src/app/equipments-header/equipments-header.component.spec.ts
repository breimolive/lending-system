import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EquipmentsHeaderComponent } from './equipments-header.component';

describe('EquipmentsHeaderComponent', () => {
  let component: EquipmentsHeaderComponent;
  let fixture: ComponentFixture<EquipmentsHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EquipmentsHeaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EquipmentsHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
