import i18n from 'i18next';
import {initReactI18next} from 'react-i18next';
import Backend from 'i18next-http-backend';
import LanguageDetector from 'i18next-browser-languagedetector';

i18n
  // load translation using http -> see /public/locales (i.e. https://github.com/i18next/react-i18next/tree/master/example/react/public/locales)
  // learn more: https://github.com/i18next/i18next-http-backend
  .use(Backend)
  // detect user language
  // learn more: https://github.com/i18next/i18next-browser-languageDetector
  .use(LanguageDetector)
  // pass the i18n instance to react-i18next.
  .use(initReactI18next)
  // init i18next
  // for all options read: https://www.i18next.com/overview/configuration-options
  .init({
    fallbackLng: 'ru-RU',
    debug: false,
    ns: ['label', 'settings', 'message', 'authorization', 'newCaseStepper'],
    defaultNS: 'common',
    interpolation: {
      format: (value, rawFormat, lng) => {
        const [format, ...additionalValues] = rawFormat.split(',').map((v) => v.trim());

        switch (format) {
          case 'number':
            return new Intl.NumberFormat(lng).format(value)
          case 'date':
            return new Intl.DateTimeFormat(lng).format(value);
          case 'time':
            return value.toLocaleTimeString(lng, {hour: '2-digit', minute: '2-digit'});
          case 'currency':
            return Intl.NumberFormat(lng, {
              style: 'currency',
              currency: additionalValues[0]
            }).format(value);
          default:
            return value;
        }
      },
      escapeValue: false, // not needed for react as it escapes by default
    }
  });


export default i18n;
