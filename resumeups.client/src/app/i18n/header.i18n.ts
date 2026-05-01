import { LanguageDictionary } from '../core/i18n/language.types';

interface HeaderTranslation {
  nav: {
    home: string;
    resume: string;
    about: string;
  };
  languageSwitcher: {
    vi: string;
    en: string;
  };
}

export const headerTranslations: LanguageDictionary<HeaderTranslation> = {
  en: {
    nav: {
      home: 'Home',
      resume: 'Resume',
      about: 'About'
    },
    languageSwitcher: {
      vi: 'VI',
      en: 'EN'
    }
  },
  vi: {
    nav: {
      home: 'Trang chủ',
      resume: 'Kiểm tra',
      about: 'Giới thiệu'
    },
    languageSwitcher: {
      vi: 'VI',
      en: 'EN'
    }
  }
};
