import React from 'react';
import { Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import SignupPage from './pages/SignupPage';
import CoursesPage from './pages/CoursesPage';
import StudentsPage from './pages/StudentsPage';
import Header from './components/Layout/Header';
import Footer from './components/Layout/Footer';
import './styles/App.css';

function App() {
  return (
    <div className="App">
      <Header />
      <main className="container mt-3">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/signup" element={<SignupPage />} />
          <Route path="/courses" element={<CoursesPage />} />
          <Route path="/students" element={<StudentsPage />} />
        </Routes>
      </main>
      <Footer />
    </div>
  );
}

export default App;
