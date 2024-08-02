import React, { useState, useEffect, useRef } from "react";
import { FaHeart } from "react-icons/fa";
import { BiSolidMessageSquare } from "react-icons/bi";
import { AiOutlineRetweet } from "react-icons/ai";
import { FiShare } from "react-icons/fi";
import { BASE_URL } from "@/config";
import Link from "next/link";
import PostModel from "@/Components/Nav/PostModel";

const Post = ({
  post,
  isRetweet,
  retweetContent,
  retweetedBy,
  onRetweetUpdate,
  UserDetails,
}) => {
  const {
    postId,
    content,
    image,
    dateCreated,
    likesCount,
    commentsCount,
    retweetsCount,
    user: { name, userName, profileImage },
    postLikes,
    postComments,
    postRetweets,
  } = post;

  const [likes, setLikes] = useState(likesCount);
  const [comments, setComments] = useState(commentsCount);
  const [retweets, setRetweets] = useState(retweetsCount);
  const [postLikesState, setPostLikesState] = useState(postLikes);
  const [postCommentsState, setPostCommentsState] = useState(postComments);
  const [postRetweetsState, setPostRetweetsState] = useState(postRetweets);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [commentContent, setCommentContent] = useState("");
  const [currentUser, setcurrentUser] = useState({});
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const dropdownRef = useRef(null);
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    setcurrentUser(UserDetails);
  }, [UserDetails]);

  const handleOpenModal = () => {
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
  };

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setDropdownOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [dropdownRef]);

  const handleLike = async () => {
    try {
      const hasLiked = postLikesState.some(
        (like) => like.userName === currentUser?.userName
      );

      if (hasLiked) {
        await fetch(`${BASE_URL}/api/PostLike/${postId}`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          credentials: "include",
        });
        setLikes((prev) => prev - 1);
        setPostLikesState((prev) =>
          prev.filter((like) => like.userName !== currentUser?.userName)
        );
      } else {
        await fetch(`${BASE_URL}/api/PostLike/${postId}`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          credentials: "include",
        });
        setLikes((prev) => prev + 1);
        setPostLikesState((prev) => [...prev, currentUser]);
      }
    } catch (error) {
      console.error("Error liking/unliking the post", error);
    }
  };

  const handleRetweet = async () => {
    try {
      const hasRetweeted = postRetweetsState.some(
        (retweet) => retweet.userName === currentUser?.userName
      );

      if (hasRetweeted) {
        await fetch(`${BASE_URL}/api/PostRetweet/${postId}`, {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
          },
          credentials: "include",
        });
        setRetweets((prev) => prev - 1);
        setPostRetweetsState((prev) =>
          prev.filter((retweet) => retweet.userName !== currentUser?.userName)
        );
        onRetweetUpdate(postId, false);
      } else {
        await fetch(`${BASE_URL}/api/PostRetweet`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ postId }),
          credentials: "include",
        });
        setRetweets((prev) => prev + 1);
        setPostRetweetsState((prev) => [...prev, currentUser]);
        onRetweetUpdate(postId, true);
      }
    } catch (error) {
      console.error("Error retweeting/un-retweeting the post", error);
    }
  };

  const handleComment = async (commentContent) => {
    try {
      if (commentContent) {
        await fetch(`${BASE_URL}/api/PostComment`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ postId, content: commentContent }),
          credentials: "include",
        });
        setComments((prev) => prev + 1);
        setPostCommentsState((prev) => [
          ...prev,
          { userName: currentUser.userName, content: commentContent },
        ]);
      }
    } catch (error) {
      console.error("Error commenting on the post", error);
    }
  };

  const handleShare = () => {
    if (navigator.share) {
      navigator.share({
        title: `Post by ${name}`,
        text: content,
        url: window.location.href,
      });
    } else {
      alert("Sharing is not supported in this browser.");
    }
  };

  const toggleDialog = () => {
    setIsDialogOpen((prev) => !prev);
  };

  const hasLiked = postLikesState.some(
    (like) => like.userName === currentUser?.userName
  );

  const hasCommented = postCommentsState.some(
    (comment) => comment.userName === currentUser?.userName
  );
  const hasRetweeted = postRetweetsState.some(
    (retweet) => retweet.userName === currentUser?.userName
  );

  const getTimeSincePost = () => {
    const currentDate = new Date();
    const postDate = new Date(dateCreated);
    const timeDiff = Math.abs(currentDate - postDate);
    const minutes = Math.floor(timeDiff / (1000 * 60));
    const hours = Math.floor(timeDiff / (1000 * 60 * 60));
    const days = Math.floor(timeDiff / (1000 * 60 * 60 * 24));

    if (minutes < 60) {
      return `${minutes}m ago`;
    } else if (hours < 24) {
      return `${hours}h ago`;
    } else {
      return `${days}d ago`;
    }
  };

  const toggleDropdown = () => {
    setDropdownOpen(!dropdownOpen);
  };

  const handledelete = async (postId) => {
    try {
      const response = await fetch(`${BASE_URL}/api/PostFeed/${postId}`, {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (response.ok) {
        window.location.reload();
      } else {
        console.error("Post deletion failed");
      }
    } catch (error) {
      console.error("An error occurred during post deletion:", error);
    }
  };

  return (
    <>
      <section className=" text-[#E1E8ED] mb-4">
        {isRetweet && (
          <div className="ml-4">
            <span className="text-[#8899A6] text-sm">
              Retweeted by <strong>@{retweetedBy}</strong>
            </span>
          </div>
        )}
        <div
          className="p-4 cursor-pointer"
          onClick={() => (window.location.href = `/User/${postId}`)}
        >
          <div className="flex items-start mb-4">
            <div className="w-12 h-12 mr-4">
              <img
                src={profileImage}
                alt="Profile"
                className="rounded-full w-full h-full object-cover"
              />
            </div>
            <div className="flex-grow">
              <div className="flex mb-1">
                <div className="">
                  <div className="mr-2">{name}</div>
                  <div className="text-[#8899A6] text-sm">@{userName}</div>
                </div>
                <div className="mx-2 text-[#8899A6]">•</div>
                <div className="text-[#8899A6] text-xs pt-1">
                  {getTimeSincePost()}
                </div>
              </div>
              <div>{content}</div>
            </div>
          </div>
          {image && (
            <div className="flex justify-center mb-4">
              <img
                src={image}
                alt="Post content"
                className="rounded-lg w-full max-w-[80%]"
              />
            </div>
          )}
        </div>
        <div className="flex justify-around text-[#8899A6] p-2 border-y border-[#38444D]">
          <button
            className={`flex items-center space-x-2 ${
              hasCommented ? "text-[#1DA1F2]" : "hover:text-[#1DA1F2]"
            }`}
            onClick={toggleDialog}
          >
            <BiSolidMessageSquare size={20} />
            <span>{comments}</span>
          </button>
          <button
            className={`flex items-center space-x-2 ${
              hasRetweeted ? "text-[#17BF63]" : "hover:text-[#17BF63]"
            }`}
            onClick={handleRetweet}
          >
            <AiOutlineRetweet size={20} />
            <span>{retweets}</span>
          </button>
          <button
            className={`flex items-center space-x-2 ${
              hasLiked ? "text-[#E0245E]" : "hover:text-[#E0245E]"
            }`}
            onClick={handleLike}
          >
            <FaHeart size={20} />
            <span>{likes}</span>
          </button>
          <div className="flex gap-3">
            <button
              className="flex items-center space-x-2 hover:text-[#1DA1F2]"
              onClick={handleShare}
            >
              <FiShare size={16} />
            </button>
            {currentUser?.userName === userName && (
              <button className="text-xs" onClick={toggleDropdown}>
                ...
              </button>
            )}
            {retweetedBy === userName && (
              <button className="text-xs" onClick={toggleDropdown}>
                ...
              </button>
            )}
          </div>
        </div>
      </section>

      <div className="relative" ref={dropdownRef}>
        {dropdownOpen && (
          <div
            id="dropdownDotsHorizontal"
            className="z-50 absolute bg-white divide-y divide-gray-100 rounded-lg shadow w-44 dark:bg-gray-700 dark:divide-gray-600 bottom-14 right-20"
          >
            <ul
              className="py-2 text-sm text-gray-700 dark:text-gray-200"
              aria-labelledby="dropdownMenuIconHorizontalButton"
            >
              <li>
                <Link
                  href="#"
                  className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white"
                  onClick={() => handleOpenModal()}
                >
                  Update Post
                </Link>
              </li>
            </ul>
            <div className="py-2">
              <Link
                href="#"
                className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-200 dark:hover:text-white"
                onClick={() => handledelete(postId)}
              >
                Delete
              </Link>
            </div>
          </div>
        )}
      </div>
      {isDialogOpen && (
        <div className="fixed inset-0 flex items-center justify-center z-50 bg-black bg-opacity-50">
          <div className="bg-[#1da1f2] p-6 rounded-lg shadow-lg w-96">
            <h2 className="text-2xl mb-4">Add Comment</h2>
            <textarea
              className="w-full p-2 mb-4 border border-gray-300 rounded-lg"
              rows="4"
              value={commentContent}
              onChange={(e) => setCommentContent(e.target.value)}
            ></textarea>
            <div className="flex justify-end">
              <button
                className="px-4 py-2 bg-[#0b0c0c] text-white rounded-lg mr-2"
                onClick={toggleDialog}
              >
                Cancel
              </button>
              <button
                className="px-4 py-2 bg-[#1da1f2] text-white rounded-lg"
                onClick={() => {
                  handleComment(commentContent);
                  toggleDialog();
                }}
              >
                Submit
              </button>
            </div>
          </div>
        </div>
      )}
      {showModal && <PostModel onClose={handleCloseModal} post={post} />}
    </>
  );
};

export default Post;
