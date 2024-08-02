"use client";
import React, { useState, useEffect } from "react";
import { BASE_URL } from "@/config";
import Userdisplay from "./userdisplay";
import { IoArrowBack } from "react-icons/io5";

const PostPage = ({ params }) => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchPostData = async () => {
      try {
        const response = await fetch(
          `${BASE_URL}/api/PostFeed/${params.PostId}`
        );
        if (!response.ok) {
          throw new Error("Failed to fetch post data");
        }
        const data = await response.json();
        setData(data);
      } catch (error) {
        console.error(error);
      } finally {
        setLoading(false);
      }
    };

    fetchPostData();
  }, [params.PostId]);

  if (loading) {
    return (
      <section className="p-4">
        <div className="animate-pulse">
          <div className="p-1 m-2 text-2xl font-bold flex item-center gap-2">
            <div className="">
              <div className="w-8 h-8 bg-gray-300 rounded-full"></div>
            </div>
            <div className="w-32 h-6 bg-gray-300 rounded"></div>
          </div>
          <div className="pb-4 mb-4 border-b border-gray-700">
            <div className="w-full h-8 bg-gray-300 rounded mb-3"></div>
            <div className="mt-3 ml-3 pl-3 w-full h-4 bg-gray-300 rounded mb-2"></div>
            <div className="w-full h-64 bg-gray-300 rounded"></div>
            <div className="mt-4 text-gray-500 text-sm pl-3 w-32 h-4 bg-gray-300 rounded"></div>
          </div>
          <div className="flex justify-between text-gray-300 ml-4">
            <div className="w-full h-4 bg-gray-300 rounded"></div>
          </div>
          <div className="mt-4">
            <div className="border-b border-gray-700 mb-2 pl-4">
              <div className="w-32 h-6 bg-gray-300 rounded"></div>
            </div>
            {[...Array(3)].map((_, index) => (
              <div
                key={index}
                className="mb-4 pl-4 p-3 border-b border-gray-700"
              >
                <div className="flex items-start">
                  <div className="w-10 h-10 bg-gray-300 rounded-full mr-3"></div>
                  <div className="flex-1">
                    <div className="w-32 h-4 bg-gray-300 rounded mb-2"></div>
                    <div className="w-24 h-4 bg-gray-300 rounded mb-2"></div>
                    <div className="mt-2 text-gray-700 w-full h-4 bg-gray-300 rounded"></div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>
    );
  }

  if (!data) {
    return <div>Error loading post. Please try again later.</div>;
  }

  return (
    <section className="">
      <div className="p-1 m-2 text-2xl font-bold flex item-center gap-2">
        <div className="">
          <IoArrowBack className="pt-1" onClick={() => {history.back()}} />
        </div>
        Post
      </div>
      <div className="pb-4 mb-4 border-b border-gray-700">
        <Userdisplay user={data.userId} />
        <div className="mt-3 ml-6 pl-3 text-gray-400">
          <p>{data.content}</p>
        </div>
        {data.image && (
          <div className="flex justify-center mt-4 p-6">
            <img
              src={data.image}
              alt="Post content"
              className="rounded-lg max-w-full h-auto"
            />
          </div>
        )}
        <div className="mt-4 text-gray-500 text-sm pl-3">
          {new Date(data.dateCreated).toLocaleString()}
        </div>
      </div>
      <div className="flex justify-between text-gray-300 ml-4">
        <div>
          <span className="mr-2">{data.likesCount} Likes</span>
          <span className="mr-2">{data.commentsCount} Comments</span>
          <span>{data.retweetsCount} Retweets</span>
        </div>
      </div>
      <div className="mt-4">
        <div className="border-b border-gray-700">
          <h2 className="text-lg font-bold mb-2 pl-4">Comments</h2>
        </div>
        {data.postComments.map((comment) => (
          <div
            key={comment.postCommentId}
            className="mb-4 pl-4 p-3 border-b border-gray-700"
          >
            <div className="flex items-start">
              <img
                src={comment.profileImage}
                alt={`${comment.userName}'s profile`}
                className="w-10 h-10 rounded-full mr-3"
              />
              <div className="flex-1 justify-between">
                <div className="flex">
                  <div className="flex">
                    <div className="">
                      <div className="">{comment.name}</div>
                      <div className="text-gray-800 font-semibold">
                        @{comment.userName}
                      </div>
                    </div>
                    <div className="text-gray-500 text-sm">
                      {new Date(comment.dateCreated).toLocaleString()}
                    </div>
                  </div>
                </div>
                <div className="mt-2 text-gray-700">{comment.content}</div>
              </div>
            </div>
          </div>
        ))}
      </div>
    </section>
  );
};

export default PostPage;
