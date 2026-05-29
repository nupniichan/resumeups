import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Location } from '@angular/common';
import { LanguageService } from '../../../core/i18n/language.service';
import { notFoundTranslations } from '../../../i18n/not-found.i18n';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './not-found.html',
  styleUrl: './not-found.css'
})
export class NotFoundPage {
  private readonly languageService = inject(LanguageService);
  private readonly location = inject(Location);

  readonly text = computed(() => notFoundTranslations[this.languageService.currentLanguage()]);

  goBack(): void {
    this.location.back();
  }
}
