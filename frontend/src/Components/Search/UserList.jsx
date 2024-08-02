import { useState, useEffect } from "react";
import { BASE_URL } from "@/config";
import UserId from "@/Utils/tokenDecoder";

const UserList = ({ data }) => {
  const [loading, setLoading] = useState(true);
  const [userData, setUserData] = useState([]);
  const [loggedInUserId, setLoggedInUserId] = useState();

  const fetchRole = async () => {
    setLoggedInUserId(await UserId());
  };

  useEffect(() => {
    fetchRole();
  }, []);

  useEffect(() => {
    setUserData(data);
  }, [data]);

  useEffect(() => {
    const timer = setTimeout(() => {
      setLoading(false);
    }, 1000);

    return () => clearTimeout(timer);
  }, []);

  const handleFollowUnfollow = async (userName, action) => {
    const url = `${BASE_URL}/api/UserFollow/${action}/${userName}`;
    const response = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
    });

    if (response.ok) {
      setUserData((prevData) =>
        prevData.map((user) => {
          if (user.userName === userName) {
            if (action === "follow") {
              return {
                ...user,
                followersCount: user.followersCount + 1,
                following: [...user.following, { userId: loggedInUserId }],
              };
            } else {
              return {
                ...user,
                followersCount: user.followersCount - 1,
                following: user.following.filter(
                  (follow) => follow.userId !== loggedInUserId
                ),
              };
            }
          }
          return user;
        })
      );
    }
  };

  if (loading) {
    return (
      <div className="flex flex-col justify-center items-center mt-8 space-y-4">
        <div className="w-full max-w-md p-4 border border-gray-800 rounded-xl animate-pulse">
          <div className="h-6 bg-gray-700 rounded mb-4"></div>
          <div className="space-y-4">
            <div className="flex items-center space-x-4">
              <div className="w-12 h-12 bg-gray-700 rounded-full"></div>
              <div className="flex-1">
                <div className="h-4 bg-gray-700 rounded w-1/2 mb-2"></div>
                <div className="h-3 bg-gray-700 rounded w-1/4"></div>
              </div>
            </div>
            <div className="flex items-center space-x-4">
              <div className="w-12 h-12 bg-gray-700 rounded-full"></div>
              <div className="flex-1">
                <div className="h-4 bg-gray-700 rounded w-1/2 mb-2"></div>
                <div className="h-3 bg-gray-700 rounded w-1/4"></div>
              </div>
            </div>
            <div className="flex items-center space-x-4">
              <div className="w-12 h-12 bg-gray-700 rounded-full"></div>
              <div className="flex-1">
                <div className="h-4 bg-gray-700 rounded w-1/2 mb-2"></div>
                <div className="h-3 bg-gray-700 rounded w-1/4"></div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }

  return (
    <section className="border border-gray-800 mt-8 p-4 rounded-xl">
      <h1 className="text-xl font-semibold mb-4">Who to follow</h1>
      {userData
        .filter((user) => user.id !== loggedInUserId)
        .map((user) => (
          <div
            key={user.id}
            className="flex flex-col sm:flex-row items-center mb-2 p-2"
          >
            <img
              src={user.profileImage}
              alt={`${user.name}'s avatar`}
              className="w-12 h-12 rounded-full"
              onClick={() =>
                (window.location.href = `/User/Profile/${user.id}`)
              }
            />
            <div className="ml-4">
              <h2 className="font-medium">{user.name}</h2>
              <p className="text-sm text-gray-500">@{user.userName}</p>
            </div>
            <button
              className="mt-2 sm:mt-0 ml-auto bg-gray-100 text-black font-bold px-4 py-1 rounded-full hover:bg-gray-500 transition-colors"
              onClick={() =>
                handleFollowUnfollow(
                  user.userName,
                  user.following.some(
                    (follow) => follow.userId === loggedInUserId
                  )
                    ? "unfollow"
                    : "follow"
                )
              }
            >
              {user.following.some((follow) => follow.userId === loggedInUserId)
                ? "Unfollow"
                : "Follow"}
            </button>
          </div>
        ))}
    </section>
  );
};

export default UserList;
