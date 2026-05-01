import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LanguageService } from '../../core/i18n/language.service';
import { homeTranslations } from '../../i18n/home.i18n';
import { ScrollRevealDirective } from './reveal.directive';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, ScrollRevealDirective],
  templateUrl: './home.html'
})
export class HomePage {
  private readonly languageService = inject(LanguageService);

  readonly text = computed(() => homeTranslations[this.languageService.currentLanguage()]);
}
