export interface ReviewStats {
  found: boolean;
  rating?: number;
  logoUrl?: string;
  website?: string;
  summary?: string;
  pros?: string[];
  cons?: string[];
  recommendations?: string[];
}

export interface CompanyReviewResult {
  companyName: string;
  note8?: ReviewStats;
  reviewCongTy?: ReviewStats;
}
