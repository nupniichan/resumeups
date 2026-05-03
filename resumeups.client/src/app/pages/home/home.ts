import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  computed,
  inject
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { gsap } from 'gsap';
import { ScrollTrigger } from 'gsap/ScrollTrigger';
import { LanguageService } from '../../core/i18n/language.service';
import { homeTranslations } from '../../i18n/home.i18n';
import { ScrollRevealDirective } from './reveal.directive';

interface HiwDemoFeedbackIssue {
  category: string;
  severity: string;
  issue: string;
  suggestion: string;
}

interface HiwDemoFeedback {
  feedback_score: number;
  summary: string;
  issues: HiwDemoFeedbackIssue[];
}

interface HiwDemoMatching {
  match_score: number;
  keywords_matching: string[];
  keywords_missing: string[];
}

interface HiwAnalysisDemo {
  matching: HiwDemoMatching;
  feedback: HiwDemoFeedback;
}

const HIW_ANALYSIS_DEMO: HiwAnalysisDemo = {
  matching: {
    match_score: 72,
    keywords_matching: ['Angular', 'TypeScript', 'C#', '.NET', 'ASP.NET Core'],
    keywords_missing: ['AWS', 'ECS', 'Lambda']
  },
  feedback: {
    feedback_score: 78,
    summary:
      'Clear overlap on the Angular + ASP.NET Core side of the posting, but the JD weights hands-on AWS delivery (networking, IAM, observability, and shipping to ECS/Lambda). Right now cloud reads like a checklist rather than owned outcomes tied to your .NET services and Angular apps.',
    issues: [
      {
        category: 'Keyword coverage',
        severity: 'High',
        issue:
          'AWS surfaces mostly as a skills line while the job asks for concrete ECS/Lambda experience and how you operated .NET workloads in AWS (deployments, scaling, alarms, cost).',
        suggestion:
          'Move proof into experience bullets—e.g. “Hardened an ASP.NET Core API behind ALB + WAF, shipped containers to ECS Fargate, and wired CloudWatch alarms for p95 latency and 5xx budgets.” Avoid burying AWS next to unrelated keywords.'
      },
      {
        category: 'Impact & evidence',
        severity: 'Medium',
        issue:
          'Angular feature work is described qualitatively; fewer bullets quantify user impact, release cadence, or reliability improvements for the services your UI calls.',
        suggestion:
          'Pair UI wins with backend signals: reduced incident volume, faster deployments after pipeline changes, adoption of a new module, or API latency/availability targets your team tracked.'
      }
    ]
  }
};

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, ScrollRevealDirective],
  templateUrl: './home.html'
})
export class HomePage implements AfterViewInit, OnDestroy {
  private readonly languageService = inject(LanguageService);
  private readonly host = inject(ElementRef<HTMLElement>);
  private scrollTriggers: ScrollTrigger[] = [];

  readonly text = computed(() => homeTranslations[this.languageService.currentLanguage()]);
  readonly hiwAnalysisDemo = HIW_ANALYSIS_DEMO;

  ngAfterViewInit(): void {
    gsap.registerPlugin(ScrollTrigger);
    queueMicrotask(() => this.initScrollAnimations());
  }

  ngOnDestroy(): void {
    this.scrollTriggers.forEach((st) => st.kill());
    this.scrollTriggers = [];
  }

  private initScrollAnimations(): void {
    const root = this.host.nativeElement;

    const step1Panel = root.querySelector('.hiw-step-1-panel') as HTMLElement | null;
    const step2Panel = root.querySelector('.hiw-step-2-panel') as HTMLElement | null;
    const step3Panel = root.querySelector('.hiw-step-3-panel') as HTMLElement | null;
    const featuresSection = root.querySelector('.features-scroll-section') as HTMLElement | null;

    if (step1Panel) this.buildStep1(step1Panel);
    if (step2Panel) this.buildStep2(step2Panel);
    if (step3Panel) this.buildStep3(step3Panel);
    if (featuresSection) this.buildFeatures(featuresSection);

    ScrollTrigger.refresh();
  }

  private register(st: ScrollTrigger): void {
    this.scrollTriggers.push(st);
  }

