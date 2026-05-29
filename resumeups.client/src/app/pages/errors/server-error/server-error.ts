import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Location } from '@angular/common';
import { LanguageService } from '../../../core/i18n/language.service';
import { serverErrorTranslations } from '../../../i18n/server-error.i18n';

@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './server-error.html',
  styleUrl: './server-error.css'
})
export class ServerErrorPage {
  private readonly languageService = inject(LanguageService);
  private readonly location = inject(Location);

  readonly text = computed(() => serverErrorTranslations[this.languageService.currentLanguage()]);

  goBack(): void {
    this.location.back();
  }
}
