export const SUPPORTED_LANGUAGES = ['en', 'vi'] as const;

export type LanguageCode = (typeof SUPPORTED_LANGUAGES)[number];

export type LanguageDictionary<T> = Record<LanguageCode, T>;
