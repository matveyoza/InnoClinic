import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import basicSsl from '@vitejs/plugin-basic-ssl'

export default defineConfig({
  plugins: [
    react(),
    tailwindcss(),
    basicSsl(),
  ],
  server: {
    port: 5173,
    strictPort: true,
    proxy: {
      '/api/auth': {
        target: 'https://localhost:7096',
        changeOrigin: true,
        secure: false,
      },
      '/api/users': {
        target: 'https://localhost:7183',
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
