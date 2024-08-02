import React, { useState, useEffect } from "react";
import PhoneInput from "react-phone-input-2";
import "react-phone-input-2/lib/style.css";
import Select from "react-select";
import countryList from "react-select-country-list";

const UserProfileModal = ({ userData, isOpen, onClose, onUpdate }) => {
  const [formData, setFormData] = useState(userData);
  const [selectedMedia, setSelectedMedia] = useState(null);
  const [imageUrl, setImageUrl] = useState(userData.profileImage);
  const countryOptions = countryList().getData();

  useEffect(() => {
    setFormData(userData);
    setImageUrl(userData.profileImage);
  }, [userData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    setSelectedMedia(file);
    setImageUrl(URL.createObjectURL(file));
  };

  const handleCloudinaryUpload = async () => {
    if (selectedMedia) {
      const formData = new FormData();
      formData.append("file", selectedMedia);
      formData.append("upload_preset", "kratos");

      const response = await fetch(
        "https://api.cloudinary.com/v1_1/dy33jpyuv/image/upload",
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

  const handleSubmit = async () => {
    const profileImageUrl = selectedMedia
      ? await handleCloudinaryUpload()
      : imageUrl;
    const updatedData = { ...formData, profileImage: profileImageUrl };
    onUpdate(updatedData);
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-gray-900 bg-opacity-80 overflow-y-auto no-scrollbar">
      <div className="relative bg-black text-white rounded-lg shadow-lg w-full max-w-3xl p-6 space-y-4">
        <button
          onClick={onClose}
          className="absolute lg:top-4  lg:right-4 text-gray-400 hover:text-white focus:outline-none"
          aria-label="Close"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            className="h-6 w-6"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
            strokeWidth="2"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M6 18L18 6M6 6l12 12"
            />
          </svg>
        </button>
        <h2 className="text-2xl font-bold text-center">Update Profile</h2>
        <div className="flex justify-center mb-4">
          <label className="cursor-pointer">
            <img
              src={imageUrl || "https://via.placeholder.com/150"}
              alt="Profile"
              className="w-24 h-24 rounded-full border-2 border-gray-300 object-cover"
            />
            <input
              type="file"
              accept="image/*"
              onChange={handleFileChange}
              className="hidden"
            />
          </label>
        </div>
        <form className="space-y-4">
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label className="font-bold text-white text-xl">Name</label>
              <input
                type="text"
                name="name"
                value={formData.name}
                onChange={handleChange}
                className="w-full p-2 bg-transparent border-b border-gray-500 focus:outline-none focus:border-indigo-500"
              />
            </div>
            <div>
              <label className="font-bold text-white text-xl">Username</label>
              <input
                type="text"
                name="userName"
                value={formData.userName}
                onChange={handleChange}
                className="w-full p-2 bg-transparent border-b border-gray-500 focus:outline-none focus:border-indigo-500"
              />
            </div>
          </div>
          <div>
            <label className="font-bold text-white text-xl">Bio</label>
            <textarea
              name="bio"
              value={formData.bio}
              onChange={handleChange}
              className="w-full p-2 bg-transparent border-b border-gray-500 focus:outline-none focus:border-indigo-500"
            ></textarea>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label className="font-bold text-white text-xl">Location</label>
              <input
                type="text"
                name="location"
                value={formData.location}
                onChange={handleChange}
                className="w-full p-2 bg-transparent border-b border-gray-500 focus:outline-none focus:border-indigo-500"
              />
            </div>
            <div>
              <label className="font-bold text-white text-xl">
                Date of Birth
              </label>
              <input
                type="date"
                name="dateOfBirth"
                value={formData.dateOfBirth}
                onChange={handleChange}
                className="w-full p-2 bg-transparent border-b border-gray-500 focus:outline-none focus:border-indigo-500"
              />
            </div>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label className="font-bold text-white text-xl">Gender</label>
              <select
                name="gender"
                value={formData.gender}
                onChange={handleChange}
                className="w-full p-2 bg-gray-700 border-b  border-gray-500 focus:outline-none focus:border-indigo-500"
              >
                <option value="">Select Gender</option>
                <option value="Male">Male</option>
                <option value="Female">Female</option>
                <option value="Other">Other</option>
              </select>
            </div>
            <div>
              <label className="font-bold text-white text-xl">Country</label>
              <Select
                options={countryOptions}
                value={countryOptions.find(
                  (option) => option.value === formData.country
                )}
                onChange={(option) =>
                  setFormData({ ...formData, country: option.value })
                }
                className="w-full p-2 border-b before:bg-gray-700 border-gray-500 focus:outline-none focus:border-indigo-500 text-black"
              />
            </div>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label className="font-bold text-white text-xl">Email</label>
              <input
                type="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                className="w-full p-2 bg-transparent border-b border-gray-500 focus:outline-none focus:border-indigo-500"
              />
            </div>
            <div>
              <label className="font-bold text-white text-xl">
                Phone Number
              </label>
              <PhoneInput
                country={"us"}
                value={formData.phoneNumber}
                onChange={(phone) =>
                  setFormData({ ...formData, phoneNumber: phone })
                }
                inputClass="w-full p-2 bg-transparent border-b border-gray-500 focus:outline-none focus:border-indigo-500"
              />
            </div>
          </div>
          <div>
            <button
              type="button"
              onClick={handleSubmit}
              className="w-full py-2 px-4 bg-gray-100 text-black font-semibold rounded-md hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
            >
              Update
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default UserProfileModal;
