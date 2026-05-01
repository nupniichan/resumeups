import { Routes } from '@angular/router';
import { HomePage } from './pages/home/home';
import { ResumeCheckerPage } from './pages/resume-checker/resume-checker';
import { TermsOfServicePage } from './pages/terms-of-service/terms-of-service';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomePage },
  { path: 'resume-checker', component: ResumeCheckerPage },
  { path: 'terms-of-service', component: TermsOfServicePage }
];
