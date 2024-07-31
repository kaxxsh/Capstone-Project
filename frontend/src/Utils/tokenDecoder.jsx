"use server";
import { cookies } from "next/headers";
import { jwtDecode } from "jwt-decode";

const UserId = async () => {
  const { value } = cookies().get("jwt") || {};
  const { nameid } = jwtDecode(value);
  return nameid;
};

export default UserId;
