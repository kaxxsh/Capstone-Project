import React, { useState, useEffect } from "react";
import { FaHeart } from "react-icons/fa";
import { BiSolidMessageSquare } from "react-icons/bi";
import { AiOutlineRetweet } from "react-icons/ai";
import { FiShare } from "react-icons/fi";
import { BASE_URL } from "@/config";

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

  useEffect(() => {
    setcurrentUser(UserDetails);
  }, [UserDetails]);

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

  return (
    <>
      <section className="bg-[0b0c0c] text-[#E1E8ED] rounded-lg shadow-lg mb-4">
        {isRetweet && (
          <div className="p-2">
            <span className="text-[#8899A6]">
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
              <div className="flex items-center mb-1">
                <div className="font-bold text-lg mr-2">{name}</div>
                <div className="text-[#8899A6]">@{userName}</div>
                <div className="mx-2 text-[#8899A6]">•</div>
                <div className="text-[#8899A6]">
                  {new Date(dateCreated).toLocaleTimeString()}
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
          <button
            className="flex items-center space-x-2 hover:text-[#1DA1F2]"
            onClick={handleShare}
          >
            <FiShare size={20} />
          </button>
        </div>
      </section>

      {isDialogOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-80">
          <div className="rounded-lg shadow-lg p-4 w-1/3 border border-[#38444D] bg-[#15202B]">
            <div className="text-2xl font-semibold mb-4 text-[#E1E8ED]">
              Add a comment
            </div>
            <textarea
              className="w-full p-2 mb-4 border rounded-lg bg-[#15202B] text-[#E1E8ED] border-[#38444D]"
              placeholder="Write your comment here..."
              rows="4"
              onChange={(e) => setCommentContent(e.target.value)}
            />
            <div className="flex justify-end">
              <button
                className="px-4 py-2 bg-[#E0245E] text-white rounded-lg mr-2"
                onClick={toggleDialog}
              >
                Cancel
              </button>
              <button
                className="px-4 py-2 bg-[#17BF63] text-white rounded-lg"
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
    </>
  );
};

export default Post;
