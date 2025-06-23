import { useState, useContext } from 'react';
import { useRouter } from 'next/router';
import { AuthContext } from '../context/AuthContext';
import { api } from '../utils/api';

const Verify = () => {
  const { email, getUser } = useContext(AuthContext); // Retrieve email from context
  const [code, setCode] = useState('');
  const [error, setError] = useState('');
  const router = useRouter();

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!email) {
      setError('Email not found. Please register again.');
      return;
    }
    try {
      var res = await api.post('/api/User/verification', { email, code });
      getUser(res.data.token, res.data.userId)
      router.push('/login');
    } catch (err) {
      setError('Verification failed');
    }
  };

  return (
    <div>
      <h1>Verify Email</h1>
      <p>Verification code sent to: {email || 'Unknown'}</p>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          value={code}
          onChange={(e) => setCode(e.target.value)}
          placeholder="Verification Code"
          required
        />
        <button type="submit">Verify</button>
      </form>
      {error && <p>{error}</p>}
    </div>
  );
};

export default Verify;