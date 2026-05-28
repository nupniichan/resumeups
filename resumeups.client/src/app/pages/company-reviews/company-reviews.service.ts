import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CompanyReviewResult } from './company-reviews.models';

@Injectable({ providedIn: 'root' })
export class CompanyReviewsService {
  private readonly http = inject(HttpClient);

  search(companyName: string, language: string = 'vi'): Observable<CompanyReviewResult> {
    return this.http.post<CompanyReviewResult>('/api/company-reviews/search', { companyName, language });
  }
}
