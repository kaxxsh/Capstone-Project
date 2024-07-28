import React from 'react'

const HashtagList = ({data}) => {
  return (
    <section>
      <div className="border border-zinc-800 mt-8 p-4 rounded-xl">
        <h1 className="text-xl font-bold">What's happening</h1>
        <div className="flex flex-col space-y-4 mt-4">
          {data.map((tag) => (
            <div key={tag.hashtagId} className="">
              <p className="text-xl text-gray-700 hover:text-gray-300 transition-colors duration-300">{tag.tag.toUpperCase()}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

export default HashtagList