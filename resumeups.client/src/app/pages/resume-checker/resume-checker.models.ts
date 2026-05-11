export interface MatchingResult {
  matchScore: number;
  keywordsMatching: string[];
  keywordsMissing: string[];
}

export interface FeedbackResult {
  feedbackScore: number;
  summary: string;
  issues: string[];
  suggestions: string[];
}

export interface AnalyzeResult {
  matching: MatchingResult;
  feedback: FeedbackResult;
}
