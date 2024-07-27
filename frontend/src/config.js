export const BASE_URL =
  process.env.NEXT_PUBLIC_NODE_ENV === "development"
    ? "https://localhost:7073"
    : process.env.NEXT_PUBLIC_BASE_URL || "";
