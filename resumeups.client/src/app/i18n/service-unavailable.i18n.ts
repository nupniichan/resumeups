import { LanguageDictionary } from '../core/i18n/language.types';

interface ServiceUnavailableTranslation {
  errorCode: string;
  errorTitle: string;
  errorDescription: string;
  stopCode: string;
  whatFailed: string;
  whatFailedValue: string;
  percentComplete: string;
  restartNote: string;
  goHomeBtn: string;
  goBackBtn: string;
}

export const serviceUnavailableTranslations: LanguageDictionary<ServiceUnavailableTranslation> = {
  en: {
    errorCode: '503',
    errorTitle: 'ResumeUps currently down for maintenance.',
    errorDescription:
      "ResumeUps is temporarily unavailable. We're performing scheduled maintenance to improve your experience. Please check back soon.",
    stopCode: 'Stop code: SERVICE_UNAVAILABLE',
    whatFailed: 'What failed: ',
    whatFailedValue: 'ResumeUps_Maintenance.sys',
    percentComplete: '% complete',
    restartNote:
      'Error 0x000001F7  ·  IRQL: 0x00000002  ·  DRIVER: ResumeUps_Maintenance.sys  ·  ADDRESS: 0xFFFFF804_000001F7',
    goHomeBtn: 'Back to Home',
    goBackBtn: 'Try Again'
  },
  vi: {
    errorCode: '503',
    errorTitle: 'ResumeUps đang bảo trì hệ thống.',
    errorDescription:
      'ResumeUps tạm thời không khả dụng. Tụi mình đang bảo trì để cải thiện trải nghiệm của bạn. Vui lòng quay lại sau.',
    stopCode: 'STOP CODE: SERVICE_UNAVAILABLE',
    whatFailed: 'Cái gì gây ra lỗi: ',
    whatFailedValue: 'ResumeUps_Maintenance.sys',
    percentComplete: '% hoàn thành',
    restartNote:
      'Lỗi 0x000001F7  ·  IRQL: 0x00000002  ·  DRIVER: ResumeUps_Maintenance.sys  ·  ADDRESS: 0xFFFFF804_000001F7',
    goHomeBtn: 'Về Trang Chủ',
    goBackBtn: 'Thử Lại'
  }
};
