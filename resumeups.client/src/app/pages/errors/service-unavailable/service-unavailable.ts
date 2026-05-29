import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LanguageService } from '../../../core/i18n/language.service';
import { serviceUnavailableTranslations } from '../../../i18n/service-unavailable.i18n';

@Component({
  selector: 'app-service-unavailable',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './service-unavailable.html',
  styleUrl: './service-unavailable.css'
})
export class ServiceUnavailablePage {
  private readonly languageService = inject(LanguageService);

  readonly text = computed(() => serviceUnavailableTranslations[this.languageService.currentLanguage()]);

  goBack(): void {
    window.location.reload();
  }
}