  private buildStep1(panel: HTMLElement): void {
    const dragGroup = panel.querySelector('.hiw-drag-group') as HTMLElement | null;
    const cv = panel.querySelector('.hiw-cv-chip') as HTMLElement | null;
    const cursor = panel.querySelector('.hiw-cursor') as HTMLElement | null;
    const dropZone = panel.querySelector('.hiw-drop-zone') as HTMLElement | null;
    const jdField = panel.querySelector('.hiw-jd-field') as HTMLElement | null;
    const jdText = panel.querySelector('.hiw-jd-text') as HTMLElement | null;
    const submitBtn = panel.querySelector('.hiw-submit-btn') as HTMLElement | null;
    const dropCaption = panel.querySelector('.hiw-drop-caption') as HTMLElement | null;

    const jdFull =
      'Senior Software Engineer · .NET & Angular\n\nBuild and evolve customer-facing Angular apps backed by ASP.NET Core APIs on AWS, working closely with Product and platform engineers.\n\nMust-have:\n- 4+ years shipping production SPAs in Angular (modern TypeScript) and APIs in C# / .NET\n- Solid ASP.NET Core fundamentals: auth, validation, performance, and clean service boundaries\n- AWS experience operating .NET workloads (ECS/EKS or Lambda + API Gateway), IAM least-privilege patterns, and observability (CloudWatch/X-Ray)\n- Pragmatic ownership during incidents; clear written communication for design and rollout decisions';

    if (!dragGroup || !cv || !cursor || !dropZone || !jdField || !jdText || !submitBtn || !dropCaption)
      return;

    const cursorRect = cursor.getBoundingClientRect();
    const jdRect = jdText.getBoundingClientRect();
    const submitRect = submitBtn.getBoundingClientRect();

    const cursorAnchorX = cursorRect.left + cursorRect.width * 0.35;
    const cursorAnchorY = cursorRect.top + cursorRect.height * 0.4;
    const jdFocusX = jdRect.left + Math.min(jdRect.width * 0.15, 90);
    const jdFocusY = jdRect.top + 28;
    const submitFocusX = submitRect.left + submitRect.width * 0.5;
    const submitFocusY = submitRect.top + submitRect.height * 0.65;

    const cursorToJdX = jdFocusX - cursorAnchorX;
    const cursorToJdY = jdFocusY - cursorAnchorY;
    const cursorToSubmitX = submitFocusX - cursorAnchorX;
    const cursorToSubmitY = submitFocusY - cursorAnchorY;

    gsap.set(dragGroup, { y: 130, opacity: 0 });
    gsap.set(cv, { scale: 0.94 });
    gsap.set(cursor, { opacity: 1, scale: 1, x: 0, y: 0, transformOrigin: '35% 40%' });
    gsap.set(dropZone, { borderColor: '#c6d8ea', boxShadow: '0 0 0 0 rgba(79,143,184,0)' });
    gsap.set(dropCaption, { autoAlpha: 0, y: 6 });
    gsap.set(jdField, { opacity: 1, y: 0 });
    jdText.textContent = '';
    gsap.set(submitBtn, { scale: 1, boxShadow: '0 0 0 0 rgba(26,54,86,0)' });

    const tl = gsap.timeline({ defaults: { ease: 'power2.inOut' } });

    tl.to(dragGroup, { opacity: 1, duration: 0.1, ease: 'none' }, 0)
      .to(dragGroup, { y: 0, duration: 0.38 }, 0.04)
      .to(dropZone, { borderColor: '#4f8fb8', boxShadow: '0 8px 28px rgba(79,143,184,0.18)', duration: 0.15 }, 0.3)
      .to(cursor, { scale: 0.88, duration: 0.06, yoyo: true, repeat: 1 }, 0.36)
      .to(cv, { scale: 0.97, duration: 0.08 }, 0.36)
      .to(dropCaption, { autoAlpha: 1, y: 0, duration: 0.22, ease: 'power2.out' }, 0.36)
      .to(dragGroup, { opacity: 0.72, y: 10, duration: 0.22, ease: 'power2.in' }, 0.46)
      .to(cursor, { x: cursorToJdX, y: cursorToJdY, duration: 0.2, ease: 'power2.out' }, 0.54)
      .to(
        {},
        {
          duration: 0.32,
          ease: 'none',
          onUpdate() {
            const p = this['progress']();
            const n = Math.max(0, Math.floor(p * jdFull.length));
            jdText.textContent = jdFull.slice(0, n);
          }
        },
        0.72
      )
      .to(cursor, { scale: 0.92, duration: 0.04, yoyo: true, repeat: 5, ease: 'power1.inOut' }, 0.78)
      .to(cursor, { x: cursorToSubmitX, y: cursorToSubmitY, scale: 1, duration: 0.2, ease: 'power2.inOut' }, 1.06)
      .to(submitBtn, { boxShadow: '0 0 0 3px rgba(79,143,184,0.35)', duration: 0.12 }, 1.18)
      .to(cursor, { scale: 0.82, duration: 0.07, yoyo: true, repeat: 1, ease: 'power2.inOut' }, 1.24)
      .to(submitBtn, { scale: 0.95, duration: 0.07, yoyo: true, repeat: 1 }, 1.25)
      .to(submitBtn, { boxShadow: '0 0 0 0 rgba(79,143,184,0)', duration: 0.15 }, 1.35)
      .to(cursor, { opacity: 0, duration: 0.12 }, 1.4);

    const st = ScrollTrigger.create({
      trigger: panel,
      start: 'top top',
      end: '+=220%',
      pin: true,
      scrub: 1,
      animation: tl,
      anticipatePin: 1
    });
    this.register(st);
  }

