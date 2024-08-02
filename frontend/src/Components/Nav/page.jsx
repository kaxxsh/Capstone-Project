"use client";
import Link from "next/link";
import { useState, useEffect } from "react";
import { FaHome, FaSearch } from "react-icons/fa";
import { IoMdNotifications } from "react-icons/io";
import { BiSolidMessageSquareDots } from "react-icons/bi";
import { FaCircleUser } from "react-icons/fa6";
import Image from "next/image";
import { usePathname } from "next/navigation";
import { BASE_URL } from "@/config";
import UserId from "@/Utils/tokenDecoder";
import PostModel from "./PostModel";

const Skeleton = () => (
  <div className="flex flex-grow items-center gap-2 animate-pulse">
    <div className="rounded-full bg-gray-300 h-10 w-10"></div>
    <div className="flex flex-col gap-1">
      <div className="h-4 bg-gray-300 rounded w-24"></div>
      <div className="h-4 bg-gray-300 rounded w-20"></div>
    </div>
  </div>
);

const Nav = () => {
  const currentPath = usePathname();
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const [userdata, setUserdata] = useState(null);
  const [isPostModelOpen, setPostModelOpen] = useState(false);

  const navlist = [
    { Logo: <FaHome size={30} />, Title: "Home", Link: "/User" },
    { Logo: <FaSearch size={30} />, Title: "Explore", Link: "/User/explore" },
    {
      Logo: <IoMdNotifications size={30} />,
      Title: "Notification",
      Link: userdata ? `/User/Notification/${userdata.id}` : "",
    },
    {
      Logo: <BiSolidMessageSquareDots size={30} />,
      Title: "Message",
      Link: "/User/Message",
    },
    {
      Logo: <FaCircleUser size={30} />,
      Title: "Profile",
      Link: userdata ? `/User/Profile/${userdata.id}` : "#",
    },
  ];

  const toggleDropdown = () => {
    setDropdownOpen(!dropdownOpen);
  };

  const handleLogout = async () => {
    try {
      const response = await fetch(`${BASE_URL}/api/Auth/Logout`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (response.ok) {
        console.log("Logout successful");
        window.location.href = "/";
      } else {
        console.error("Logout failed");
      }
    } catch (error) {
      console.error("An error occurred during logout:", error);
    }
  };

  useEffect(() => {
    const fetchUserData = async () => {
      const user = await UserId();
      if (user) {
        const response = await fetch(
          `${BASE_URL}/api/User/UserId?UserId=${user}`,
          {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
            },
            credentials: "include",
          }
        );
        const data = await response.json();
        setUserdata(data);
      }
    };
    fetchUserData();
  }, []);

  return (
    <section>
      {/* Desktop Navigation */}
      <nav className="hidden lg:flex flex-col h-screen text-gray-700 font-mono font-extralight justify-between">
        <div>
          <div className="h-16 p-2 mb-4">
            <Image src="/logo.jpg" width={45} height={45} alt="Logo" />
          </div>
          <ul className="flex flex-col gap-8">
            {navlist.map((nav, index) => (
              <li key={index} className="flex gap-2 items-center">
                <Link
                  href={nav.Link}
                  aria-label={nav.Title}
                  className={`mx-4 flex gap-4 items-center hover:text-gray-100 transition duration-300 ${
                    currentPath === nav.Link ? "text-gray-100" : ""
                  }`}
                >
                  {nav.Logo}
                  <span className="text-xl font-semibold">{nav.Title}</span>
                </Link>
              </li>
            ))}
          </ul>
        </div>
        <div className="flex flex-col items-center space-y-4 p-2">
          <button
            onClick={() => setPostModelOpen(true)}
            className="flex justify-center items-center text-xl font-bold bg-blue-600 text-white m-3 p-3 px-16 rounded-3xl hover:bg-blue-800 transition duration-100"
          >
            Post
          </button>
          {isPostModelOpen && (
            <PostModel onClose={() => setPostModelOpen(false)} />
          )}
          <div className="hidden lg:flex items-center p-2 gap-6 border-gray-700">
            <div className="flex flex-grow items-center gap-2">
              {userdata ? (
                <>
                  <div className="rounded-full">
                    <img
                      src={userdata.profileImage}
                      alt="userImage"
                      className="w-10 h-10 object-cover rounded-full"
                    />
                  </div>
                  <div className="flex flex-col">
                    <div className="text-gray-100 text-xs truncate max-w-xs">
                      {userdata.name}
                    </div>
                    <div className="text-gray-500 text-xs truncate max-w-xs">
                      @{userdata.userName}
                    </div>
                  </div>
                </>
              ) : (
                <Skeleton />
              )}
            </div>
            <div className="ml-auto relative">
              <button
                id="dropdownMenuIconHorizontalButton"
                onClick={toggleDropdown}
                className="inline-flex items-center p-2 text-sm font-medium text-center focus:outline-none dark:text-white"
                type="button"
              >
                <svg
                  className="w-5 h-5"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="currentColor"
                  viewBox="0 0 16 3"
                >
                  <path d="M2 0a1.5 1.5 0 1 1 0 3 1.5 1.5 0 0 1 0-3Zm6.041 0a1.5 1.5 0 1 1 0 3 1.5 1.5 0 0 1 0-3ZM14 0a1.5 1.5 0 1 1 0 3 1.5 1.5 0 0 1 0-3Z" />
                </svg>
              </button>
              {dropdownOpen && (
                <div
                  id="dropdownDotsHorizontal"
                  className="z-50 absolute bg-white divide-y divide-gray-100 rounded-lg shadow w-44 dark:bg-gray-700 dark:divide-gray-600 top-0 ml-10 -mt-24"
                >
                  <ul
                    className="py-2 text-sm text-gray-700 dark:text-gray-200"
                    aria-labelledby="dropdownMenuIconHorizontalButton"
                  >
                    <li>
                      <Link
                        href="#"
                        className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white"
                      >
                        Update Profile
                      </Link>
                    </li>
                  </ul>
                  <div className="py-2">
                    <Link
                      href="#"
                      className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-200 dark:hover:text-white"
                      onClick={handleLogout}
                    >
                      Log out
                    </Link>
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>
      </nav>
      {/* Mobile Navigation */}
      <nav className="lg:hidden fixed bottom-0 left-0 text-gray-500 right-0 p-2 flex justify-around items-center border-t border-gray-700 bg-black">
        {navlist.map((nav, index) => (
          <Link
            key={index}
            href={nav.Link}
            aria-label={nav.Title}
            className={`flex flex-col items-center hover:text-gray-100 transition duration-300 ${
              currentPath === nav.Link ? "text-gray-100" : ""
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
