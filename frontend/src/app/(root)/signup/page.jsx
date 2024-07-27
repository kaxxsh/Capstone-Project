"use client";
import { useState } from "react";
import Link from "next/link";
import { BASE_URL } from "@/config";

const Signup = () => {
  const [username, setUsername] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [password, setPassword] = useState("");

  const handleSignup = async (e) => {
    e.preventDefault();
    await fetch(`${BASE_URL}/api/Auth/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        userName: username,
        name: username,
        profileImage:
          "https://res.cloudinary.com/dy33jpyuv/image/upload/v1722090684/kcbrijslds60mdlerixr.png",
        bio: "Hey there! I am using kratos.",
        location: "India",
        dateOfBirth: "2000-01-01",
        gender: "select Your Gender",
        phoneNumber: phoneNumber,
        email: "Enter Your Email",
        password: password,
      }),
      credentials: "include",
    }).then((res) => {
      if (res.ok) {
        window.location.href = "/login";
      } else {
        alert("Invalid username or password");
      }
    });
  };
  return (
    <section className="w-full min-h-screen flex flex-col items-center justify-center">
      <div className="bg-[#252424] shadow-xl rounded-lg p-8 max-w-sm w-full">
        <h1 className="text-2xl font-bold text-center mb-6">Register</h1>
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
              placeholder="Enter your userName"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />
          </div>
          <div>
            <label
              htmlFor="phoneNumber"
              className="block text-sm font-medium text-zinc-100"
            >
              PhoneNumber
            </label>
            <input
              type="text"
              id="phoneNumber"
              name="phoneNumber"
              className="mt-1 block w-full px-3 py-2 border border-gray-300 text-black rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              placeholder="Enter your phoneNumber"
              minLength={10}
              maxLength={10}
              value={phoneNumber}
              onChange={(e) => setPhoneNumber(e.target.value)}
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
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          <button
            type="submit"
            className="w-full bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600 transition duration-300 font-semibold"
            onClick={handleSignup}
          >
            Register
          </button>
        </form>
        <p className="text-center text-zinc-100 mt-4">
          click here to{" "}
          <Link href="/login" className="text-blue-500 hover:underline ">
            Login
          </Link>
        </p>
      </div>
    </section>
  );
};

export default Signup;
