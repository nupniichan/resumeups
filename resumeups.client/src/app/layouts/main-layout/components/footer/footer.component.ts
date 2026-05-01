import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LanguageService } from '../../../../core/i18n/language.service';
import { footerTranslations } from '../../../../i18n/footer.i18n';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './footer.component.html'
})
export class FooterComponent {
  private readonly languageService = inject(LanguageService);

  readonly text = computed(() => footerTranslations[this.languageService.currentLanguage()]);
}
