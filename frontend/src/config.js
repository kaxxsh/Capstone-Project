export const BASE_URL =
  process.env.NEXT_PUBLIC_NODE_ENV === "development"
    ? "http://localhost:5123"
    : process.env.NEXT_PUBLIC_BASE_URL || "";
