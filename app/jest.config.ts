import type { Config } from 'jest';

const jestConfig: Config = {
  preset: 'jest-preset-angular',
  setupFilesAfterEnv: ['<rootDir>/setup-jest.ts'],
  testPathIgnorePatterns: [
    '<rootDir>/node_modules/',
    '<rootDir>/dist/',
  ],
  moduleNameMapper: {
    '@app/(.*)': '<rootDir>/src/app/$1',
    '@assets/(.*)': '<rootDir>/src/assets/$1',
    '@core/(.*)': '<rootDir>/src/app/core/$1',
    '@env/(.*)': '<rootDir>/src/environments/$1',
    '@shared/(.*)': '<rootDir>/src/app/shared/$1',
    '\\.(html|svg)$': '<rootDir>/src/test/mocks/html.mock.ts'
  },
  transform: {
    '^.+\\.(ts|js|mjs|html|svg)$': [
      'jest-preset-angular',
      {
        tsconfig: '<rootDir>/tsconfig.spec.json',
        stringifyContentPathRegex: '\\.html$',
        isolatedModules: true
      }
    ]
  }
};

export default jestConfig;