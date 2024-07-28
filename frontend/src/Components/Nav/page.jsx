"use client";
import Link from "next/link";
import React from "react";
import { FaHome, FaSearch } from "react-icons/fa";
import { IoMdNotifications } from "react-icons/io";
import { BiSolidMessageSquareDots } from "react-icons/bi";
import { FaCircleUser } from "react-icons/fa6";
import { BsThreeDots } from "react-icons/bs";
import Image from "next/image";
import { usePathname } from "next/navigation";

const navlist = [
  { Logo: <FaHome size={30} />, Title: "Home", Link: "/User" },
  { Logo: <FaSearch size={30} />, Title: "Explore", Link: "/User/explore" },
  {
    Logo: <IoMdNotifications size={30} />,
    Title: "Notification",
    Link: "/User/Notification",
  },
  {
    Logo: <BiSolidMessageSquareDots size={30} />,
    Title: "Message",
    Link: "/User/Message",
  },
  { Logo: <FaCircleUser size={30} />, Title: "Profile", Link: "/User/Profile" },
];

const Nav = () => {
  const currentPath = usePathname();

  return (
    <section>
      <nav className="relative hidden flex flex-col h-screen text-zinc-500 font-mono font-extralight lg:flex lg:flex-col justify-between">
        <div>
          <div className="h-16 p-2 mb-4">
            <Image
              src="/logo.jpg"
              width={45}
              height={45}
              alt="Logo"
              className="hidden lg:inline"
            />
          </div>
          <ul className="w-60 flex flex-col gap-8">
            {navlist.map((nav, index) => (
              <li key={index} className="flex gap-2 items-center">
                <Link
                  href={nav.Link}
                  aria-label={nav.Title}
                  className={`mx-4 flex gap-4 items-center lg:inline lg:flex hover:text-zinc-300 transition duration-300 ${
                    currentPath === nav.Link ? "text-zinc-100" : ""
                  }`}
                >
                  {nav.Logo}
                  <span className="text-xl font-semibold">{nav.Title}</span>
                </Link>
              </li>
            ))}
          </ul>
        </div>
        <div className="flex flex-col items-center p-4 space-y-4">
          <Link
            href=""
            className="flex justify-center items-center text-xl font-bold bg-blue-600 text-white py-3 px-20 rounded-3xl hover:bg-blue-800 transition duration-100"
          >
            Post
          </Link>
          <div className="hidden lg:flex items-center mx-4 gap-4 p-2 rounded-lg shadow-lg text-zinc-300">
            <div className="flex-shrink-0">
              <div className="rounded-full bg-gray-300 p-2">
                <FaCircleUser size={24} className="text-gray-700" />
              </div>
            </div>
            <div className="flex flex-col">
              <div className="font-semibold">Name</div>
              <div className="text-gray-500 text-sm">@UserName</div>
            </div>
            <div className="ml-auto">
              <BsThreeDots
                size={24}
                className="hover:text-blue-500 transition duration-300 cursor-pointer"
              />
            </div>
          </div>
        </div>
      </nav>
      <nav className="lg:hidden fixed bottom-0 left-0 text-zinc-500 right-0 p-2 flex justify-around items-center border-t border-gray-700 bg-black">
        {navlist.map((nav, index) => (
          <Link
            key={index}
            href={nav.Link}
            aria-label={nav.Title}
            className={`flex flex-col items-center hover:text-zinc-100 transition duration-300 ${
              currentPath === nav.Link ? "text-zinc-100" : ""
            }`}
          >
            {nav.Logo}
          </Link>
        ))}
      </nav>
    </section>
  );
};

export default Nav;
