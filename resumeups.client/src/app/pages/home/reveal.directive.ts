import {
    Directive,
    ElementRef,
    Input,
    OnDestroy,
    OnInit,
    Renderer2,
  } from '@angular/core';
  
  export type ScrollRevealVariant =
    | 'fade-up'
    | 'fade-down'
    | 'fade-left'
    | 'fade-right'
    | 'fade'
    | 'zoom-in'
    | 'flip-up';
  
  @Directive({
    selector: '[scrollReveal]',
    standalone: true,
  })
  export class ScrollRevealDirective implements OnInit, OnDestroy {
    @Input('scrollReveal') variant: ScrollRevealVariant = 'fade-up';
      @Input() revealDelay = 0;
      @Input() revealDuration = 600;
      @Input() revealThreshold = 0.15;
      @Input() revealOnce = true;
  
    private observer: IntersectionObserver | null = null;
  
    constructor(private el: ElementRef<HTMLElement>, private renderer: Renderer2) {}
  
    ngOnInit() {
      this.setInitialState();
      this.createObserver();
    }
  
    ngOnDestroy() {
      this.observer?.disconnect();
    }
  
    private setInitialState() {
      const el = this.el.nativeElement;
      this.renderer.setStyle(
        el,
        'transition',
        `opacity ${this.revealDuration}ms cubic-bezier(0.25, 0.46, 0.45, 0.94) ${this.revealDelay}ms, transform ${this.revealDuration}ms cubic-bezier(0.25, 0.46, 0.45, 0.94) ${this.revealDelay}ms`
      );
      this.renderer.setStyle(el, 'will-change', 'opacity, transform');
  
      this.renderer.setStyle(el, 'opacity', '0');
      const transform = this.getHiddenTransform();
      if (transform) this.renderer.setStyle(el, 'transform', transform);
    }
  
    private getHiddenTransform(): string {
      switch (this.variant) {
        case 'fade-up':    return 'translateY(40px)';
        case 'fade-down':  return 'translateY(-40px)';
        case 'fade-left':  return 'translateX(40px)';
        case 'fade-right': return 'translateX(-40px)';
        case 'zoom-in':    return 'scale(0.88)';
        case 'flip-up':    return 'perspective(600px) rotateX(18deg) translateY(30px)';
        case 'fade':
        default:           return '';
      }
    }
  
    private createObserver() {
      this.observer = new IntersectionObserver(
        (entries) => {
          entries.forEach((entry) => {
            if (entry.isIntersecting) {
              this.reveal();
              if (this.revealOnce) this.observer?.disconnect();
            } else if (!this.revealOnce) {
              this.hide();
            }
          });
        },
        { threshold: this.revealThreshold }
      );
      this.observer.observe(this.el.nativeElement);
    }
  
    private reveal() {
      const el = this.el.nativeElement;
      this.renderer.setStyle(el, 'opacity', '1');
      this.renderer.setStyle(el, 'transform', 'none');
      setTimeout(
        () => this.renderer.setStyle(el, 'will-change', 'auto'),
        this.revealDuration + this.revealDelay + 100
      );
    }
  
    private hide() {
      const el = this.el.nativeElement;
      this.renderer.setStyle(el, 'opacity', '0');
      const transform = this.getHiddenTransform();
      if (transform) this.renderer.setStyle(el, 'transform', transform);
    }
  }