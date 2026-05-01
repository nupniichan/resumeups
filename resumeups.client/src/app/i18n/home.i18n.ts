import { LanguageDictionary } from '../core/i18n/language.types';

interface HomeTranslation {
  hero: {
    titlePrefix: string;
    titleBrandNormal: string;
    titleBrandBold: string;
    description: string;
    primaryCta: string;
    secondaryCta: string;
  };
  problemSolution: {
    title: string;
    description: string;
    problemTitle: string;
    problemItems: string[];
    solutionTitle: string;
    solutionItems: string[];
  };
  howItWorks: {
    title: string;
    description: string;
    steps: Array<{ title: string; description: string }>;
  };
  features: {
    title: string;
    description: string;
    items: Array<{ title: string; description: string }>;
  };
  finalCta: {
    title: string;
    description: string;
    primaryCta: string;
    secondaryCta: string;
  };
}

export const homeTranslations: LanguageDictionary<HomeTranslation> = {
  en: {
    hero: {
      titlePrefix: 'Increase chances of getting an interview with',
      titleBrandNormal: 'Resume',
      titleBrandBold: 'Ups',
      description:
        'ResumeUps checks the fit between your CV and the job opening, evaluates it, and provides tailored suggestions for you.',
      primaryCta: 'Check Now',
      secondaryCta: 'See How It Works'
    },
    problemSolution: {
      title: 'Know exactly why your CV is a fit or not yet a fit',
      description:
        'ResumeUps focuses on the core issue: helping you improve the fit for each specific position using AI and actionable suggestions.',
      problemTitle: 'Are you facing these issues?',
      problemItems: [
        'You don’t know if your CV truly fits the job description or not.',
        'You don’t receive feedback from manual reviews, or the feedback is unclear.',
        'You don’t know which areas to prioritize for improvement first.'
      ],
      solutionTitle: 'The Solution',
      solutionItems: [
        'ResumeUps directly compares your CV with the target position.',
        'You receive a clear match score along with detailed analysis.',
        'Suggestions to edit your CV to increase your chances of being chosen as the "best fit".'
      ]
    },
    howItWorks: {
      title: 'How It Works',
      description: 'A simple process focused on fit, score clarity, and next-step improvement suggestions.',
      steps: [
        {
          title: 'Submit CV and target position',
          description: 'Send ResumeUps your CV and the job description you are aiming for.'
        },
        {
          title: 'Receive feedback',
          description: 'Get feedback from AI regarding your fit with the required position.'
        },
        {
          title: 'Apply suggestions and improve',
          description: 'Apply AI suggestions, update your CV, and re-check for better results.'
        }
      ]
    },
    features: {
      title: 'ResumeUps features',
      description: 'Every feature is designed to help you understand your fit and improve faster.',
      items: [
        {
          title: 'Compare CV with job openings',
          description: 'Analyze the fit between your CV content and the job requirements.'
        },
        {
          title: 'Clear match score',
          description: 'Receive a clear match score to quickly measure your CV’s compatibility.'
        },
        {
          title: 'Identify gaps',
          description: 'Pinpoint missing skills, weak keywords, and low-coverage sections.'
        },
        {
          title: 'Actionable AI suggestions',
          description: 'Receive direct, practical suggestions that you can apply immediately.'
        },
        {
          title: 'Guidance for each position',
          description: 'Feedback is adjusted according to the specific job description you are targeting.'
        },
        {
          title: 'Fast evaluation cycle',
          description: 'Update your CV and re-evaluate in just a few seconds.'
        }
      ]
    },
    finalCta: {
      title: 'So what are you waiting for? Try it now! :D',
      description: 'Compare your CV with the job opening and get clear suggestions.',
      primaryCta: 'Check CV Fit',
      secondaryCta: 'Learn More'
    }
  },
  vi: {
    hero: {
      titlePrefix: 'Tăng tỷ lệ được gọi phỏng vấn cùng',
      titleBrandNormal: 'Resume',
      titleBrandBold: 'Ups',
      description:
        'ResumeUps sẽ kiểm tra mức độ phù hợp giữa CV và vị trí cần tuyển, đánh giá, và đưa ra gợi ý cho bạn.',
      primaryCta: 'Kiểm tra ngay',
      secondaryCta: 'Xem cách hoạt động'
    },
    problemSolution: {
      title: 'Biết rõ vì sao CV của bạn phù hợp hoặc chưa phù hợp',
      description:
        'ResumeUps sẽ tập trung vào giải quyết vấn đề chính là giúp bạn cải thiện độ phù hợp cho từng vị trí cụ thể bằng cách sử dụng AI và đưa ra gợi ý.',
      problemTitle: 'Bạn có đang gặp các vấn đề sau?',
      problemItems: [
        'Bạn không biết CV của bạn có thực sự phù hợp với mô tả công việc hay không.',
        'Bạn không nhận được phản hồi từ đánh giá thủ công hoặc phản hồi không rõ ràng.',
        'Bạn không biết nên ưu tiên cải thiện mức độ nào trước.'
      ],
      solutionTitle: 'Giải pháp',
      solutionItems: [
        'ResumeUps sẽ so sánh trực tiếp CV của bạn với vị trí cần tuyển.',
        'Bạn sẽ nhận được điểm phù hợp rõ ràng kèm phân tích.',
        'Gợi ý bạn sửa để tăng tỷ lệ được chọn làm người "phù hợp nhất".'
      ]
    },
    howItWorks: {
      title: 'Cách hoạt động',
      description: 'Quy trình đơn giản, tập trung vào độ phù hợp, độ rõ ràng của điểm và gợi ý cải thiện tiếp theo.',
      steps: [
        {
          title: 'Gửi CV và vị trí cần tuyển',
          description: 'Gửi ResumeUps CV của bạn và mô tả công việc mà bạn đang nhắm tới.'
        },
        {
          title: 'Nhận phản hồi',
          description: 'Nhận phản hồi từ AI về độ phù hợp với vị trí cần tuyển.'
        },
        {
          title: 'Áp dụng gợi ý và cải thiện',
          description: 'Áp dụng gợi ý của AI, cập nhật CV và kiểm tra lại để có kết quả tốt hơn.'
        }
      ]
    },
    features: {
      title: 'Các tính năng của ResumeUps',
      description: 'Mọi tính năng đều hướng đến việc giúp bạn hiểu độ phù hợp và cải thiện nhanh hơn.',
      items: [
        {
          title: 'So sánh CV với vị trí cần tuyển',
          description: 'Phân tích độ phù hợp giữa nội dung CV và yêu cầu vị trí cần tuyển.'
        },
        {
          title: 'Điểm phù hợp rõ ràng',
          description: 'Nhận điểm phù hợp rõ ràng để đo lường nhanh mức độ phù hợp của CV.'
        },
        {
          title: 'Chỉ ra khoảng trống',
          description: 'Xác định kỹ năng thiếu, từ khóa yếu, và mức độ bao phủ thấp.'
        },
        {
          title: 'Gợi ý AI có thể hành động',
          description: 'Nhận gợi ý trực tiếp có thể áp dụng.'
        },
        {
          title: 'Hướng dẫn theo từng vị trí',
          description: 'Phản hồi được điều chỉnh theo mô tả công việc mà bạn đang hướng đến.'
        },
        {
          title: 'Vòng lặp đánh giá nhanh',
          description: 'Cập nhật CV và đánh giá lại chỉ trong vài giây.'
        }
      ]
    },
    finalCta: {
      title: 'Vậy bạn còn chờ gì nữa? Thử ngay thôi :D',
      description: 'So sánh CV với vị trí cần tuyển và nhận gợi ý rõ ràng.',
      primaryCta: 'Kiểm tra độ phù hợp CV',
      secondaryCta: 'Tìm hiểu thêm'
    }
  }
};