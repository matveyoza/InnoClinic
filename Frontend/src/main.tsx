import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { MainPage } from './pages/MainPage/MainPage';
import { LogInPage } from './pages/LogInPage/LogInPage';
import { SignUpPage } from './pages/SignUpPage/SignUpPage';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <SignUpPage />
  </StrictMode>,
);
