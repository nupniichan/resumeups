import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, switchMap } from 'rxjs';
import { AnalyzeResult } from './resume-checker.models';

@Injectable({ providedIn: 'root' })
export class ResumeCheckerService {
  private readonly http = inject(HttpClient);

  analyze(file: File, jobDescription: string): Observable<AnalyzeResult> {
    return this.extractResume(file).pipe(
      switchMap(maskedText => this.requestAnalysis(maskedText, jobDescription))
    );
  }

  extractResume(file: File): Observable<string> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post('/api/resume-extractions/extract', formData, { responseType: 'text' });
  }

  requestAnalysis(resume: string, jobDescription: string, language: string = 'vi'): Observable<AnalyzeResult> {
    return this.http.post<AnalyzeResult>('/api/resume-analyses/analyze', { resume, jobDescription, language });
  }
}
