import { NextResponse } from "next/server";

export async function middleware(req) {
  const res = NextResponse.next();

  if (req.nextUrl.pathname === "/User" || req.nextUrl.pathname === "/User/explore" || req.nextUrl.pathname === "/User/notification" || req.nextUrl.pathname === "/User/message" || req.nextUrl.pathname === "/User/profile") {
    try {
      const token = req.cookies.get("jwt")?.value;
      if (!token) {
        return NextResponse.redirect(new URL("/login", req.url));
      }
    } catch (error) {
      console.log(error);
    }
  }

  
  if (req.nextUrl.pathname === "/login") {
    try {
      const token = req.cookies.get("jwt")?.value;
      if (token) {
        return NextResponse.redirect(new URL("/User", req.url));
      }
    } catch (error) {
      console.log(error);
    }
  }

  return res;
}
