import { NextResponse } from "next/server";

export async function middleware(req) {
  const res = NextResponse.next();

  if (req.nextUrl.pathname === "/user") {
    try {
      const token = req.cookies.get("token")?.value;
      if (!token) {
        return NextResponse.redirect(new URL("/login", req.url));
      }
    } catch (error) {
      console.log(error);
    }
  }

  
  if (req.nextUrl.pathname === "/login") {
    try {
      const token = req.cookies.get("token")?.value;
      if (token) {
        return NextResponse.redirect(new URL("/user", req.url));
      }
    } catch (error) {
      console.log(error);
    }
  }

  return res;
}
