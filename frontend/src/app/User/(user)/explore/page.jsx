import { FaSearch } from "react-icons/fa";

const Explore = () => {
  return (
    <section>
      <div className="">
        <div className="flex justify-center items-center border border-zinc-800 rounded-3xl p-4 m-6 pl-8 gap-3">
          <FaSearch className="text-gray-500" />
          <input
            type="text"
            className="border-none outline-none flex-grow p-1 bg-transparent placeholder:text-gray-700"
            placeholder="Search for people"
          />
        </div>
      </div>
      <div className="">
        <div className="">
          <div className="">Top HashTags</div>
        </div>
        <div className="">
          <div className="">
            <div className="">#Hashtag1</div>
            <div className="">#Hashtag2</div>
            <div className="">#Hashtag3</div>
          </div>
        </div>
      </div>
    </section>
  );
};

export default Explore;
