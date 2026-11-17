import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { App } from './App';
import { ErrorBoundary } from 'react-error-boundary';
import { GlobalErrorFallback } from './components/Fallback/GlobalErrorFallback';
import { AxiosInterceptor } from './components/AxiosInterceptor';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ErrorBoundary FallbackComponent={GlobalErrorFallback}>
      <AxiosInterceptor>
        <App />
      </AxiosInterceptor>
    </ErrorBoundary>
  </StrictMode>,
);
