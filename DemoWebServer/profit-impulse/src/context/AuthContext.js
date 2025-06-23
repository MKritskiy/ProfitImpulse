import React, { createContext, useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import { setAuthToken } from '../utils/api';
import { api } from '../utils/api';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [email, setEmail] = useState(null);
  const router = useRouter();

  useEffect(() => {
    async function fetchData() {
      const token = localStorage.getItem('token');
      setAuthToken(token);
      try{
        await api.get('/api/Test/protected')
        setUser({ token });
      } catch (error)
      {
        setUser(null);
      }
      
      setLoading(false);
    }
    fetchData();
  }, []);

  const login = async (email, password) => {
    try {
      const response = await api.post('/api/User/login', { email, password });
      const token = response.data.token; // Assuming token is returned
      const userId = response.data.userId;
      localStorage.setItem('token', token);
      setAuthToken(token);
      setUser({ userId });
      setEmail(null);
      router.push('/dashboard');
    } catch (error) {
      console.error('Login failed', error);
      throw error;
    }
  };
  const getUser = async (token, usrid) => {
    try {
      localStorage.setItem('token', token);
      setAuthToken(token);
      setUser({ usrid });
      setEmail(null);
      //TODO: Gdeto tut on dostaet usera
    } catch(error) {
      console.error(error);
      throw error;
    }
  }
  const logout = () => {
    localStorage.removeItem('token');
    setAuthToken(null);
    setUser(null);
    setEmail(null);
    router.push('/login');
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, loading, email, setEmail, getUser }}>
      {children}
    </AuthContext.Provider>
  );
};