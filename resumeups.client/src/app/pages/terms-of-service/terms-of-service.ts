import { Component, computed, inject } from '@angular/core';
import { LanguageService } from '../../core/i18n/language.service';
import { termsOfServiceTranslations } from '../../i18n/terms-of-service.i18n';
import { ScrollRevealDirective } from '../home/reveal.directive';

@Component({
  selector: 'app-terms-of-service',
  standalone: true,
  imports: [ScrollRevealDirective],
  templateUrl: './terms-of-service.html'
})
export class TermsOfServicePage {
  private readonly languageService = inject(LanguageService);

  readonly text = computed(() => termsOfServiceTranslations[this.languageService.currentLanguage()]);
}
