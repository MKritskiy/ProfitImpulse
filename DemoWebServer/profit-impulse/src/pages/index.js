import Link from 'next/link';

const Home = () => {
  return (
    <div>
      <h1>Welcome to Meetya</h1>
      <Link href="/register">Register</Link>
      <br />
      <Link href="/login">Login</Link>
    </div>
  );
};

export default Home;