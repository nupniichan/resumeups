import { Routes } from '@angular/router';
import { HomePage } from './pages/home/home';
import { ResumeCheckerPage } from './pages/resume-checker/resume-checker';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomePage },
  { path: 'resume-checker', component: ResumeCheckerPage }
];
