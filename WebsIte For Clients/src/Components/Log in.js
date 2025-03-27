import { useState } from "react";
import "../Styles/Login.css";

const _HttpClient = "https://ramysalama-001-site1.qtempurl.com/api/";
export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [notification, setNotification] = useState("");

  async function HandleLogin(e) {
    e.preventDefault();
    setNotification("");

    if (!email || !password) {
      setNotification("Email and password are required.");
      return;
    }

    try {
      const response = await fetch(`${_HttpClient}Users/Login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ UserName: email, Password: password }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.errorMessage || "Login failed.");
      }

      const data = await response.json();
      setNotification("Logged in successfully!");
      window.localStorage.setItem("Token", data.token);
      window.localStorage.setItem("Id", Number(data.clientID));
      window.location.pathname = "/Home";
    } catch (error) {
      setNotification(error.message);
    }
  }

  return (
    <div className="login-container">
      <div className="login-card">
        <img
          src="/Salama Image.webp"
          alt="Karm Al-Salama Logo"
          className="login-logo"
        />
        <h1 className="login-title">Welcome Back! 👋</h1>
        <p className="login-subtitle">Please login to your account</p>
        {notification && (
          <div
            className={
              notification === "Logged in successfully!"
                ? "Goodnotification"
                : "Badnotification"
            }
            style={{ marginTop: "90px" }} // تعديل المسافة
          >
            {notification}
          </div>
        )}
        <form className="login-form" onSubmit={HandleLogin}>
          <div className="form-group">
            <label htmlFor="email">Email Address</label>
            <input
              type="email"
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Enter your email"
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Enter your password"
              required
            />
          </div>
          <button type="submit" className="login-button">
            Login
          </button>
        </form>
        <div className="forgot-password-link">
          <p>
            <a href="/forgot-password">Forgot your password?</a>
          </p>
        </div>
        <div className="login-footer">
          <p>
            Don't have an account? <a href="/signup">Sign Up</a>
          </p>
        </div>
      </div>
    </div>
  );
}
