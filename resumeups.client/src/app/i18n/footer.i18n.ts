import { LanguageDictionary } from '../core/i18n/language.types';

interface FooterTranslation {
  tagline: string;
  nav: {
    home: string;
    resume: string;
    about: string;
  };
  social: {
    github: string;
    linkedin: string;
  };
  copyright: string;
}

export const footerTranslations: LanguageDictionary<FooterTranslation> = {
  en: {
    tagline: 'Increase your chances of getting an interview',
    nav: {
      home: 'Home',
      resume: 'Resume',
      about: 'About'
    },
    social: {
      github: 'GitHub',
      linkedin: 'LinkedIn'
    },
    copyright: '© 2026 nupniichan. All rights reserved.'
  },
  vi: {
    tagline: 'Tăng tỷ lệ được gọi phỏng vấn cùng ResumeUps',
    nav: {
      home: 'Trang chủ',
      resume: 'Kiểm tra',
      about: 'Giới thiệu'
    },
    social: {
      github: 'GitHub',
      linkedin: 'LinkedIn'
    },
    copyright: '© 2026 nupniichan. Da dang ky ban quyen.'
  }
};
