import { Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { LanguageService } from '../../../../core/i18n/language.service';
import { LanguageCode } from '../../../../core/i18n/language.types';
import { headerTranslations } from '../../../../i18n/header.i18n';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './header.component.html'
})
export class HeaderComponent {
  private readonly languageService = inject(LanguageService);

  readonly currentLanguage = this.languageService.currentLanguage;
  readonly text = computed(() => headerTranslations[this.currentLanguage()]);

  setLanguage(language: LanguageCode): void {
    this.languageService.setLanguage(language);
  }
}
