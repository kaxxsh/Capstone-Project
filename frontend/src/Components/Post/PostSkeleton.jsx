import React from "react";

const PostSkeleton = () => {
  return (
    <section className="text-white rounded-lg shadow-lg mb-4 animate-pulse">
      <div className="p-4">
        <div className="flex items-start mb-4">
          <div className="w-12 h-12 mr-4 bg-gray-700 rounded-full"></div>
          <div className="flex-grow">
            <div className="flex items-center mb-1">
              <div className="bg-gray-700 rounded w-1/3 h-6 mr-2"></div>
              <div className="bg-gray-700 rounded w-1/4 h-4"></div>
              <div className="mx-2 text-gray-700">•</div>
              <div className="bg-gray-700 rounded w-1/6 h-4"></div>
            </div>
            <div className="bg-gray-700 rounded h-6 w-full"></div>
          </div>
        </div>
        <div className="bg-gray-700 rounded-lg w-full h-48"></div>
      </div>
      <div className="flex justify-around text-gray-700 p-2 border-y border-gray-700">
        <div className="flex items-center space-x-2">
          <div className="bg-gray-700 rounded w-6 h-6"></div>
          <div className="bg-gray-700 rounded w-6 h-4"></div>
        </div>
        <div className="flex items-center space-x-2">
          <div className="bg-gray-700 rounded w-6 h-6"></div>
          <div className="bg-gray-700 rounded w-6 h-4"></div>
        </div>
        <div className="flex items-center space-x-2">
          <div className="bg-gray-700 rounded w-6 h-6"></div>
          <div className="bg-gray-700 rounded w-6 h-4"></div>
        </div>
        <div className="flex items-center space-x-2">
          <div className="bg-gray-700 rounded w-6 h-6"></div>
          <div className="bg-gray-700 rounded w-6 h-4"></div>
        </div>
      </div>
    </section>
  );
};

export default PostSkeleton;
