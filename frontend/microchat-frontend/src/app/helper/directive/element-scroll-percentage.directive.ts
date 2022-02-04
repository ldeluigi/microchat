import { Directive, ElementRef, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { ElementScrollPercentageService } from 'src/app/services/element-scroll-percentage.service';

@Directive({
  selector: '[appElementScrollPercentage]'
})
export class ElementScrollPercentageDirective implements OnInit, OnDestroy {

  @Output("scroll") public scrollPercentageEvent: EventEmitter<number>;

  private elementRef: ElementRef;
  private elementScrollPercentage: ElementScrollPercentageService;
  private subscription!: Subscription;

  // I initialize the element scroll percentage directive.
  constructor(
    elementRef: ElementRef,
    elementScrollPercentage: ElementScrollPercentageService
  ) {

    this.elementRef = elementRef;
    this.elementScrollPercentage = elementScrollPercentage;

    this.scrollPercentageEvent = new EventEmitter();

  }

  // I get called once when the directive is being unmounted.
  public ngOnDestroy(): void {
    (this.subscription) && this.subscription.unsubscribe();
  }


  // I get called once after the inputs have been bound for the first time.
  public ngOnInit(): void {

    // The purpose of the directive is to act as the GLUE between the element scroll
    // service and the host element for this directive. Let's subscribe to the scroll
    // events and then pipe them into the output event for this directive.
    this.subscription = this.elementScrollPercentage
      .getScrollAsStream(this.elementRef.nativeElement)
      .subscribe(
        (percent: number): void => {
          this.scrollPercentageEvent.next(percent);
        });
  }
}
