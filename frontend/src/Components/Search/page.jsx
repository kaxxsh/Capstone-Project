"use client";
import { useState, useEffect } from "react";
import { FaSearch } from "react-icons/fa";
import UserList from "./UserList";
import { BASE_URL } from "@/config";
import debounce from "lodash.debounce";
import HashtagList from "./HashtagList";

const SearchComponent = () => {
  const [search, setSearch] = useState("");
  const [data, setData] = useState([]);
  const [HashtagData, setHashtagData] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [isFocused, setIsFocused] = useState(false);

  const fetchData = debounce(async (searchTerm) => {
    setLoading(true);
    setError(null);
    try {
      const response = await fetch(
        `${BASE_URL}/api/User/Search?search=${searchTerm}`
      );
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      const json = await response.json();
      setData(json);
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  }, 300);

  const GetUnfollowedUser = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await fetch(`${BASE_URL}/api/User`);
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      const json = await response.json();
      setData(json);
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  };

  const GetTrendingHashTag = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await fetch(`${BASE_URL}/api/PostFeed/hashtags`);
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      const json = await response.json();
      const firstThreeTags = json.slice(0, 3);
      setHashtagData(firstThreeTags);
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (search) {
      fetchData(search);
    } else {
      setData([]);
    }
  }, [search]);

  useEffect(() => {
    if (!isFocused) {
      GetUnfollowedUser();
      GetTrendingHashTag();
    }
  }, []);

  return (
    <section className="block lg:inline">
      <div
        className={`flex items-center border ${
          isFocused ? "border-blue-600" : "border-gray-800"
        } rounded-3xl p-2 mt-4`}
      >
        <FaSearch
          className={`mx-2 ${isFocused ? "text-blue-600" : "text-gray-500"}`}
          size={14}
        />
        <input
          type="text"
          placeholder="Search"
          className="border-none outline-none p-1 bg-transparent"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          onFocus={() => setIsFocused(true)}
          onBlur={() => setIsFocused(false)}
        />
      </div>
      <HashtagList data={HashtagData} />
      <UserList data={data} />
    </section>
  );
};

export default SearchComponent;
