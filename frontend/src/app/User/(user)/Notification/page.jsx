import { BASE_URL } from "@/config";

const Notification = async () => {
  const data = await fetch(
    `${BASE_URL}/api/Notification/user/2e4f85cd-a264-4552-be14-f8b56f877a2f`
  ).then((res) => res.json());

  console.log(data);

  return (
    <section className="w-full text-white rounded-lg shadow-md">
      <h1 className="text-2xl font-bold mb-4 p-4 border-b border-gray-700">
        Notifications
      </h1>
      <div className="">
        {data.length === 0 ? (
          <p className="text-gray-400">No notifications available.</p>
        ) : (
          data.map((notification) => (
            <div
              key={notification.notifyId}
              className={`p-4 border-b border-gray-700 ${
                notification.isRead ? "bg-gray-800" : ""
              }`}
            >
              <p className="relative left-0 text-xs text-gray-500">
                {new Date(notification.dateCreated).toLocaleString()}
              </p>
              <p className="font-medium">{notification.content}</p>
            </div>
          ))
        )}
      </div>
    </section>
  );
};

export default Notification;
