"use client";
import React, { useState, useEffect } from "react";
import Post from "@/components/Post/page";
import PostSkeleton from "@/Components/Post/PostSkeleton";
import AddPost from "@/components/Post/addPost";
import { BASE_URL } from "@/config";
import UserId from "@/Utils/tokenDecoder";

const UserPage = () => {
  const [activeTab, setActiveTab] = useState("forYou");
  const [posts, setPosts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [loggedInUserId, setLoggedInUserId] = useState(null);
  const [currentUser, setCurrentUser] = useState({});

  useEffect(() => {
    const fetchUserId = async () => {
      try {
        const userId = await UserId();
        setLoggedInUserId(userId);
      } catch (error) {
        console.error("Error fetching user ID", error);
      }
    };
    fetchUserId();
  }, []);

  useEffect(() => {
    const getUser = async () => {
      try {
        if (loggedInUserId) {
          const response = await fetch(
            `${BASE_URL}/api/User/Search?search=${loggedInUserId}`,
            {
              method: "GET",
              headers: {
                "Content-Type": "application/json",
              },
              credentials: "include",
            }
          );
          if (response.ok) {
            const data = await response.json();
            setCurrentUser(data[0]);
          }
        }
      } catch (error) {
        console.error("Error fetching user data", error);
      }
    };
    getUser();
  }, [loggedInUserId]);

  useEffect(() => {
    const fetchPosts = async () => {
      setIsLoading(true);
      const url =
        activeTab === "forYou"
          ? `${BASE_URL}/api/PostFeed/GetAllPostsAndRetweets`
          : `${BASE_URL}/api/PostFeed/GetPostByUserFollowed`;
      try {
        const response = await fetch(url, {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.getItem("jwt")}`,
          },
          credentials: "include",
        });
        const data = await response.json();
        setPosts(data);
      } catch (error) {
        console.error("Error fetching posts:", error);
      } finally {
        setTimeout(() => setIsLoading(false), 2000);
      }
    };
    fetchPosts();
  }, [activeTab]);

  return (
    <section className="text-sm lg:text-base">
      <div className="h-12 lg:h-16 flex justify-around items-center p-4 lg:p-6 border-b border-gray-700 font-bold text-gray-500">
        <div
          className={`h-12 lg:h-16 flex-1 flex justify-center items-center cursor-pointer hover:text-gray-800 ${
            activeTab === "forYou"
              ? "border-b-4 border-blue-500 text-gray-300"
              : ""
          }`}
          onClick={() => setActiveTab("forYou")}
        >
          <span>For You</span>
        </div>
        <div
          className={`h-12 lg:h-16 flex-1 flex justify-center items-center cursor-pointer hover:text-gray-300 ${
            activeTab === "following"
              ? "border-b-4 border-blue-500 text-gray-300"
              : ""
          }`}
          onClick={() => setActiveTab("following")}
        >
          <span>Following</span>
        </div>
      </div>
      <div className="">
        <AddPost />
      </div>
      <div className="">
        {isLoading ? (
          <div className="p-4 space-y-2">
            {[...Array(5)].map((_, index) => (
              <PostSkeleton key={index} />
            ))}
          </div>
        ) : (
          posts.map((data) =>
            data.isRetweet ? (
              <Post
                key={data.retweetedBy}
                post={data.post}
                isRetweet={true}
                retweetContent={data.retweetContent}
                retweetedBy={data.retweetedBy}
                UserDetails={currentUser}
              />
            ) : (
              <Post
                key={data.post.id}
                post={data.post}
                UserDetails={currentUser}
              />
            )
          )
        )}
      </div>
    </section>
  );
};

export default UserPage;
