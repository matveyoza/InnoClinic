import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { App } from './App';
import { setupAxiosInterceptors } from './api/axios';
import { useAuthStore } from './store/useAuthStore';

setupAxiosInterceptors(() => {
  useAuthStore.getState().setUser(null);
});

useAuthStore.getState().checkAuthStatus();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
);
