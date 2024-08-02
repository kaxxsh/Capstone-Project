"use client";
import Link from "next/link";
import Image from "next/image";
import { TypeAnimation } from "react-type-animation";

export default function Home() {

  return (
    <section className="min-h-screen flex flex-col items-center justify-center relative bg-gradient-to-r from-slate-900 to-slate-700">
      <div className="my-8">
        <span className="w-full">
          <Image src="/logo.png" alt="Kratos" width={400} height={95} />
        </span>
      </div>
      <TypeAnimation
        sequence={[
          "Share your thoughts with the world",
          2000,
          "Engage with a vibrant community",
          2000,
          "Explore trending topics and stories",
          2000,
          "Follow your favorite creators and influencers",
          2000,
        ]}
        wrapper="span"
        speed={50}
        repeat={Infinity}
        className="font-extrabold mb-4 sm:text-[2.5em] bg-gradient-to-r from-indigo-400 to-cyan-400 bg-clip-text text-transparent"
      />
      <div className="flex flex-col items-center justify-evenly">
        <label className="text-lg font-medium mb-2 hover:text-orange-400">
          Join today
        </label>
        <div className="w-80 flex flex-col p-4">
          <Link
            href="/login"
            className="w-full mb-4 flex items-center justify-center bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded transition duration-300"
          >
            Login
          </Link>
          <Link
            href="/signup"
            className="w-full mb-4 flex items-center justify-center bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded transition duration-300"
          >
            Register
          </Link>
        </div>
      </div>
    </section>
  );
}
