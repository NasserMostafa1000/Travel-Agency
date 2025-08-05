import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { SiteName } from "../Constant";
import {
  FiHome,
  FiUser,
  FiShoppingBag,
  FiLogIn,
  FiLogOut,
  FiFileText,
  FiUserPlus,
  FiMenu,
  FiX,
} from "react-icons/fi";

export default function Nav() {
  const [isOpen, setIsOpen] = useState(false);
  const [isScrolled, setIsScrolled] = useState(false);

  useEffect(() => {
    const handleScroll = () => setIsScrolled(window.scrollY > 10);
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  const HandleLogOut = () => {
    localStorage.removeItem("Id");
    localStorage.removeItem("Token");
    window.location.reload();
  };

  const toggleSidebar = () => setIsOpen(!isOpen);
  const closeSidebar = () => setIsOpen(false);

  const isLoggedIn = localStorage.getItem("Token");

  const links = [
    { to: "/Home", icon: <FiHome />, text: "Home" },
    !isLoggedIn && {
      to: "/Login",
      icon: <FiLogIn />,
      text: "Login",
      color: "green",
    },
    !isLoggedIn && {
      to: "/signup",
      icon: <FiUserPlus />,
      text: "Sign Up",
      isButton: true,
    },
    isLoggedIn && { to: "/MyProfile", icon: <FiUser />, text: "Profile" },
    isLoggedIn && { to: "/Orders", icon: <FiShoppingBag />, text: "Orders" },
    isLoggedIn && {
      to: "/LogOut",
      icon: <FiLogOut />,
      text: "Logout",
      color: "red",
      onClick: HandleLogOut,
    },
    {
      to: "/OurPrivacyAndTerms",
      icon: <FiFileText />,
      text: "Privacy & Terms",
    },
  ].filter(Boolean);

  return (
    <>
      {/* Overlay */}
      {isOpen && (
        <div
          className="fixed inset-0 bg-black bg-opacity-50 z-40 lg:hidden"
          onClick={closeSidebar}
          aria-hidden="true"
        />
      )}

      <nav
        className={`fixed top-0 left-0 right-0 z-50 transition-all duration-300 ${
          isScrolled
            ? "bg-white shadow-md py-2"
            : "bg-white/90 backdrop-blur-sm py-3"
        }`}
      >
        <div className="container mx-auto px-4 sm:px-6 flex justify-between items-center">
          {/* Logo */}
          <Link
            to="/Home"
            className="flex items-center space-x-2 sm:space-x-3"
            onClick={closeSidebar}
          >
            <div className="w-10 h-10 sm:w-12 sm:h-12 rounded-full bg-white flex items-center justify-center overflow-hidden shadow-md transition-transform duration-300 group-hover:scale-110 hover:shadow-lg">
              <img
                src="/SalamaImage.webp"
                alt="Logo"
                className="w-full h-full object-cover filter hover:brightness-105"
                loading="lazy"
              />
            </div>
            <span className="text-lg sm:text-xl font-bold bg-gradient-to-r from-blue-600 to-indigo-600 bg-clip-text text-transparent font-['Poppins']">
              {SiteName}
            </span>
          </Link>

          {/* Desktop Navigation */}
          <ul className="hidden lg:flex items-center space-x-6">
            {links.map(
              ({ to, icon, text, color = "indigo", onClick, isButton }, idx) =>
                isButton ? (
                  <li key={idx}>
                    <Link
                      to={to}
                      onClick={closeSidebar}
                      className="inline-flex items-center bg-gradient-to-r from-green-500 to-emerald-500 text-white px-4 py-1.5 rounded-full shadow hover:shadow-md transition-all text-sm sm:text-base"
                    >
                      {React.cloneElement(icon, { className: "mr-2 text-lg" })}
                      <span className="font-medium">{text}</span>
                    </Link>
                  </li>
                ) : (
                  <li key={idx}>
                    <NavLink
                      to={to}
                      icon={icon}
                      text={text}
                      color={color}
                      onClick={onClick}
                    />
                  </li>
                )
            )}
          </ul>

          {/* Mobile Menu Button */}
          <button
            onClick={toggleSidebar}
            className="lg:hidden p-2 text-gray-700 hover:text-indigo-600 transition-colors"
            aria-label={isOpen ? "Close menu" : "Open menu"}
            aria-expanded={isOpen}
          >
            {isOpen ? <FiX size={24} /> : <FiMenu size={24} />}
          </button>
        </div>

        {/* Mobile Sidebar */}
        <div
          className={`fixed top-0 left-0 h-full w-64 bg-white shadow-xl z-50 transition-transform duration-300 ease-in-out ${
            isOpen ? "translate-x-0" : "-translate-x-full"
          }`}
        >
          <div className="p-4 border-b border-gray-200 flex justify-between items-center">
            <Link
              to="/Home"
              className="flex items-center space-x-2"
              onClick={closeSidebar}
            >
              <div className="w-10 h-10 rounded-full bg-white flex items-center justify-center overflow-hidden shadow-md">
                <img
                  src="/SalamaImage.webp"
                  alt="Logo"
                  className="w-full h-full object-cover"
                  loading="lazy"
                />
              </div>
              <span className="text-xl font-bold bg-gradient-to-r from-blue-600 to-indigo-600 bg-clip-text text-transparent font-['Poppins']">
                {SiteName}
              </span>
            </Link>
            <button
              onClick={closeSidebar}
              aria-label="Close menu"
              className="p-2 text-gray-600 hover:text-indigo-600 transition-colors"
            >
              <FiX size={24} />
            </button>
          </div>

          <nav className="p-4">
            <ul className="space-y-2">
              {links.map(
                (
                  { to, icon, text, color = "indigo", onClick, isButton },
                  idx
                ) => (
                  <li key={idx}>
                    {isButton ? (
                      <Link
                        to={to}
                        onClick={() => {
                          onClick?.();
                          closeSidebar();
                        }}
                        className="flex items-center justify-center bg-gradient-to-r from-green-500 to-emerald-500 text-white px-4 py-3 rounded-full shadow hover:shadow-md transition-all text-base font-medium w-full"
                      >
                        {React.cloneElement(icon, {
                          className: "mr-3 text-lg",
                        })}
                        {text}
                      </Link>
                    ) : (
                      <Link
                        to={to}
                        onClick={() => {
                          onClick?.();
                          closeSidebar();
                        }}
                        className={`flex items-center px-4 py-3 rounded-md transition-colors ${
                          color === "red"
                            ? "hover:bg-red-50 hover:text-red-600"
                            : "hover:bg-indigo-50 hover:text-indigo-600"
                        }`}
                      >
                        {React.cloneElement(icon, {
                          className: "mr-3 text-lg",
                        })}
                        <span className="font-medium text-base">{text}</span>
                      </Link>
                    )}
                  </li>
                )
              )}
            </ul>
          </nav>
        </div>
      </nav>
    </>
  );
}

function NavLink({ to, icon, text, color = "indigo", onClick }) {
  const colorClasses = {
    indigo: "text-gray-700 hover:text-indigo-600",
    green: "text-gray-700 hover:text-green-600",
    red: "text-gray-700 hover:text-red-600",
  };

  return (
    <Link
      to={to}
      className={`flex items-center ${colorClasses[color]} transition-colors group px-2 py-1 rounded-md hover:bg-gray-100`}
      onClick={onClick}
    >
      {React.cloneElement(icon, {
        className: "mr-2 text-lg group-hover:scale-110 transition-transform",
      })}
      <span className="font-medium text-sm sm:text-base">{text}</span>
    </Link>
  );
}
