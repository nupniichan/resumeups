import { Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LanguageService } from '../../core/i18n/language.service';
import { resumeCheckerTranslations } from '../../i18n/resume-checker.i18n';
import { ScrollRevealDirective } from '../home/reveal.directive';

@Component({
  selector: 'app-resume-checker',
  standalone: true,
  imports: [ScrollRevealDirective, RouterLink],
  templateUrl: './resume-checker.html'
})
export class ResumeCheckerPage {
  private readonly languageService = inject(LanguageService);

  readonly text = computed(() => resumeCheckerTranslations[this.languageService.currentLanguage()]);
  readonly selectedFileName = signal('');
  readonly jdInput = signal('');
  readonly fileError = signal('');
  readonly jdError = signal('');
  readonly consentError = signal('');
  readonly termsAccepted = signal(false);
  readonly showResult = signal(false);
  readonly openedFaqIndex = signal<number | null>(0);

  private readonly acceptedMimeTypes = new Set([
    'application/pdf',
    'application/msword',
    'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
  ]);

  onFileSelected(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const selectedFile = inputElement.files?.[0];

    this.fileError.set('');
    this.showResult.set(false);

    if (!selectedFile) {
      this.selectedFileName.set('');
      return;
    }

    const normalizedName = selectedFile.name.toLowerCase();
    const isSupportedByExtension =
      normalizedName.endsWith('.pdf') || normalizedName.endsWith('.doc') || normalizedName.endsWith('.docx');
    const isSupportedByMime = this.acceptedMimeTypes.has(selectedFile.type);

    if (!isSupportedByMime && !isSupportedByExtension) {
      this.selectedFileName.set('');
      this.fileError.set(this.text().form.invalidFileError);
      inputElement.value = '';
      return;
    }

    this.selectedFileName.set(selectedFile.name);
  }

  onJdInput(value: string): void {
    this.jdInput.set(value);
    if (this.jdError()) {
      this.jdError.set('');
    }
    this.showResult.set(false);
  }

  onTermsAcceptedChange(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    this.termsAccepted.set(inputElement.checked);
    if (inputElement.checked) {
      this.consentError.set('');
    }
  }

  onSubmit(): void {
    const jdValue = this.jdInput().trim();
    let isValid = true;

    if (!this.selectedFileName()) {
      this.fileError.set(this.text().form.invalidFileError);
      isValid = false;
    }

    if (!jdValue) {
      this.jdError.set(this.text().form.requiredJdError);
      isValid = false;
    }

    if (!this.termsAccepted()) {
      this.consentError.set(this.text().form.consentRequiredError);
      isValid = false;
    }

    if (!isValid) {
      this.showResult.set(false);
      return;
    }

    this.jdError.set('');
    this.fileError.set('');
    this.consentError.set('');
    this.showResult.set(true);
  }

  toggleFaq(index: number): void {
    this.openedFaqIndex.set(this.openedFaqIndex() === index ? null : index);
  }
}
