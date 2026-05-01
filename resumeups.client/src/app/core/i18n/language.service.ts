import { computed, Injectable, signal } from '@angular/core';
import { LanguageCode, SUPPORTED_LANGUAGES } from './language.types';

const STORAGE_KEY = 'resumeups.language';
const DEFAULT_LANGUAGE: LanguageCode = 'en';

@Injectable({ providedIn: 'root' })
export class LanguageService {
  private readonly languageState = signal<LanguageCode>(this.getInitialLanguage());

  readonly currentLanguage = computed(() => this.languageState());

  setLanguage(language: LanguageCode): void {
    if (this.languageState() === language) {
      return;
    }

    this.languageState.set(language);
    this.persistLanguage(language);
  }

  private getInitialLanguage(): LanguageCode {
    if (typeof window === 'undefined') {
      return DEFAULT_LANGUAGE;
    }

    const savedLanguage = window.localStorage.getItem(STORAGE_KEY);
    return this.isSupportedLanguage(savedLanguage) ? savedLanguage : DEFAULT_LANGUAGE;
  }

  private persistLanguage(language: LanguageCode): void {
    if (typeof window === 'undefined') {
      return;
    }

    window.localStorage.setItem(STORAGE_KEY, language);
  }

  private isSupportedLanguage(language: string | null): language is LanguageCode {
    return language !== null && SUPPORTED_LANGUAGES.includes(language as LanguageCode);
  }
}