  private buildStep2(panel: HTMLElement): void {
    const bar = panel.querySelector('.hiw-progress-bar') as HTMLElement | null;
    const spin = panel.querySelector('.hiw-ai-spinner') as HTMLElement | null;
    const shimmer = panel.querySelector('.hiw-shimmer-card') as HTMLElement | null;
    const stream = panel.querySelector('.hiw-stream-pre') as HTMLElement | null;

    if (!bar || !spin || !shimmer || !stream) return;

    const fullStream = `${JSON.stringify(HIW_ANALYSIS_DEMO.matching, null, 2)}\n\n${JSON.stringify(HIW_ANALYSIS_DEMO.feedback, null, 2)}`;

    gsap.set(bar, { scaleX: 0, transformOrigin: 'left center' });
    gsap.set(spin, { rotation: 0 });
    gsap.set(shimmer, { backgroundPosition: '200% 0' });
    stream.textContent = '';

    const tl = gsap.timeline({ defaults: { ease: 'none' } });

    tl.to(bar, { scaleX: 1, duration: 0.28, ease: 'power1.inOut' }, 0)
      .to(spin, { rotation: 720, duration: 0.9, ease: 'none' }, 0)
      .to(
        shimmer,
        { backgroundPosition: '-200% 0', duration: 0.85, ease: 'none' },
        0
      )
      .to(
        {},
        {
          duration: 0.9,
          ease: 'none',
          onUpdate() {
            const p = this['progress']();
            const n = Math.max(0, Math.floor(p * fullStream.length));
            stream.textContent = fullStream.slice(0, n);
            stream.scrollTop = stream.scrollHeight;
          }
        },
        0
      );

    const st = ScrollTrigger.create({
      trigger: panel,
      start: 'top top',
      end: '+=200%',
      pin: true,
      scrub: 1,
      animation: tl,
      anticipatePin: 1
    });
    this.register(st);
  }

  private buildStep3(panel: HTMLElement): void {
    const circle = panel.querySelector('.hiw-score-ring') as SVGCircleElement | null;
    const scoreNum = panel.querySelector('.hiw-score-number') as HTMLElement | null;
    const matchChips = panel.querySelectorAll('.hiw-match-chip') as NodeListOf<HTMLElement>;
    const missChips = panel.querySelectorAll('.hiw-miss-chip') as NodeListOf<HTMLElement>;
    const feedbackCard = panel.querySelector('.hiw-feedback-card') as HTMLElement | null;
    const issueRows = panel.querySelectorAll('.hiw-issue-row') as NodeListOf<HTMLElement>;

    if (!circle || !scoreNum || !feedbackCard) return;

    const circumference = 2 * Math.PI * 52;
    circle.style.strokeDasharray = `${circumference}`;
    circle.style.strokeDashoffset = `${circumference}`;

    const targetScore = HIW_ANALYSIS_DEMO.matching.match_score;
    gsap.set(scoreNum, { textContent: '0' });
    gsap.set(matchChips, { opacity: 0, y: 12 });
    gsap.set(missChips, { opacity: 0, y: 12 });
    gsap.set(feedbackCard, { opacity: 0, x: 40 });
    gsap.set(issueRows, { opacity: 0, y: 10 });

    const scoreVal = { n: 0 };
    const tl = gsap.timeline({ defaults: { ease: 'power2.out' } });

    tl.to(
      circle,
      {
        strokeDashoffset: circumference * (1 - targetScore / 100),
        duration: 0.35,
        ease: 'power2.inOut'
      },
      0
    ).to(
      scoreVal,
      {
        n: targetScore,
        duration: 0.35,
        ease: 'power1.out',
        onUpdate: () => {
          scoreNum.textContent = String(Math.round(scoreVal.n));
        }
      },
      0
    )
      .to(matchChips, { opacity: 1, y: 0, duration: 0.18, stagger: 0.06 }, 0.22)
      .to(missChips, { opacity: 1, y: 0, duration: 0.18, stagger: 0.06 }, 0.38)
      .to(feedbackCard, { opacity: 1, x: 0, duration: 0.28 }, 0.5)
      .to(issueRows, { opacity: 1, y: 0, duration: 0.2, stagger: 0.08 }, 0.62);

    const st = ScrollTrigger.create({
      trigger: panel,
      start: 'top top',
      end: '+=200%',
      pin: true,
      scrub: 1,
      animation: tl,
      anticipatePin: 1
    });
    this.register(st);
  }

  private buildFeatures(section: HTMLElement): void {
    const pinWrap = section.querySelector('.features-pin-wrap') as HTMLElement | null;
    const cards = section.querySelectorAll('.feature-card') as NodeListOf<HTMLElement>;

    if (!pinWrap || !cards.length) return;

    gsap.set(cards, { y: 56, opacity: 0, transformOrigin: '50% 50%' });

    const tl = gsap.timeline({ defaults: { ease: 'power3.out' } });
    tl.to(cards, { y: 0, opacity: 1, duration: 0.55, stagger: 0.12 }, 0).to(
      cards,
      {
        borderColor: '#4f8fb8',
        boxShadow: '0 12px 32px rgba(79,143,184,0.12)',
        duration: 0.35,
        stagger: 0.08
      },
      0.25
    );

    const st = ScrollTrigger.create({
      trigger: pinWrap,
      start: 'top top',
      end: '+=160%',
      pin: true,
      scrub: 1,
      animation: tl,
      anticipatePin: 1
    });
    this.register(st);
  }
}
