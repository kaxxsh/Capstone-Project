"use client";
import React, { useEffect, useState } from "react";
import { BASE_URL } from "@/config";

const Followers = ({ params }) => {
  const [data, setdata] = useState({});
  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch(
        `${BASE_URL}/api/UserFollow/followers/${params.UserName}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
          credentials: "include",
        }
      );
      console.log(response);
      const data = await response.json();
      setdata(data);
    };
    fetchData();
  }, []);

  return (
    <section className="p-6 max-w-xl mx-auto">
      <h2 className="text-2xl font-bold mb-4">Following</h2>
      {data.length > 0 ? (
        data.map((user) => (
          <div
            key={user.userId}
            className="flex items-center justify-between m-6 p-4 "
          >
            <div className="flex">
              <img
                src={user.profileImage}
                alt={`${user.userName}'s profile`}
                className="w-12 h-12 rounded-full mr-4"
              />
              <div className="">
                <div className="flex gap-3 items-center">
                  <div className="font-semibold text-lg">{user.name}</div>
                  <div className="text-gray-500">@{user.userName}</div>
                </div>
                <div className="">{user.bio}</div>
              </div>
            </div>
            <div className="">
              <button className=" bg-gray-100 text-black px-4 py-2 rounded-full">
                {user.isFollowed ? "Unfollow" : "Follow"}
              </button>
            </div>
          </div>
        ))
      ) : (
        <p>No users followed yet.</p>
      )}
    </section>
  );
};

export default Followers;
