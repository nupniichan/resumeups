import { LanguageDictionary } from '../core/i18n/language.types';

interface ResumeCheckerFaqItem {
  question: string;
  answer: string;
}

interface ResumeCheckerTranslation {
  promo: {
    title: string;
    subtitle: string;
    description: string;
  };
  hero: {
    title: string;
    description: string;
  };
  form: {
    step1Title: string;
    step1Hint: string;
    fileTypesLabel: string;
    uploadSubtext: string;
    chooseFileButton: string;
    selectedFileLabel: string;
    step2Title: string;
    jdLabel: string;
    jdPlaceholder: string;
    submitButton: string;
    invalidFileError: string;
    requiredJdError: string;
    consentLabelPrefix: string;
    consentLabelLink: string;
    consentRequiredError: string;
  };
  result: {
    title: string;
    summary: string;
    matchScoreLabel: string;
    strengthsTitle: string;
    strengths: string[];
    improvementsTitle: string;
    improvements: string[];
  };
  faq: {
    title: string;
    items: ResumeCheckerFaqItem[];
  };
}

export const resumeCheckerTranslations: LanguageDictionary<ResumeCheckerTranslation> = {
  en: {
    promo: {
      title: 'Check Your Resume',
      subtitle: 'And Get a Free Job Tracking Template',
      description: 'ResumeUps evaluate your resume based on how well it matches the target job description.'
    },
    hero: {
      title: 'Comprehensive ATS Resume Checker',
      description: 'Upload your resume, add the target job description, and get an initial ATS-fit result quickly.'
    },
    form: {
      step1Title: 'Step 1: Upload your resume',
      step1Hint: 'Accepted formats: PDF, DOC, DOCX',
      fileTypesLabel: 'Upload your resume',
      uploadSubtext: 'PDF, DOC, DOCX - up to 10 MB',
      chooseFileButton: 'Choose file',
      selectedFileLabel: 'Selected file',
      step2Title: 'Step 2: Paste the job description',
      jdLabel: 'Job description (JD)',
      jdPlaceholder: 'Paste the full job description here...',
      submitButton: 'View result',
      invalidFileError: 'Please upload a valid PDF, DOC, or DOCX file.',
      requiredJdError: 'Please enter a job description before viewing results.',
      consentLabelPrefix: 'I have read and agree to the',
      consentLabelLink: 'Terms of Service',
      consentRequiredError: 'Please read and agree to the Terms of Service before continuing.'
    },
    result: {
      title: 'Result',
      summary: 'This is a sample result, will be updated with the actual result from the API',
      matchScoreLabel: 'Estimated ATS fit',
      strengthsTitle: 'Strengths',
      strengths: [
        'Your resume includes core keywords from the target role.',
        'Work experience section is clear and easy to scan.',
        'Technical skills are listed with good coverage.'
      ],
      improvementsTitle: 'Suggested improvements',
      improvements: [
        'Add more measurable impact for recent projects.',
        'Align role-specific keywords with the JD requirements.',
        'Reorder sections to prioritize the most relevant experience.'
      ]
    },
    faq: {
      title: 'Frequently asked questions',
      items: [
        {
          question: 'Gonna add later',
          answer: 'Im too lazy to add this right now'
        }
      ]
    }
  },
  vi: {
    promo: {
      title: 'Kiểm Tra CV Của Bạn',
      subtitle: 'Và Nhận Ngay Job Tracking Template Miễn Phí',
      description: 'ResumeUps sẽ đánh giá CV của bạn dựa trên mức độ phù hợp với mô tả công việc'
    },
    hero: {
      title: 'Công cụ Kiểm Tra CV Toàn Diện ATS',
      description: 'Tải CV của bạn lên, nhập mô tả công việc và xem kết quả độ phù hợp ATS ban đầu.'
    },
    form: {
      step1Title: 'Bước 1: Tải CV của bạn lên',
      step1Hint: 'Định dạng chấp nhận: PDF, DOC, DOCX',
      fileTypesLabel: 'Tải CV của bạn lên',
      uploadSubtext: 'PDF, DOC, DOCX - tối đa 10 MB',
      chooseFileButton: 'Chọn tệp',
      selectedFileLabel: 'Tệp đã chọn',
      step2Title: 'Bước 2: Nhập mô tả công việc',
      jdLabel: 'Mô tả công việc (JD)',
      jdPlaceholder: 'Dán đầy đủ nội dung JD vào đây...',
      submitButton: 'Xem kết quả',
      invalidFileError: 'Vui lòng tải lên tệp PDF, DOC hoặc DOCX hợp lệ.',
      requiredJdError: 'Vui lòng nhập mô tả công việc trước khi xem kết quả.',
      consentLabelPrefix: 'Tôi đã đọc và đồng ý với',
      consentLabelLink: 'Điều khoản dịch vụ',
      consentRequiredError: 'Vui lòng đọc và đồng ý với Điều khoản dịch vụ trước khi tiếp tục.'
    },
    result: {
      title: 'Kết quả',
      summary: 'Này là bước tạm thôi nào có api tính tiếp',
      matchScoreLabel: 'Mức độ phù hợp',
      strengthsTitle: 'Điểm mạnh',
      strengths: [
        'CV đã chứa các từ khóa cốt lõi của vị trí ứng tuyển.',
        'Phần kinh nghiệm trình bày rõ ràng, dễ quét thông tin.',
        'Kỹ năng chuyên môn được liệt kê với độ bao phủ tốt.'
      ],
      improvementsTitle: 'Gợi ý cải thiện',
      improvements: [
        'Bổ sung số liệu kết quả cho các dự án gần đây.',
        'Điều chỉnh từ khóa sát hơn với yêu cầu trong JD.',
        'Sắp xếp lại thứ tự mục để ưu tiên nội dung liên quan nhất.'
      ]
    },
    faq: {
      title: 'Câu hỏi thường gặp',
      items: [
        {
          question: 'Gonna add later',
          answer: 'Im too lazy to add this right now'
        }
      ]
    }
  }
};
