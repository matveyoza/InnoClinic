import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { App } from './App';
import { setupAxiosInterceptors } from './api/axios';
import { checkAuthStatus } from './services/authService';
import { ErrorBoundary } from 'react-error-boundary';
import { GlobalErrorFallback } from './components/Fallback/GlobalErrorFallback';

setupAxiosInterceptors();

checkAuthStatus();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ErrorBoundary FallbackComponent={GlobalErrorFallback}>
      <App />
    </ErrorBoundary>
  </StrictMode>,
);
