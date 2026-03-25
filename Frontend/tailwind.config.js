/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}"
  ],
  theme: {
    extend: {
      fontFamily: {
        sans: ['Inter', 'sans-serif'],
        serif: ['Playfair Display', 'serif'],
      },
      colors: {
        warm: '#F8F9FA', // renamed offwhite to warm so existing layout works
        ink: '#1E213A', // renamed navy to ink
        muted: '#6B6A76',
        accent: '#8C8BFF', // renamed brand to accent
        'accent-hover': '#706FCD',
        gold: '#F5B841',
        border: '#EEEEEE'
      }
    },
  },
  plugins: [],
}