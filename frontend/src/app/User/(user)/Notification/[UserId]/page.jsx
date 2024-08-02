"use client";
import { BASE_URL } from "@/config";
import { useEffect, useState } from "react";
import { IoArrowBack } from "react-icons/io5";

const Notification = ({ params }) => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);

  const handleFetchNotification = async () => {
    const data = await fetch(
      `${BASE_URL}/api/Notification/user/${params.UserId}`
    ).then((res) => res.json());
    setData(data);
    setLoading(false);
  };

  useEffect(() => {
    handleFetchNotification();
  }, []);

  const handlereadpost = async (Id, postId, user) => {
    const response = await fetch(`${BASE_URL}/api/Notification/${Id}`);
    if (response.ok) {
      handleFetchNotification();
      if (postId === "00000000-0000-0000-0000-000000000000") {
        window.location.href = `/User/Profile/${user}`;
      } else {
        window.location.href = `/User/${postId}`;
      }
    }
  };

  return (
    <section className="w-full text-white rounded-lg shadow-md">
      <div className="p-1 m-2 text-2xl font-bold flex item-center gap-2">
        <div className="">
          <IoArrowBack
            className="pt-1"
            onClick={() => {
              history.back();
            }}
          />
        </div>
        Notifications
      </div>
      <div className="">
        {loading ? (
          <div>
            {[...Array(10)].map((_, index) => (
              <div
                key={index}
                className="p-4 border-b border-gray-700 animate-pulse"
              >
                <div className="h-4 bg-gray-800 rounded w-1/2 mb-2"></div>
                <div className="h-6 bg-gray-800 rounded w-full"></div>
              </div>
            ))}
          </div>
        ) : data.length === 0 ? (
          <p className="text-gray-400">No notifications available.</p>
        ) : (
          data.map((notification) => (
            <div
              key={notification.notifyId}
              className={`p-4 border-b border-gray-700 ${
                notification.isRead ? "bg-gray-900 opacity-50" : ""
              }`}
              onClick={() =>
                handlereadpost(
                  notification.notifyId,
                  notification.postId,
                  notification.userId
                )
              }
            >
              <p className="relative left-0 text-xs text-gray-500">
                {new Date(notification.dateCreated).toLocaleString()}
              </p>
              <p className="font-medium">{notification.content}</p>
            </div>
          ))
        )}
      </div>
    </section>
  );
};

export default Notification;
