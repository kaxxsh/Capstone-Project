import Nav from "@/Components/Nav/page";
import Search from "@/Components/Search/page";

export default function RootLayout({ children }) {
  return (
    <html lang="en">
      <body>
        <div className="lg:flex lg:w-full lg:justify-center lg:h-screen">
          <div className="w-[80%] lg:flex lg:h-full lg:justify-between">
            <div className="lg:w-[20%] border-r border-zinc-800 flex-shrink-0">
              <Nav />
            </div>
            <main className="lg:w-full lg:overflow-y-auto lg:no-scrollbar">
              {children}
            </main>
            <div className="lg:w-[60%] flex justify-center border-l border-zinc-800">
              <Search />
            </div>
          </div>
        </div>
      </body>
    </html>
  );
}
