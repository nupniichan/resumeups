import { CommonModule, DecimalPipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ScrollRevealDirective } from '../home/reveal.directive';
import { LanguageService } from '../../core/i18n/language.service';
import { companyReviewsTranslations } from '../../i18n/company-reviews.i18n';
import { CompanyReviewsService } from './company-reviews.service';
import { CompanyReviewResult, ReviewStats } from './company-reviews.models';

@Component({
  selector: 'app-company-reviews',
  standalone: true,
  imports: [CommonModule, FormsModule, DecimalPipe, ScrollRevealDirective, RouterLink],
  templateUrl: './company-reviews.html'
})
export class CompanyReviewsPage {
  private readonly languageService = inject(LanguageService);
  private readonly reviewsService = inject(CompanyReviewsService);

  readonly text = computed(() => companyReviewsTranslations[this.languageService.currentLanguage()]);

  // States
  readonly searchQuery = signal<string>('');
  readonly isLoading = signal<boolean>(false);
  readonly showResult = signal<boolean>(false);
  readonly apiError = signal<string | null>(null);

  readonly reviewResult = signal<CompanyReviewResult | null>(null);
  readonly activeTab = signal<string>('note8');

  // SVG progress circle constant
  readonly circumference = 2 * Math.PI * 52; // roughly 326.72

  // Computed properties
  readonly activePlatform = computed<ReviewStats | null>(() => {
    const result = this.reviewResult();
    if (!result) return null;

    switch (this.activeTab()) {
      case 'note8':
        return result.note8 || null;
      case 'reviewcongty':
        return result.reviewCongTy || null;
      case 'indeed':
        return result.indeed || null;
      default:
        return null;
    }
  });

  readonly resolvedLogoUrl = computed<string>(() => {
    const result = this.reviewResult();
    if (!result) return '';

    const active = this.activePlatform();
    if (active?.logoUrl) return active.logoUrl;

    return result.note8?.logoUrl || result.reviewCongTy?.logoUrl || '';
  });

  readonly ratingScore = computed<number>(() => {
    const platform = this.activePlatform();
    return platform?.rating || 0;
  });

  readonly scoreCircumference = computed<string>(() => {
    return `${this.circumference}`;
  });

  readonly scoreStrokeDashoffset = computed<string>(() => {
    const score = this.ratingScore();
    const percent = Math.min(Math.max(score / 5, 0), 1);
    return `${this.circumference * (1 - percent)}`;
  });

  onSearchQueryInput(val: string): void {
    this.searchQuery.set(val);
  }

  setActiveTab(tab: string): void {
    this.activeTab.set(tab);
  }

  onSubmit(): void {
    const query = this.searchQuery().trim();
    if (!query) return;

    this.isLoading.set(true);
    this.apiError.set(null);
    this.showResult.set(false);

    this.reviewsService.search(query, this.languageService.currentLanguage()).subscribe({
      next: (res) => {
        this.reviewResult.set(res);
        this.showResult.set(true);
        this.isLoading.set(false);

        if (res.indeed?.found) {
          this.activeTab.set('indeed');
        } else if (res.note8?.found) {
          this.activeTab.set('note8');
        } else if (res.reviewCongTy?.found) {
          this.activeTab.set('reviewcongty');
        } else {
          this.activeTab.set('indeed');
        }

        setTimeout(() => {
          const el = document.getElementById('reviews-result-section');
          if (el) {
            el.scrollIntoView({ behavior: 'smooth', block: 'start' });
          }
        }, 150);
      },
      error: (err) => {
        console.error('Reviews search error:', err);
        this.apiError.set(
          this.languageService.currentLanguage() === 'vi'
            ? 'Đã xảy ra lỗi khi tải đánh giá công ty. Vui lòng thử lại.'
            : 'Failed to retrieve company reviews. Please try again.'
        );
        this.isLoading.set(false);
      }
    });
  }
}
