import Nav from "@/Components/Nav/page";
import Search from "@/Components/Search/page";

export default function RootLayout({ children }) {
  return (
    <html lang="en">
      <body>
        <script
          async
          src="https://unpkg.com/@material-tailwind/html@latest/scripts/ripple.js"
        ></script>
        <div className="lg:flex lg:w-full lg:justify-center lg:h-screen no-scrollbar font-mono font-extralight">
          <div className="lg:w-[70%] lg:flex lg:h-full lg:justify-between">
            <div className="lg:w-[20%]  flex-shrink-0 pr-4">
              <Nav />
            </div>
            <main className="w-full lg:w-[50%] overflow-y-auto no-scrollbar lg:mx-6 border-x border-gray-700">
              {children}
            </main>
            <div className="hidden lg:inline lg:w-[30%] flex justify-center">
              <Search />
            </div>
          </div>
        </div>
      </body>
    </html>
  );
}
