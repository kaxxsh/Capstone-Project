import React, { useState, useEffect } from "react";

const HashtagList = ({ data }) => {
  const [isVisible, setIsVisible] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const timer = setTimeout(() => {
      setIsVisible(true);
      setIsLoading(false);
    }, 1000);

    return () => clearTimeout(timer);
  }, []);

  return (
    <section>
      {isLoading ? (
        <div className="flex flex-col justify-center items-center mt-8 space-y-4">
          <div className="w-full max-w-md p-4 border border-gray-800 rounded-xl animate-pulse">
            <div className="h-6 bg-gray-700 rounded mb-4"></div>
            <div className="space-y-4">
              <div className="h-4 bg-gray-700 rounded"></div>
              <div className="h-4 bg-gray-700 rounded"></div>
              <div className="h-4 bg-gray-700 rounded"></div>
            </div>
          </div>
        </div>
      ) : (
        isVisible && (
          <div className="border border-gray-800 mt-8 p-4 rounded-xl">
            <h1 className="text-xl font-bold">What's happening</h1>
            <div className="flex flex-col space-y-4 mt-4">
              {data.map((tag) => (
                <div
                  key={tag.hashtagId}
                  className=""
                  onClick={() => (window.location.href = `/User/explore`)}
                >
                  <p className="text-base text-gray-300 hover:text-gray-700 transition-colors duration-300">
                    {tag.tag.toUpperCase()}
                  </p>
                </div>
              ))}
            </div>
          </div>
        )
      )}
    </section>
  );
};

export default HashtagList;
