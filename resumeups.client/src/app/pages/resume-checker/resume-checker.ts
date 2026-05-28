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
  readonly selectedFileName = signal(localStorage.getItem('resumeups_file_name') || '');
  readonly jdInput = signal(localStorage.getItem('resumeups_jd_input') || '');
  readonly selectedLanguage = signal(localStorage.getItem('resumeups_selected_language') || 'auto');
  readonly fileError = signal('');
  readonly jdError = signal('');
  readonly consentError = signal('');
  readonly termsAccepted = signal(false);
  readonly analyzeResult = signal<AnalyzeResult | null>(
    (() => {
      try {
        const cached = localStorage.getItem('resumeups_analyze_result');
        return cached ? JSON.parse(cached) : null;
      } catch {
        return null;
      }
    })()
  );
  readonly showResult = signal(!!this.analyzeResult());
  readonly isLoading = signal(false);
  readonly apiError = signal('');
  readonly openedFaqIndex = signal<number | null>(0);
  readonly isDragging = signal(false);
  readonly isVi = computed(() => this.languageService.currentLanguage() === 'vi');

  private selectedFile: File | null = null;
  private cachedExtractedText = localStorage.getItem('resumeups_extracted_text') || '';

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
    const file = inputElement.files?.[0] || null;
    this.processFile(file, inputElement);
  }

  processFile(file: File | null, inputElement?: HTMLInputElement): void {
    this.fileError.set('');
    this.showResult.set(false);
    this.apiError.set('');
    this.analyzeResult.set(null);
    try {
      localStorage.removeItem('resumeups_analyze_result');
    } catch { }

    if (!file) {
      this.selectedFileName.set('');
      this.selectedFile = null;
      this.cachedExtractedText = '';
      try {
        localStorage.removeItem('resumeups_extracted_text');
        localStorage.removeItem('resumeups_file_name');
      } catch { }
      return;
    }

    const normalizedName = file.name.toLowerCase();
    const isSupportedByExtension =
      normalizedName.endsWith('.pdf') || normalizedName.endsWith('.doc') || normalizedName.endsWith('.docx');
    const isSupportedByMime = this.acceptedMimeTypes.has(file.type);

    if (!isSupportedByMime && !isSupportedByExtension) {
      this.selectedFileName.set('');
      this.selectedFile = null;
      this.cachedExtractedText = '';
      try {
        localStorage.removeItem('resumeups_extracted_text');
        localStorage.removeItem('resumeups_file_name');
      } catch { }
      this.fileError.set(this.text().form.invalidFileError);
      if (inputElement) {
        inputElement.value = '';
      }
      return;
    }

    this.selectedFile = file;
    this.selectedFileName.set(file.name);
    this.cachedExtractedText = '';
    try {
      localStorage.removeItem('resumeups_extracted_text');
    } catch { }

    if (inputElement) {
      inputElement.value = '';
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging.set(true);
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging.set(false);
  }

  onDrop(event: DragEvent, fileInput: HTMLInputElement): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging.set(false);

    const file = event.dataTransfer?.files?.[0] || null;
    if (file) {
      this.processFile(file, fileInput);
    }
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

  onLanguageChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    this.selectedLanguage.set(select.value);
    try {
      localStorage.setItem('resumeups_selected_language', select.value);
    } catch { }
  }

  onSubmit(): void {
    const jdValue = this.jdInput().trim();
    let isValid = true;

    if (!this.selectedFile && !this.cachedExtractedText) {
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

    if (this.selectedFile) {
      this.resumeCheckerService.extractResume(this.selectedFile).subscribe({
        next: (extractedText) => {
          this.cachedExtractedText = extractedText;
          try {
            localStorage.setItem('resumeups_extracted_text', extractedText);
            localStorage.setItem('resumeups_file_name', this.selectedFileName());
          } catch (e) {
            console.warn('Failed to save extracted text to localStorage', e);
          }
          this.performAnalysis(extractedText, jdValue);
        },
        error: () => this.handleError()
      });
    } else if (this.cachedExtractedText) {
      this.performAnalysis(this.cachedExtractedText, jdValue);
    }
  }

  private performAnalysis(resumeText: string, jobDescription: string): void {
    this.resumeCheckerService.requestAnalysis(resumeText, jobDescription, this.selectedLanguage()).subscribe({
      next: (result) => {
        try {
          localStorage.setItem('resumeups_analyze_result', JSON.stringify(result));
          localStorage.setItem('resumeups_jd_input', jobDescription);
        } catch (e) {
          console.warn('Failed to save analysis result to localStorage', e);
        }

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
      error: () => this.handleError()
    });
  }

  private handleError(): void {
    this.isLoading.set(false);
    this.apiError.set(this.text().form.apiError);
  }

  toggleFaq(index: number): void {
    this.openedFaqIndex.set(this.openedFaqIndex() === index ? null : index);
  }
}
