export interface DemographicRating {
  group: string;
  rating?: number;
}

export interface ReviewStats {
  found: boolean;
  rating?: number;
  logoUrl?: string;
  website?: string;
  reviewsUrl?: string;
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
  diversityAndInclusion?: number;
  demographicRatings?: DemographicRating[];
  reviewTitles?: string[];
}

export interface CompanyReviewResult {
  companyName: string;
  note8?: ReviewStats;
  reviewCongTy?: ReviewStats;
  indeed?: ReviewStats;
  glassdoor?: ReviewStats;
}
