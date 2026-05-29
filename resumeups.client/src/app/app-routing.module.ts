import { Routes } from '@angular/router';
import { HomePage } from './pages/home/home';
import { ResumeCheckerPage } from './pages/resume-checker/resume-checker';
import { TermsOfServicePage } from './pages/terms-of-service/terms-of-service';
import { CompanyReviewsPage } from './pages/company-reviews/company-reviews';
import { NotFoundPage } from './pages/errors/not-found/not-found';
import { ServerErrorPage } from './pages/errors/server-error/server-error';
import { ServiceUnavailablePage } from './pages/errors/service-unavailable/service-unavailable';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomePage },
  { path: 'resume-checker', component: ResumeCheckerPage },
  { path: 'terms-of-service', component: TermsOfServicePage },
  { path: 'company-reviews', component: CompanyReviewsPage },
  { path: 'error/404', component: NotFoundPage },
  { path: 'error/500', component: ServerErrorPage },
  { path: 'error/503', component: ServiceUnavailablePage },
  { path: '**', redirectTo: 'error/404' }
];
