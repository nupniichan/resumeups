import { LanguageDictionary } from '../core/i18n/language.types';

interface ServerErrorTranslation {
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

export const serverErrorTranslations: LanguageDictionary<ServerErrorTranslation> = {
  en: {
    errorCode: '500',
    errorTitle: 'Something went wrong on our end.',
    errorDescription:
      "Our server crashed unexpectedly. We're already looking into it. Please try again in a moment.",
    stopCode: 'Stop code: INTERNAL_SERVER_ERROR',
    whatFailed: 'What failed: ',
    whatFailedValue: 'ResumeUps_Server.sys',
    percentComplete: '% complete',
    restartNote:
      'Error 0x000001F4  ·  IRQL: 0x00000001  ·  DRIVER: ResumeUps_Server.sys  ·  ADDRESS: 0xFFFFF804_000001F4',
    goHomeBtn: 'Back to Home',
    goBackBtn: 'Go Back'
  },
  vi: {
    errorCode: '500',
    errorTitle: 'Có lỗi xảy ra ở bên server.',
    errorDescription:
      'Server của ResumeUps gặp sự cố ngoài ý muốn. Vui lòng thử lại sau ít phút.',
    stopCode: 'STOP CODE: INTERNAL_SERVER_ERROR',
    whatFailed: 'Cái gì gây ra lỗi: ',
    whatFailedValue: 'ResumeUps_Server.sys',
    percentComplete: '% hoàn thành',
    restartNote:
      'Lỗi 0x000001F4  ·  IRQL: 0x00000001  ·  DRIVER: ResumeUps_Server.sys  ·  ADDRESS: 0xFFFFF804_000001F4',
    goHomeBtn: 'Về Trang Chủ',
    goBackBtn: 'Quay Lại'
  }
};
