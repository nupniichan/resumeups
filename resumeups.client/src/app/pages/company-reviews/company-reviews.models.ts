export interface ReviewStats {
  found: boolean;
  rating?: number;
  logoUrl?: string;
  website?: string;
  summary?: string;
  pros?: string[];
  cons?: string[];
  recommendations?: string[];

  reviewsCount?: string;
  workLifeBalance?: number;
  payAndBenefits?: number;
  jobSecurityAndAdvancement?: number;
  management?: number;
  culture?: number;
  reviewTitles?: string[];
}

export interface CompanyReviewResult {
  companyName: string;
  note8?: ReviewStats;
  reviewCongTy?: ReviewStats;
  indeed?: ReviewStats;
}
