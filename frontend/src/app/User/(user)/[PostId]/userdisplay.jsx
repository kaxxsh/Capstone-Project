"use client";
import React, { useEffect, useState } from "react";
import UserId from "@/Utils/tokenDecoder";
import { BASE_URL } from "@/config";

const UserDisplay = ({ user }) => {
  const [data, setData] = useState({});
  const [loggedInUserId, setLoggedInUserId] = useState();
  const [hasFollow, setHasFollow] = useState(false);

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
    if (user) {
      const fetchData = async () => {
        try {
          const response = await fetch(
            `${BASE_URL}/api/User/UserId?UserId=${user.item.id}`
          );
          const result = await response.json();
          setData(result);
        } catch (error) {
          console.error(error);
        }
      };
      fetchData();
    }
  }, [user]);

  useEffect(() => {
    const check = data.following?.some(
      (follower) => follower.userId === loggedInUserId
    );
    setHasFollow(check);
  }, []);

  const handleFollowRequest = async (userName) => {
    try {
      const url = hasFollow
        ? `${BASE_URL}/api/UserFollow/unfollow/${userName}`
        : `${BASE_URL}/api/UserFollow/follow/${userName}`;
      const response = await fetch(url, {
        method: hasFollow ? "POST" : "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (response.ok) {
        setHasFollow(!hasFollow);
      }
    } catch (error) {
      console.error("Error updating follow status", error);
    }
  };
  return (
    <div className=" py-4 px-8">
      <div className="flex items-center justify-between">
        <div
          className="flex gap-2"
          onClick={() => {
            window.location.href = `/User/Profile/${data.id}`;
          }}
        >
          <img
            src={data.profileImage}
            alt={`${data.name}'s profile`}
            className="w-14 h-14 rounded-full"
          />
          <div className="ml-3 flex flex-col justify-center">
            <div className="font-bold">{data.name}</div>
            <div className="text-gray-500 text-sm">@{data.userName}</div>
          </div>
        </div>
        {loggedInUserId !== data.id ? (
          <button
            className="text-black bg-white p-3 px-8 rounded-3xl font-bold"
            aria-label={hasFollow ? "Unfollow" : "Follow"}
            onClick={() => handleFollowRequest(data.userName)}
          >
            {hasFollow ? "Unfollow" : "Follow"}
          </button>
        ) : (
          <button
            className="text-black bg-white p-3 px-8 rounded-3xl font-bold"
            aria-label={hasFollow ? "Unfollow" : "Follow"}
            onClick={() => {
              window.location.href = `/User/Profile/${data.id}`;
            }}
          >
            Profile
          </button>
        )}
      </div>
    </div>
  );
};

export default UserDisplay;
