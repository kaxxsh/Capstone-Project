"use client";
import React, { useEffect, useState } from "react";
import { BASE_URL } from "@/config";
import { IoArrowBack } from "react-icons/io5";

const FollowersSkeleton = () => (
  <div className="flex items-center justify-between m-6 p-3 animate-pulse">
    <div className="flex">
      <div className="w-12 h-12 rounded-full bg-gray-300 mr-4"></div>
      <div className="space-y-2">
        <div className="w-24 h-4 bg-gray-300"></div>
        <div className="w-16 h-3 bg-gray-300"></div>
        <div className="w-32 h-2 bg-gray-200"></div>
      </div>
    </div>
    <div>
      <div className="w-20 h-8 bg-gray-200 rounded-full"></div>
    </div>
  </div>
);

const Following = ({ params }) => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch(
          `${BASE_URL}/api/UserFollow/following/${params.UserName}`,
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
          setData(data);
        } else {
          console.error("Failed to fetch data");
        }
      } catch (error) {
        console.error("Error fetching data", error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [params.UserName]);

  const handleFollow = async (UserName, check) => {
    let url = check
      ? `${BASE_URL}/api/UserFollow/unfollow/${UserName}`
      : `${BASE_URL}/api/UserFollow/follow/${UserName}`;
    let method = "POST";
    const response = await fetch(url, {
      method,
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
    });
    if (response.ok) {
      setData((prevData) =>
        prevData.map((user) =>
          user.userName === UserName
            ? { ...user, isFollowed: !user.isFollowed }
            : user
        )
      );
      console.log(check ? "Unfollowed" : "Followed");
    } else {
      console.error("Failed to update follow status");
    }
  };

  return (
    <section className="p-6 max-w-xl mx-auto">
      <div className="p-1 m-2 text-2xl font-bold flex item-center gap-2">
        <div className="">
          <IoArrowBack
            className="pt-1"
            onClick={() => {
              history.back();
            }}
          />
        </div>
        Following
      </div>
      {loading ? (
        <>
          <FollowersSkeleton />
          <FollowersSkeleton />
          <FollowersSkeleton />
        </>
      ) : data.length > 0 ? (
        data.map((user) => (
          <div
            key={user.userId}
            className="flex items-center justify-between m-6 p-3"
          >
            <div
              className="flex"
              onClick={() =>
                (window.location.href = `/User/Profile/${user.userId}`)
              }
            >
              <img
                src={user.profileImage}
                alt={`${user.userName}'s profile`}
                className="w-12 h-12 rounded-full mr-4"
              />
              <div className="">
                <div className="flex gap-3 items-center">
                  <div className="font-semibold text-lg">{user.name}</div>
                  <div className="text-gray-500 text-sm">@{user.userName}</div>
                </div>
                <div className="text-gray-300 text-xs text-ellipsis">
                  {user.bio}
                </div>
              </div>
            </div>
            <div className="">
              <button
                className="bg-gray-100 text-black px-4 py-2 rounded-full"
                onClick={() => handleFollow(user.userName, user.isFollowed)}
              >
                {user.isFollowed ? "Unfollow" : "Follow"}
              </button>
            </div>
          </div>
        ))
      ) : (
        <p>No followers yet.</p>
      )}
    </section>
  );
};

export default Following;
