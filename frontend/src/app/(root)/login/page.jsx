"use client";
import { useState } from "react";
import Link from "next/link";
import { BASE_URL } from "@/config";

const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = async (e) => {
    e.preventDefault();
    await fetch(`${BASE_URL}/api/Auth/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ userName: username, password: password }),
      credentials: "include",
    }).then((res) => {
      if (res.ok) {
        window.location.href = "/User";
      } else {
        alert("Invalid username or password");
      }
    });
  };

  return (
    <section className="w-full min-h-screen flex flex-col items-center justify-center">
      <div className="bg-[#252424] shadow-xl rounded-lg p-8 max-w-sm w-full">
        <h1 className="text-2xl font-bold text-center mb-6">Login</h1>
        <form className="flex flex-col space-y-4 md:gap-2">
          <div>
            <label
              htmlFor="username"
              className="block text-sm font-medium text-zinc-100"
            >
              Username
            </label>
            <input
              type="text"
              id="username"
              name="username"
              className="mt-1 block w-full px-3 py-2 border border-gray-300 text-black rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              placeholder="Enter your username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />
          </div>
          <div>
            <label
              htmlFor="password"
              className="block text-sm font-medium text-zinc-100"
            >
              Password
            </label>
            <input
              type="password"
              id="password"
              name="password"
              className="mt-1 block w-full px-3 py-2  border border-gray-300 text-black rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              placeholder="Enter your password"
              value={password}
              autoComplete="current-password"
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          <button
            type="submit"
            className="w-full bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600 transition duration-300 font-semibold"
            onClick={handleLogin}
          >
            Login
          </button>
        </form>
        <p className="text-center text-zinc-100 mt-4">
          Don't have an account?{" "}
          <Link href="/signup" className="text-blue-500 hover:underline">
            Register
          </Link>
        </p>
      </div>
    </section>
  );
};

export default Login;
