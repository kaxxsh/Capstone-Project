import { BASE_URL } from "@/config";

const UserList = ({ data }) => {
  const ID = "2e4f85cd-a264-4552-be14-f8b56f877a2f";

  const handleUnfollow = async (name) => {
    console.log(name);
    await fetch(`http://localhost:5123/api/UserFollow/unfollow/${name}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
    });
  };

  return (
    <section className="border border-zinc-800 mt-8 p-4 rounded-xl">
      <h1 className="text-xl font-semibold mb-4">Who to follow</h1>
      {data.map((user) => (
        <div key={user.id} className="flex items-center mb-2 p-2">
          <img
            src={user.profileImage}
            alt={`${user.name}'s avatar`}
            className="w-12 h-12 rounded-full"
          />
          <div className="ml-4">
            <h2 className="font-medium">{user.name}</h2>
            <p className="text-sm text-gray-500">@{user.userName}</p>
          </div>
          <button
            className="ml-auto bg-slate-200 text-black font-bold px-4 py-1 rounded-full hover:bg-zinc-500 transition-colors"
            onClick={() => handleUnfollow(user.name)}
          >
            {user.following.some((follow) => follow.userId === ID)
              ? "Unfollow"
              : "Follow"}
          </button>
        </div>
      ))}
    </section>
  );
};

export default UserList;
