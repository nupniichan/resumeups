import { LanguageDictionary } from '../core/i18n/language.types';

interface NotFoundTranslation {
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

export const notFoundTranslations: LanguageDictionary<NotFoundTranslation> = {
  en: {
    errorCode: '404',
    errorTitle: 'This page does not exist.',
    errorDescription:
      "Wait, looks like this page doesn't exist or is being worked on. You can check the details below.",
    stopCode: 'Stop code: PAGE_NOT_FOUND',
    whatFailed: 'What failed: ',
    whatFailedValue: 'ResumeUps_Router.sys',
    percentComplete: '% complete',
    restartNote:
      'Error 0x00000404  ·  IRQL: 0x00000000  ·  DRIVER: ResumeUps_Router.sys  ·  ADDRESS: 0xFFFFF804_00000404',
    goHomeBtn: 'Back to Home',
    goBackBtn: 'Go Back'
  },
  vi: {
    errorCode: '404',
    errorTitle: 'Trang này không tồn tại.',
    errorDescription:
      'Khoan, hình như cái trang này không tồn tại hoặc đang được làm gì đó. Bạn có thể kiểm tra chi tiết bên dưới.',
    stopCode: 'STOP CODE: PAGE_NOT_FOUND',
    whatFailed: 'Cái gì gây ra lỗi: ',
    whatFailedValue: 'ResumeUps_Router.sys',
    percentComplete: '% hoàn thành',
    restartNote:
      'Lỗi 0x00000404  ·  IRQL: 0x00000000  ·  DRIVER: ResumeUps_Router.sys  ·  ADDRESS: 0xFFFFF804_00000404',
    goHomeBtn: 'Về Trang Chủ',
    goBackBtn: 'Quay Lại'
  }
};
