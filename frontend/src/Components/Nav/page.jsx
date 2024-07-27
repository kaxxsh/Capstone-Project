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
      <nav className="flex flex-col h-screen p-4 lg:w-[20%] lg:fixed lg:border-r lg:border-zinc-800 ml-6 pl-10 text-zinc-500 font-mono font-extralight">
        <div className="h-16 ml-6 p-2 mb-4">
          <Image
            src="/logo.jpg"
            width={45}
            height={45}
            className="hidden h-fit lg:inline "
          />
        </div>
        <div className=" h-[75%]">
          <ul className="w-60 flex flex-col gap-8">
            {navlist.map((nav, index) => (
              <li key={index} className="flex gap-2 items-center">
                <Link
                  href={nav.Link}
                  className={`ml-10 flex gap-4 hidden lg:inline lg:flex hover:text-zinc-300 transition duration-300 ${
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
        <div className="hidden lg:block mt-4 ml-10">
          <Link
            href=""
            className="flex justify-center items-center text-xl font-bold bg-blue-600 text-white py-3 rounded-3xl hover:bg-blue-800 transition duration-100"
          >
            Post
          </Link>
        </div>
        <div className="hidden lg:flex items-center gap-4 mt-6 ml-10 p-4 py-2 rounded-lg shadow-lg text-zinc-300">
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
              className="hover:text-blue-500 transition duration-300"
            />
          </div>
        </div>
      </nav>
      <nav className="lg:hidden fixed bottom-0 left-0 right-0 p-2 flex justify-around items-center border-t border-gray-700 bg-black">
        {navlist.map((nav, index) => (
          <Link
            key={index}
            href={nav.Link}
            className={`flex flex-col items-center hover:text-blue-500 transition duration-300 ${
              currentPath === nav.Link ? "text-blue-500" : ""
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
