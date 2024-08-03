import { useState, useEffect } from "react";
import { MdPermMedia } from "react-icons/md";
import { HiMiniGif } from "react-icons/hi2";
import { BsEmojiGrinFill } from "react-icons/bs";
import GifPicker from "gif-picker-react";
import UserId from "@/Utils/tokenDecoder";
import { BASE_URL } from "@/config";

const PostModel = ({ onClose, post }) => {
  const [inputValue, setInputValue] = useState(post ? post.content : "");
  const [hashtags, setHashtags] = useState(post ? post.hashtags : []);
  const [showEmojiPicker, setShowEmojiPicker] = useState(false);
  const [showGifPicker, setShowGifPicker] = useState(false);
  const [selectedGif, setSelectedGif] = useState(post ? post.image : null);
  const [selectedMedia, setSelectedMedia] = useState(null);
  const [loggedInUserId, setLoggedInUserId] = useState("");
  const [data, setData] = useState({});
  const [isLoading, setIsLoading] = useState(true);

  console.log(post);

  useEffect(() => {
    const fetchUser = async () => {
      const userId = await UserId();
      setLoggedInUserId(userId);

      if (userId) {
        const response = await fetch(
          `${BASE_URL}/api/User/UserId?UserId=${userId}`,
          {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
            },
            credentials: "include",
          }
        );

        if (response.ok) {
          const userData = await response.json();
          setData(userData);
        }
      }

      setIsLoading(false);
    };

    fetchUser();
  }, []);

  const handleInputChange = (e) => {
    const text = e.target.value;
    setInputValue(text);
    const tags = text.match(/#[\w]+/g) || [];
    setHashtags(tags);
  };

  const handleEmojiSelect = (emoji) => {
    setInputValue((prev) => prev + emoji.native);
    setShowEmojiPicker(false);
  };

  const handleGifSelect = (gif) => {
    setSelectedGif(gif.url);
    setShowGifPicker(false);
  };

  const handleMediaUpload = () => {
    document.getElementById("mediaUploadInput").click();
  };

  const handleMediaChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (event) => {
        setSelectedMedia(event.target.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleCloudinaryUpload = async () => {
    if (selectedMedia) {
      const formData = new FormData();
      formData.append("file", selectedMedia);
      formData.append("upload_preset", "kratos");

      const response = await fetch(
        `https://api.cloudinary.com/v1_1/${process.env.NEXT_PUBLIC_CLOUDNARY_CLOUD_NAME}/image/upload`,
        {
          method: "POST",
          body: formData,
        }
      );

      if (response.ok) {
        const data = await response.json();
        return data.secure_url;
      } else {
        console.log("Failed to upload media to Cloudinary");
      }
    }
    return null;
  };

  const handlePost = async () => {
    let imageUrl = "";
    if (selectedMedia) {
      imageUrl = await handleCloudinaryUpload();
    } else if (selectedGif) {
      imageUrl = selectedGif;
    }

    const postData = {
      content: inputValue,
      image: imageUrl,
      hashtags: hashtags,
    };

    const endpoint = post
      ? `${BASE_URL}/api/PostFeed/${post.postId}`
      : `${BASE_URL}/api/PostFeed`;
    const method = post ? "PUT" : "POST";

    const response = await fetch(endpoint, {
      method: method,
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
      body: JSON.stringify(postData),
    });

    if (response.ok) {
      console.log(`Post ${post ? "updated" : "created"} successfully`);
      setInputValue("");
      setSelectedGif(null);
      setSelectedMedia(null);
      onClose(); // Close the modal after posting
    } else {
      console.error(`Failed to ${post ? "update" : "create"} post`);
    }
  };

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-secondary-600 bg-opacity-40 z-100 -top-5">
      <div className="bg-black rounded-lg shadow-lg p-6 max-w-lg w-full text-black">
        <div className="mb-4">
          <button
            onClick={onClose}
            className="text-gray-600 hover:text-gray-900"
          >
            Close
          </button>
        </div>
        {isLoading ? (
          <div className="animate-pulse flex space-x-4 p-4">
            <div className="rounded-full bg-gray-700 h-12 w-12"></div>
            <div className="flex-1 space-y-4 py-1">
              <div className="h-4 bg-gray-700 rounded w-3/4"></div>
              <div className="space-y-2">
                <div className="h-4 bg-gray-700 rounded"></div>
                <div className="h-4 bg-gray-700 rounded w-5/6"></div>
              </div>
            </div>
          </div>
        ) : (
          <div className="pb-4">
            <div className="flex items-start space-x-4">
              <div className="w-12 h-12">
                <img
                  src={data.profileImage}
                  alt="User Avatar"
                  className="w-full h-full rounded-full"
                />
              </div>
              <div className="flex-1">
                <input
                  type="text"
                  placeholder="What's happening?"
                  className="bg-black border border-gray-300 text-white placeholder-gray-500 rounded-lg p-2 w-full"
                  value={inputValue}
                  onChange={handleInputChange}
                />
                {/* {showEmojiPicker && (
                  <Picker onEmojiSelect={handleEmojiSelect} />
                )} */}
                {showGifPicker && (
                  <GifPicker
                    tenorApiKey={process.env.NEXT_PUBLIC_TENOR_API_KEY}
                    onGifClick={handleGifSelect}
                  />
                )}
                {selectedGif && <img src={selectedGif} alt="Selected GIF" />}
                {selectedMedia && (
                  <img src={selectedMedia} alt="Uploaded Media" />
                )}
                <input
                  type="file"
                  id="mediaUploadInput"
                  style={{ display: "none" }}
                  onChange={handleMediaChange}
                />
              </div>
            </div>
            <div className="flex justify-between items-center mt-4">
              <div className="flex space-x-4 text-gray-500">
                <MdPermMedia
                  size={24}
                  onClick={handleMediaUpload}
                  fill="#2196F3"
                />
                <HiMiniGif
                  size={24}
                  onClick={() => setShowGifPicker(!showGifPicker)}
                  fill="#2196F3"
                />
                {/* <BsEmojiGrinFill
                  size={24}
                  onClick={() => setShowEmojiPicker(!showEmojiPicker)}
                  fill="#2196F3"
                /> */}
              </div>
              <button
                className="bg-blue-500 hover:bg-blue-600 text-white font-semibold py-2 px-4 rounded-2xl transition-colors"
                onClick={handlePost}
              >
                {post ? "Update" : "Post"}
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default PostModel;
