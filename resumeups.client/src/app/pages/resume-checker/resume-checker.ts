import { Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LanguageService } from '../../core/i18n/language.service';
import { resumeCheckerTranslations } from '../../i18n/resume-checker.i18n';
import { ScrollRevealDirective } from '../home/reveal.directive';
import { ResumeCheckerService } from './resume-checker.service';
import { AnalyzeResult } from './resume-checker.models';

@Component({
  selector: 'app-resume-checker',
  standalone: true,
  imports: [ScrollRevealDirective, RouterLink],
  templateUrl: './resume-checker.html'
})
export class ResumeCheckerPage {
  private readonly languageService = inject(LanguageService);
  private readonly resumeCheckerService = inject(ResumeCheckerService);

  readonly text = computed(() => resumeCheckerTranslations[this.languageService.currentLanguage()]);
  readonly selectedFileName = signal('');
  readonly jdInput = signal('');
  readonly fileError = signal('');
  readonly jdError = signal('');
  readonly consentError = signal('');
  readonly termsAccepted = signal(false);
  readonly showResult = signal(false);
  readonly isLoading = signal(false);
  readonly apiError = signal('');
  readonly analyzeResult = signal<AnalyzeResult | null>(null);
  readonly openedFaqIndex = signal<number | null>(0);

  private selectedFile: File | null = null;

  private readonly acceptedMimeTypes = new Set([
    'application/pdf',
    'application/msword',
    'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
  ]);

  get matchScore(): number {
    return this.analyzeResult()?.matching?.matchScore ?? 0;
  }

  get feedbackScore(): number {
    return this.analyzeResult()?.feedback?.feedbackScore ?? 0;
  }

  get scoreCircumference(): number {
    return 2 * Math.PI * 52;
  }

  get scoreStrokeDashoffset(): number {
    return this.scoreCircumference * (1 - this.matchScore / 100);
  }

  onFileSelected(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const file = inputElement.files?.[0];

    this.fileError.set('');
    this.showResult.set(false);
    this.apiError.set('');

    if (!file) {
      this.selectedFileName.set('');
      this.selectedFile = null;
      return;
    }

    const normalizedName = file.name.toLowerCase();
    const isSupportedByExtension =
      normalizedName.endsWith('.pdf') || normalizedName.endsWith('.doc') || normalizedName.endsWith('.docx');
    const isSupportedByMime = this.acceptedMimeTypes.has(file.type);

    if (!isSupportedByMime && !isSupportedByExtension) {
      this.selectedFileName.set('');
      this.selectedFile = null;
      this.fileError.set(this.text().form.invalidFileError);
      inputElement.value = '';
      return;
    }

    this.selectedFile = file;
    this.selectedFileName.set(file.name);
  }

  onJdInput(value: string): void {
    this.jdInput.set(value);
    if (this.jdError()) {
      this.jdError.set('');
    }
    this.showResult.set(false);
    this.apiError.set('');
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

    if (!this.selectedFile) {
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

    this.fileError.set('');
    this.jdError.set('');
    this.consentError.set('');
    this.apiError.set('');
    this.isLoading.set(true);
    this.showResult.set(false);

    this.resumeCheckerService.analyze(this.selectedFile!, jdValue).subscribe({
      next: (result) => {
        this.analyzeResult.set(result);
        this.isLoading.set(false);
        this.showResult.set(true);
        setTimeout(() => {
          const element = document.getElementById('result-section');
          if (element) {
            element.scrollIntoView({ behavior: 'smooth', block: 'start' });
          }
        }, 100);
      },
      error: () => {
        this.isLoading.set(false);
        this.apiError.set(this.text().form.apiError);
      }
    });
  }

  toggleFaq(index: number): void {
    this.openedFaqIndex.set(this.openedFaqIndex() === index ? null : index);
  }
}
