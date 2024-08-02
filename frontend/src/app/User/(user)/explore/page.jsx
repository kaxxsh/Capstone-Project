"use client";
import { useState, useEffect } from "react";
import { FaSearch } from "react-icons/fa";
import { BASE_URL } from "@/config";

const Explore = () => {
  const [search, setSearch] = useState("");
  const [hashtags, setHashtags] = useState([]);
  const [hashtagPosts, setHashtagPosts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

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
            // Limit to top 4 hashtags
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
    fetchHashtags();
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <section>
      <div className="">
        <div className="flex justify-center items-center border border-zinc-800 rounded-3xl p-4 m-6 pl-8 gap-3">
          <FaSearch className="text-gray-500" />
          <input
            type="text"
            className="border-none outline-none flex-grow p-1 bg-transparent placeholder:text-gray-700"
            placeholder="Search for people"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>
      </div>
      {search.length === 0 && (
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
                <div className="text-xl font-semibold text-blue-600 bg-gray-700 text-center bg-opacity-20 p-2 rounded">
                  {hashtag.tag}
                </div>
              </div>
            ))}
          </div>
        </div>
      )}
    </section>
  );
};

export default Explore;
