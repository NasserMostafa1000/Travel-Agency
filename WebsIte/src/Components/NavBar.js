import { useState } from "react";
import "../Styles/Nav.css";
import { Link } from "react-router-dom";

export default function Nav() {
  const [isOpen, setIsOpen] = useState(false);

  const HandleLogOut = () => {
    window.localStorage.removeItem("Id");
    window.localStorage.removeItem("Token");
  };
  const toggleSidebar = () => {
    setIsOpen(!isOpen);
  };

  return (
    <nav className="navbar">
      {/* الزر الذي يحتوي على ثلاث شرط */}
      <div className="hamburger" onClick={toggleSidebar}>
        <span className="bar"></span>
        <span className="bar"></span>
        <span className="bar"></span>
      </div>

      {/* القائمة الجانبية */}
      <div className={`sidebar ${isOpen ? "open" : ""}`}>
        <ul className="sidebar-links">
          <li onClick={toggleSidebar}>
            <Link to="/Home">Home 🏠</Link>
          </li>

          {!localStorage.getItem("Token") ? (
            <>
              <li onClick={toggleSidebar}>
                <Link to="/Login" style={{ color: "green" }}>
                  Login
                </Link>
              </li>
              <li onClick={toggleSidebar}>
                <Link to="/signup" style={{ color: "grey" }}>
                  Signup
                </Link>
              </li>
            </>
          ) : (
            <>
              <li onClick={toggleSidebar}>
                <Link to="/MyProfile">My Profile</Link>
              </li>
              <li onClick={toggleSidebar}>
                <Link to="/Orders">My Orders🛒</Link>
              </li>
              <li onClick={toggleSidebar}>
                <Link
                  to="/LogOut"
                  onClick={HandleLogOut}
                  style={{ color: "red" }}
                >
                  LogOut
                </Link>
              </li>
            </>
          )}
          <li onClick={toggleSidebar}>
            <Link to="/OurPrivacyAndTerms">Privacy And Terms</Link>
          </li>
        </ul>
      </div>
    </nav>
  );
}
