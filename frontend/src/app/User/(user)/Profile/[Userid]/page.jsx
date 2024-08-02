"use client";
import React, { useEffect, useState, useCallback, use } from "react";
import { BASE_URL } from "@/config";
import UserIdFromToken from "@/Utils/tokenDecoder";
import UserProfileModal from "./UserProfileModal";
import Post from "@/Components/Post/page";
import PostSkeleton from "@/Components/Post/PostSkeleton";
import { IoArrowBack } from "react-icons/io5";

const Profile = ({ params }) => {
  const [data, setData] = useState(null);
  const [isFollow, setIsFollow] = useState(false);
  const [error, setError] = useState(null);
  const [getUserId, setGetUserId] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [posts, setPosts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [CurrentuserDetails, setCurrentuserDetails] = useState({});

  useEffect(() => {
    const fetchPosts = async () => {
      setIsLoading(true);
      const url = `${BASE_URL}/api/PostFeed/GetPostByUser/${params.Userid}`;
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
  }, [data]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        if (!params || !params.Userid) {
          throw new Error("User ID is not provided.");
        }

        const response = await fetch(
          `${BASE_URL}/api/User/UserId?UserId=${params.Userid}`,
          {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
            },
            credentials: "include",
          }
        );

        if (!response.ok) {
          throw new Error("Failed to fetch user data.");
        }

        const result = await response.json();
        setData(result);
      } catch (error) {
        console.error("An error occurred during fetch:", error);
        setError("Error loading user data. Please try again later.");
      }
    };

    const fetchUserId = async () => {
      try {
        const tokenid = await UserIdFromToken();
        setGetUserId(tokenid);
      } catch (error) {
        console.error("Error decoding token:", error);
        setGetUserId(null);
      }
    };

    fetchData();
    fetchUserId();
  }, [params]);

  useEffect(() => {
    if (data && getUserId) {
      setIsFollow(data.following.some((follow) => follow.userId === getUserId));
    }
  }, [data, getUserId]);
  useEffect(() => {
    const getcurrentUser = async () => {
      try {
        const response = await fetch(
          `${BASE_URL}/api/User/UserId?UserId=${getUserId}`,
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
          setCurrentuserDetails(data);
        }
      } catch (error) {
        console.error("Error fetching user data", error);
      }
    };
    getcurrentUser();
  }, [getUserId]);

  const handleFollow = useCallback(
    async (name) => {
      try {
        const url = `${BASE_URL}/api/UserFollow/${
          isFollow ? "unfollow" : "follow"
        }/${name}`;
        const response = await fetch(url, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          credentials: "include",
        });

        if (response.ok) {
          setIsFollow(!isFollow);
        } else {
          throw new Error("Failed to update follow status.");
        }
      } catch (error) {
        console.error("An error occurred while updating follow status:", error);
      }
    },
    [isFollow]
  );

  if (error) {
    return <div>{error}</div>;
  }

  if (!data) {
    return (
      <div className="flex flex-col items-center justify-center mt-8 space-y-4">
        <div className="w-full max-w-md p-4 border border-gray-800 rounded-xl animate-pulse">
          <div className="h-6 bg-gray-700 rounded mb-4"></div>
          <div className="flex items-center space-x-4">
            <div className="w-32 h-32 bg-gray-700 rounded-full"></div>
            <div className="flex-1">
              <div className="h-4 bg-gray-700 rounded w-1/2 mb-2"></div>
              <div className="h-3 bg-gray-700 rounded w-1/4"></div>
            </div>
          </div>
          <div className="mt-4 space-y-2">
            <div className="h-4 bg-gray-700 rounded w-1/2"></div>
            <div className="h-4 bg-gray-700 rounded w-1/3"></div>
            <div className="h-4 bg-gray-700 rounded w-1/4"></div>
          </div>
        </div>
      </div>
    );
  }

  const formattedDate = new Date(data.joinDate).toLocaleDateString("en-US", {
    year: "numeric",
    month: "long",
    day: "numeric",
  });

  const handleUpdate = async (updatedData) => {
    console.log(updatedData);
    const response = await fetch(`${BASE_URL}/api/User`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
      body: JSON.stringify(updatedData),
    });
    if (response.ok) {
      setData(updatedData);
    }
  };

  return (
    <section className="bg-black text-white pb-6 border-b border-gray-800">
      <div className="p-1 m-2 text-2xl font-bold flex item-center gap-2">
        <div className="">
          <IoArrowBack
            className="pt-1"
            onClick={() => {
              history.back();
            }}
          />
        </div>
        Profile
      </div>
      <div className="relative">
        <div className="h-28 bg-gray-800"></div>
        <div className="absolute -bottom-16 left-6">
          <div className="w-32 h-32 rounded-full border-4 border-black overflow-hidden">
            <img
              src={data.profileImage}
              alt="Profile"
              className="w-full h-full object-cover"
            />
          </div>
        </div>
      </div>
      <div className="mt-16 px-6 border-b border-gray-700 pb-4">
        <div className="flex justify-between items-center">
          <div>
            <h1 className="text-2xl font-bold">{data.name}</h1>
            <p className="text-gray-500">@{data.userName}</p>
          </div>
          {getUserId === data.id ? (
            <div>
              <button
                onClick={() => setIsModalOpen(true)}
                className="bg-white text-black rounded-full py-2 px-4 font-semibold"
              >
                Edit Profile
              </button>
              <UserProfileModal
                userData={data}
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onUpdate={handleUpdate}
              />
            </div>
          ) : (
            <button
              className="bg-white text-black rounded-full py-2 px-4 font-semibold"
              onClick={() => handleFollow(data.userName)}
            >
              {isFollow ? "Unfollow" : "Follow"}
            </button>
          )}
        </div>
        <div className="mt-4 text-gray-300">
          <p>{data.bio}</p>
        </div>
        <div className="mt-2 text-gray-500">
          <p>Joined {formattedDate}</p>
        </div>
        <div className="mt-4 flex space-x-8">
          <div
            className="flex items-center space-x-2"
            onClick={() =>
              (window.location.href = `/User/Profile/${params.Userid}/Followers/${data.userName}`)
            }
          >
            <span className="font-bold">{data.followersCount}</span>
            <span className="text-gray-500">Followers</span>
          </div>
          <div
            className="flex items-center space-x-2"
            onClick={() =>
              (window.location.href = `/User/Profile/${params.Userid}/Following/${data.userName}`)
            }
          >
            <span className="font-bold">{data.followingCount}</span>
            <span className="text-gray-500">Following</span>
          </div>
        </div>
      </div>
      <div className="">
        {isLoading ? (
          <div className="p-4 space-y-4">
            {[...Array(5)].map((_, index) => (
              <PostSkeleton key={index} />
            ))}
          </div>
        ) : (
          posts.map((item) =>
            item.isRetweet ? (
              <Post
                key={item.retweetedBy}
                post={item.post}
                isRetweet={true}
                retweetContent={item.retweetContent}
                retweetedBy={item.retweetedBy}
                UserDetails={CurrentuserDetails}
              />
            ) : (
              <Post
                key={item.post.id}
                post={item.post}
                UserDetails={CurrentuserDetails}
              />
            )
          )
        )}
      </div>
    </section>
  );
};

export default Profile;
