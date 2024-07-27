import Nav from "@/Components/Nav/page";
import Search from "@/Components/Search/page";

export default function RootLayout({ children }) {
  return (
    <html lang="en">
      <body>
        <Nav />
        <main className="flex-grow">{children}</main>
        <Search />
      </body>
    </html>
  );
}
