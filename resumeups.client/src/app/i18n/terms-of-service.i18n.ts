import { LanguageDictionary } from '../core/i18n/language.types';

interface TermsSection {
  title: string;
  paragraphs: string[];
}

interface TermsContact {
  title: string;
  description: string;
  emailLabel: string;
  email: string;
}

interface TermsTranslation {
  pageTitle: string;
  pageDescription: string;
  effectiveDateLabel: string;
  effectiveDateValue: string;
  sections: TermsSection[];
  contact: TermsContact;
}

export const termsOfServiceTranslations: LanguageDictionary<TermsTranslation> = {
  en: {
    pageTitle: 'Terms of Service',
    pageDescription:
      'These Terms of Service are a legally binding contract between you and ResumeUps. BY USING THE SERVICE, YOU ACKNOWLEDGE THAT YOU HAVE READ, UNDERSTOOD, AND AGREE TO BE BOUND BY THESE TERMS, INCLUDING THE PII DISCLOSURE AND LIMITATION OF LIABILITY.',
    effectiveDateLabel: 'Last Updated',
    effectiveDateValue: 'May 1, 2026',
    sections: [
      {
        title: '1. Service Nature and Open Source Status',
        paragraphs: [
          'ResumeUps provides a free, open-source AI resume analysis tool. The Service is provided as a guest-only experience; no account registration or login is required.',
          'While the software is provided for free to the public, the underlying infrastructure and API costs are funded privately by the developer. You agree not to abuse this generosity by wasting computing resources.'
        ]
      },
      {
        title: '2. PII Disclosure and Assumption of Risk (IMPORTANT)',
        paragraphs: [
          'Limited Data Anonymization: You acknowledge that the current backend processing of ResumeUps may not fully mask or redact all Personally Identifiable Information (PII). Information such as your full name, email, phone number, and portfolio URLs may be visible during processing, system logging, or AI analysis.',
          'Assumption of Risk: By uploading your resume or job descriptions, you grant express consent for this data to be processed in its current state. You voluntarily assume all risks associated with the potential exposure of PII and agree that ResumeUps and its developer are not liable for any data leaks or unauthorized access resulting from these technical limitations.',
          'Recommendation: ResumeUps strongly advise users to manually redact or remove sensitive information from their documents before submission if they are not comfortable with the aforementioned risks.'
        ]
      },
      {
        title: '3. Prohibited Conduct and Abuse',
        paragraphs: [
          'To protect the Service\'s users and the developer\'s resources, you are STRICTLY PROHIBITED from: (a) performing any form of automated scraping, hacking, or reverse engineering; (b) using scripts or bots to perform bulk analysis; (c) attempting to bypass rate limits or security measures; (d) performing "Red Teaming" or adversarial prompt injections.',
        ]
      },
      {
        title: '4. Disclaimer of Warranties ("AS IS")',
        paragraphs: [
          'THE SERVICE IS PROVIDED "AS IS" WITHOUT ANY WARRANTIES OF ANY KIND. We do not guarantee that the analysis is accurate, error-free, or that it will result in any job offers or interviews.',
          'The Service utilizes third-party AI models which may produce "hallucinations" or incorrect advice. All professional decisions made based on this Service are your sole responsibility.'
        ]
      },
      {
        title: '5. Limitation of Liability',
        paragraphs: [
          'TO THE MAXIMUM EXTENT PERMITTED BY LAW, RESUMEUPS AND ITS DEVELOPER SHALL NOT BE LIABLE FOR ANY DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES, INCLUDING DATA EXPOSURE OR FINANCIAL LOSS, ARISING FROM YOUR USE OF THIS FREE SERVICE.',
          'Since this is a free and open-source project provided without a contract, you agree that your only remedy for dissatisfaction is to stop using the Service.'
        ]
      }
    ],
    contact: {
      title: '6. Contact',
      description: 'For issues regarding these terms or to report abuse, contact:',
      emailLabel: 'Email',
      email: 'support@resumeups.com'
    }
  },
  vi: {
    pageTitle: 'Điều khoản dịch vụ',
    pageDescription:
      'Điều khoản dịch vụ này là một hợp đồng pháp lý ràng buộc giữa bạn và ResumeUps. BẰNG CÁCH SỬ DỤNG DỊCH VỤ, BẠN XÁC NHẬN ĐÃ ĐỌC, HIỂU VÀ ĐỒNG Ý BỊ RÀNG BUỘC BỞI CÁC ĐIỀU KHOẢN NÀY, BAO GỒM CÁC THÔNG BÁO VỀ PII VÀ GIỚI HẠN TRÁCH NHIỆM.',
    effectiveDateLabel: 'Cập nhật lần cuối',
    effectiveDateValue: '01/05/2026',
    sections: [
      {
        title: '1. Bản chất dịch vụ và Trạng thái Mã nguồn mở',
        paragraphs: [
          'ResumeUps cung cấp công cụ phân tích CV bằng AI miễn phí và mã nguồn mở. Dịch vụ được cung cấp dưới dạng trải nghiệm khách (guest); không yêu cầu đăng ký tài khoản hoặc đăng nhập.',
          'Mặc dù phần mềm được cung cấp miễn phí cho cộng đồng, chi phí hạ tầng và API cơ bản được chi trả bởi cá nhân nhà phát triển. Bạn đồng ý không lạm dụng sự hỗ trợ này bằng cách lãng phí tài nguyên máy chủ.'
        ]
      },
      {
        title: '2. Thông báo về PII và Chấp nhận rủi ro (QUAN TRỌNG)',
        paragraphs: [
          'Hạn chế về ẩn danh dữ liệu: Bạn xác nhận rằng hệ thống xử lý hiện tại của ResumeUps có thể chưa che giấu hoàn toàn các Thông tin định danh cá nhân (PII). Các thông tin như họ tên, email, số điện thoại và đường dẫn portfolio có thể xuất hiện trong quá trình xử lý, lưu nhật ký hệ thống (logging) hoặc kết quả phân tích AI.',
          'Chấp nhận rủi ro: Bằng cách tải lên CV hoặc mô tả công việc, bạn cấp quyền rõ ràng cho dữ liệu này được xử lý ở trạng thái hiện tại. Bạn tự nguyện chấp nhận mọi rủi ro liên quan đến việc lộ PII tiềm tàng và đồng ý rằng ResumeUps cũng như nhà phát triển không chịu trách nhiệm cho bất kỳ sự cố lộ dữ liệu nào phát sinh từ các hạn chế kỹ thuật này.',
          'Khuyến nghị: ResumeUps khuyến khích người dùng chủ động lược bỏ hoặc ẩn đi các thông tin nhạy cảm trong tài liệu trước khi gửi nếu bạn không thoải mái với các rủi ro nêu trên.'
        ]
      },
      {
        title: '3. Các hành vi bị nghiêm cấm',
        paragraphs: [
          'Để bảo vệ Dịch vụ và tài nguyên của nhà phát triển, bạn bị NGHIÊM CẤM: (a) thực hiện mọi hình thức thu thập dữ liệu tự động (scraping), hack, hoặc kỹ thuật đảo ngược; (b) sử dụng script hoặc bot để phân tích hàng loạt; (c) cố gắng vượt qua giới hạn truy cập (rate limits) hoặc các biện pháp bảo mật; (d) thực hiện các hành vi tấn công câu lệnh (prompt injection).',
        ]
      },
      {
        title: '4. Miễn trừ bảo đảm (Dịch vụ "NHƯ HIỆN CÓ")',
        paragraphs: [
          'DỊCH VỤ ĐƯỢC CUNG CẤP "NHƯ HIỆN CÓ" MÀ KHÔNG CÓ BẤT KỲ BẢO ĐẢM NÀO. Chúng tôi không đảm bảo rằng kết quả phân tích là chính xác hoàn toàn hoặc sẽ giúp bạn chắc chắn có được việc làm hay phỏng vấn.',
          'Dịch vụ sử dụng các mô hình AI của bên thứ ba, vốn có thể tạo ra thông tin sai lệch ("ảo giác AI"). Mọi quyết định nghề nghiệp dựa trên Dịch vụ này là trách nhiệm duy nhất của bạn.'
        ]
      },
      {
        title: '5. Giới hạn trách nhiệm pháp lý',
        paragraphs: [
          'TRONG PHẠM VI PHÁP LUẬT CHO PHÉP, RESUMEUPS VÀ NHÀ PHÁT TRIỂN SẼ KHÔNG CHỊU TRÁCH NHIỆM CHO BẤT KỲ THIỆT HẠI TRỰC TIẾP, GIÁN TIẾP HOẶC HỆ QUẢ NÀO, BAO GỒM VIỆC LỘ DỮ LIỆU HOẶC TỔN THẤT TÀI CHÍNH, PHÁT SINH TỪ VIỆC BẠN SỬ DỤNG DỊCH VỤ MIỄN PHÍ NÀY.',
          'Vì đây là dự án miễn phí và mã nguồn mở được cung cấp không qua hợp đồng, bạn đồng ý rằng biện pháp duy nhất nếu không hài lòng là ngừng sử dụng Dịch vụ.'
        ]
      }
    ],
    contact: {
      title: '6. Liên hệ',
      description: 'Đối với các vấn đề liên quan đến điều khoản hoặc báo cáo lạm dụng, vui lòng liên hệ:',
      emailLabel: 'Email',
      email: 'support@resumeups.com'
    }
  }
};