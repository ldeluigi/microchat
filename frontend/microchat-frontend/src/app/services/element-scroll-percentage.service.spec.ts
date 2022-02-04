import { TestBed } from '@angular/core/testing';

import { ElementScrollPercentageService } from './element-scroll-percentage.service';

describe('ElementScrollPercentageService', () => {
  let service: ElementScrollPercentageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ElementScrollPercentageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
