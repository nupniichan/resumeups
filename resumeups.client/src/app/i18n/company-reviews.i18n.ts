import { LanguageDictionary } from '../core/i18n/language.types';

interface CompanyReviewsTranslation {
  hero: {
    title: string;
    description: string;
  };
  form: {
    label: string;
    placeholder: string;
    submitButton: string;
    loadingButton: string;
  };
  tabs: {
    note8: string;
    reviewcongty: string;
  };
  result: {
    visitWebsite: string;
    ratingCircleLabel: string;
    summaryLabel: string;
    prosLabel: string;
    consLabel: string;
    recommendationsLabel: string;
    emptyStateTitle: string;
    emptyStateDesc: string;
  };
}

export const companyReviewsTranslations: LanguageDictionary<CompanyReviewsTranslation> = {
  en: {
    hero: {
      title: 'Company Review Insights',
      description: 'Search for any company to instantly fetch reviews, ratings, pros, cons, and AI-summarized insights across multiple local platforms.'
    },
    form: {
      label: 'Search Company',
      placeholder: 'Enter company name (e.g. FPT, VNG, EY)...',
      submitButton: 'Get Reviews',
      loadingButton: 'Scraping & Summarizing...'
    },
    tabs: {
      note8: 'Note8.vn Reviews',
      reviewcongty: 'ReviewCongTy.vn'
    },
    result: {
      visitWebsite: 'Visit official website',
      ratingCircleLabel: 'Average Rating Score',
      summaryLabel: 'AI Synthesis & Summary',
      prosLabel: 'Key Strengths (Pros)',
      consLabel: 'Key Concerns (Cons)',
      recommendationsLabel: 'Management Advice & Recommendations',
      emptyStateTitle: 'No Reviews Found on this Platform',
      emptyStateDesc: 'We searched extensively but could not find matching review listings for this company on this specific site.'
    }
  },
  vi: {
    hero: {
      title: 'Tra Cứu Đánh Giá Công Ty',
      description: 'Tìm kiếm bất kỳ công ty nào để xem tổng hợp đánh giá, chấm điểm, ưu nhược điểm được tóm tắt bằng AI từ nhiều nền tảng hàng đầu.'
    },
    form: {
      label: 'Tìm kiếm doanh nghiệp',
      placeholder: 'Nhập tên công ty (ví dụ: FPT, VNG, EY)...',
      submitButton: 'Xem đánh giá',
      loadingButton: 'Đang cào & Tổng hợp...'
    },
    tabs: {
      note8: 'Note8.vn Đánh giá',
      reviewcongty: 'ReviewCongTy.vn'
    },
    result: {
      visitWebsite: 'Ghé thăm website chính thức',
      ratingCircleLabel: 'Điểm đánh giá trung bình',
      summaryLabel: 'Tóm tắt & Tổng hợp bằng AI',
      prosLabel: 'Ưu điểm nổi bật',
      consLabel: 'Nhược điểm cần lưu ý',
      recommendationsLabel: 'Lời khuyên cho ứng viên & Ban quản lý',
      emptyStateTitle: 'Chưa có đánh giá trên nền tảng này',
      emptyStateDesc: 'Chúng tôi đã tra cứu kỹ lưỡng nhưng hiện tại chưa thấy thông tin đánh giá của công ty trên trang web này.'
    }
  }
};
