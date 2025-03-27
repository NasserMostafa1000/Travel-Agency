import { useState } from "react";
import "../Styles/Login.css";

const _HttpClient = "https://ramysalama-001-site1.qtempurl.com/api/";

export default function ForgetPassword() {
  const [email, setEmail] = useState("");
  const [notification, setNotification] = useState("");

  // دالة لتوليد كلمة مرور عشوائية
  function generateRandomPassword() {
    const length = 8; // تحديد طول كلمة المرور
    const characters =
      "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
    let password = "";
    for (let i = 0; i < length; i++) {
      password += characters.charAt(
        Math.floor(Math.random() * characters.length)
      );
    }
    return password;
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setNotification("");

    if (!email) {
      setNotification("Email is required.");
      return;
    }

    const newPassword = generateRandomPassword();

    try {
      const response = await fetch(`${_HttpClient}Users/ForgetPassword`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ UserName: email, Password: newPassword }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.errorMessage || "Failed to reset password.");
      }

      setNotification(
        `A new password has been sent to your email successfully: ${email} `
      );
    } catch (error) {
      setNotification(error.message);
    }
  }

  return (
    <div className="login-container">
      <div className="login-card">
        <h1 className="login-title">Forgot Password</h1>
        <p className="login-subtitle">
          Please enter your email address to reset your password
        </p>
        {notification && (
          <div
            className={
              notification.includes("successfully")
                ? "Goodnotification"
                : "Badnotification"
            }
            style={{ marginTop: "90px" }}
          >
            {notification}
          </div>
        )}
        <form className="login-form" onSubmit={handleSubmit}>
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
          <button type="submit" className="login-button">
            Reset Password
          </button>
        </form>
      </div>
    </div>
  );
}
