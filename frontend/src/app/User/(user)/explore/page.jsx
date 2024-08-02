"use client";

import { useState, useEffect } from "react";
import { FaSearch } from "react-icons/fa";
import { IoArrowBack } from "react-icons/io5";
import { BASE_URL } from "@/config";
import Userdisplay from "@/app/User/(user)/[PostId]/userdisplay";

const Explore = () => {
  const [search, setSearch] = useState("");
  const [hashtags, setHashtags] = useState([]);
  const [hashtagPosts, setHashtagPosts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [data, setData] = useState([]);

  const fetchUser = async () => {
    if (search.trim() === "") {
      setData([]);
      return;
    }
    try {
      const response = await fetch(
        `${BASE_URL}/api/User/Search?search=${search}`
      );
      const result = await response.json();
      setData(result);
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };

  console.log(data);

  const fetchHashtags = async () => {
    try {
      const response = await fetch(`${BASE_URL}/api/PostFeed/hashtags`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (response.ok) {
        const data = await response.json();
        setHashtags(data);

        const posts = await Promise.all(
          data.slice(0, 4).map(async (item) => {
            try {
              const response1 = await fetch(
                `${BASE_URL}/api/PostFeed/hashtag/${encodeURIComponent(
                  item.tag
                )}`,
                {
                  method: "GET",
                  headers: {
                    "Content-Type": "application/json",
                  },
                  credentials: "include",
                }
              );
              if (response1.ok) {
                return response1.json();
              } else {
                console.error("Failed to fetch data for tag", item.tag);
                return null;
              }
            } catch (error) {
              console.error("Error fetching data for tag", item.tag, error);
              return null;
            }
          })
        );

        setHashtagPosts(posts.filter((post) => post !== null));
      } else {
        console.error("Failed to fetch hashtags");
        setError("Failed to fetch hashtags");
      }
    } catch (error) {
      console.error("Error fetching data:", error);
      setError("Error fetching data");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUser();
  }, [search]);

  useEffect(() => {
    fetchHashtags();
  }, []);

  if (loading) {
    return (
      <section>
        <div className="p-1 m-2 text-2xl font-bold flex items-center gap-2">
          <div className="skeleton w-8 h-8 rounded-full"></div>
          <div className="skeleton h-6 w-24"></div>
        </div>
        <div className="flex justify-center items-center border border-gray-300 rounded-3xl p-4 m-6 pl-8 gap-3">
          <div className="skeleton h-6 w-6 rounded-full"></div>
          <div className="skeleton h-6 w-full"></div>
        </div>
        <div className="container mx-auto p-4 space-y-4">
          <div className="skeleton h-6 w-32 mb-6"></div>
          <div className="space-y-4">
            <div className="skeleton h-24 w-full rounded-lg"></div>
            <div className="skeleton h-24 w-full rounded-lg"></div>
            <div className="skeleton h-24 w-full rounded-lg"></div>
            <div className="skeleton h-24 w-full rounded-lg"></div>
          </div>
        </div>
      </section>
    );
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <section>
      <div className="p-1 m-2 text-2xl font-bold flex items-center gap-2">
        <div>
          <IoArrowBack
            className="pt-1 cursor-pointer"
            onClick={() => history.back()}
          />
        </div>
        Explore
      </div>
      <div className="flex justify-center items-center border border-gray-300 rounded-3xl p-4 m-6 pl-8 gap-3">
        <FaSearch className="text-gray-500" />
        <input
          type="text"
          className="border-none outline-none flex-grow p-1 bg-transparent placeholder-text-gray-700"
          placeholder="Search for people"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>
      {search.length === 0 ? (
        <div className="container mx-auto p-4">
          <div className="mb-6">
            <h2 className="text-2xl font-bold">Top Hashtags</h2>
          </div>
          <div className="space-y-4">
            {hashtags.slice(0, 4).map((hashtag, index) => (
              <div
                key={hashtag.tag}
                className="shadow rounded-lg p-4"
                style={{
                  backgroundImage: `url(${
                    hashtagPosts[index]?.[0]?.image || ""
                  })`,
                  backgroundSize: "cover",
                  backgroundPosition: "center",
                  backgroundRepeat: "no-repeat",
                }}
              >
                <div className="text-xl font-semibold text-blue-600 bg-gray-700 bg-opacity-20 p-2 rounded text-center">
                  {hashtag.tag}
                </div>
              </div>
            ))}
          </div>
        </div>
      ) : (
        <div className="container mx-auto p-4">
          <div className="mb-6">
            <h2 className="text-2xl font-bold">Search Results</h2>
          </div>
          <div className="space-y-4">
            {data.map((item) => (
              <Userdisplay key={item.userId} user={{ item }} />
            ))}
          </div>
        </div>
      )}
      <div className="container mx-auto p-4">
        <div className="mb-6">
          <h2 className="text-2xl font-bold">Trending Hashtags</h2>
        </div>
        <div className="space-y-2">
          {hashtags.map((hashtag) => (
            <div key={hashtag.tag} className="text-lg">
              {hashtag.tag}
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};

export default Explore;
