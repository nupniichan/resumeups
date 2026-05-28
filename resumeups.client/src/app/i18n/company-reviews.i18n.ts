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
    indeed: string;
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
    basedOnCount: string;
  };
  disclaimerBefore: string;
  disclaimerLinkText: string;
  disclaimerAfter: string;
}

export const companyReviewsTranslations: LanguageDictionary<CompanyReviewsTranslation> = {
  en: {
    hero: {
      title: 'Company Review Insights',
      description: 'Search for any company to instantly fetch reviews, ratings, pros, cons, and AI-summarized insights across multiple local platforms.'
    },
    form: {
      label: 'Search Company',
      placeholder: 'Enter company name (e.g. Google, Facebook, Amazon)...',
      submitButton: 'Get Reviews',
      loadingButton: 'Scraping & Summarizing...'
    },
    tabs: {
      note8: 'Note8.vn',
      reviewcongty: 'ReviewCongTy.vn',
      indeed: 'Indeed.com'
    },
    result: {
      visitWebsite: 'Visit official website',
      ratingCircleLabel: 'Average Rating Score',
      summaryLabel: 'Synthesis & Summary ( AI )',
      prosLabel: 'Key Strengths (Pros)',
      consLabel: 'Key Concerns (Cons)',
      recommendationsLabel: 'Management Advice & Suggestions',
      emptyStateTitle: 'No Reviews Found on this Platform',
      emptyStateDesc: 'We searched extensively but could not find matching review listings for this company on this specific site :(',
      basedOnCount: 'Based on {count} reviews'
    },
    disclaimerBefore: 'Reviews are aggregated from third-party platforms and are for reference only. ResumeUps is not responsible for any review content. See our ',
    disclaimerLinkText: 'Terms of Service',
    disclaimerAfter: ' for details.'
  },
  vi: {
    hero: {
      title: 'Xem Đánh Giá Công Ty',
      description: 'Tìm kiếm công ty để xem tổng hợp đánh giá, chấm điểm, ưu nhược điểm được tóm tắt bằng AI từ nhiều nền tảng hàng đầu.'
    },
    form: {
      label: 'Tìm kiếm doanh nghiệp',
      placeholder: 'Nhập tên công ty (ví dụ: Google, Facebook, Amazon)...',
      submitButton: 'Xem đánh giá',
      loadingButton: 'Đang lấy dữ liệu & Tổng hợp...'
    },
    tabs: {
      note8: 'Note8.vn',
      reviewcongty: 'ReviewCongTy.vn',
      indeed: 'Indeed.com'
    },
    result: {
      visitWebsite: 'Ghé thăm website chính thức',
      ratingCircleLabel: 'Điểm đánh giá trung bình',
      summaryLabel: 'Tóm tắt & Tổng hợp bằng AI',
      prosLabel: 'Ưu điểm',
      consLabel: 'Nhược điểm',
      recommendationsLabel: 'Lời khuyên',
      emptyStateTitle: 'Chưa có đánh giá trên nền tảng này',
      emptyStateDesc: 'Chúng tôi đã tra cứu kỹ lưỡng nhưng hiện tại chưa thấy thông tin đánh giá của công ty trên trang web này :(',
      basedOnCount: 'Dựa trên {count} đánh giá'
    },
    disclaimerBefore: 'Các đánh giá được tổng hợp từ các nền tảng bên thứ ba và chỉ mang tính tham khảo. ResumeUps không chịu trách nhiệm về nội dung đánh giá. Xem ',
    disclaimerLinkText: 'Điều khoản dịch vụ',
    disclaimerAfter: ' để biết thêm.'
  }
};
