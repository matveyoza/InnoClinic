import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite';
import basicSsl from '@vitejs/plugin-basic-ssl';

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');

  return {
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
          target: env.VITE_AUTH_SERVICE_URL,
          changeOrigin: true,
          secure: false,
        },
        '/api/users': {
          target: env.VITE_USER_SERVICE_URL,
          changeOrigin: true,
          secure: false,
        },
        '/api/appointments': {
          target: env.VITE_APPOINTMENT_SERVICE_URL,
          changeOrigin: true,
          secure: false,
        },
      },
    },
  };
});