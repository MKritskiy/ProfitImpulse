import { useState, useContext } from 'react';
import { useRouter } from 'next/router';
import { api } from '../utils/api';
import { AuthContext } from '../context/AuthContext';

const Register = () => {
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const router = useRouter();
  const { email, setEmail } = useContext(AuthContext);
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await api.post('/api/User/register', { email, password });
      router.push('/verify');
    } catch (err) {
      setError('Registration failed');
    }
  };

  return (
    <div>
      <h1>Register</h1>
      <form onSubmit={handleSubmit}>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Email"
          required
        />
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Password"
          required
        />
        <button type="submit">Register</button>
      </form>
      {error && <p>{error}</p>}
    </div>
  );
};

export default Register;