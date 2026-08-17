/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
  theme: {
    extend: {
      boxShadow: {
        soft: '0 20px 40px -20px rgba(34, 211, 238, 0.35)',
      },
    },
  },
  plugins: [],
};
