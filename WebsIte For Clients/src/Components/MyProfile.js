import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "../Styles/MyProfile.css";

const _HttpClient = "https://ramysalama-001-site1.qtempurl.com/api/";

export default function MyProfile() {
  const [isPasswordPanelOpen, setIsPasswordPanelOpen] = useState(false);
  const [currentPass, setCurrentPass] = useState("");
  const [isInfoPanelOpen, setIsInfoPanelOpen] = useState(false);
  const [newPass, setNewPass] = useState("");
  const [confirmPass, setConfirmPass] = useState("");
  const [errorNotify, setErrorNotify] = useState("");
  const [successNotify, setSuccessNotify] = useState("");
  const [clientData, setClientData] = useState(null);
  const [personalInfo, setPersonalInfo] = useState({
    clientID: 0,
    firstName: "",
    midName: "",
    lastName: "",
    phoneNumber: "",
    token: "",
  });
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const togglePasswordPanel = () => setIsPasswordPanelOpen((prev) => !prev);
  const toggleInfoPanel = () => setIsInfoPanelOpen((prev) => !prev);

  useEffect(() => {
    const fetchClientData = async () => {
      const token = localStorage.getItem("Token");
      const clientId = localStorage.getItem("Id");

      if (!token || !clientId) {
        setErrorNotify("Missing authentication details. Please login again.");
        return;
      }

      setIsLoading(true);
      try {
        const response = await fetch(`${_HttpClient}Clients/FindClient`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            clientId: Number(clientId),
            token: token,
          }),
        });

        if (!response.ok) {
          window.localStorage.removeItem("Id");
          window.localStorage.removeItem("Token");
          window.location.pathname = "/Login";
          throw new Error(
            `Failed to fetch client data: ${response.statusText}`
          );
        }

        const data = await response.json();
        setClientData(data);
        setPersonalInfo({
          clientID: data.clientID,
          firstName: data.firstName || "",
          midName: data.midName || "",
          lastName: data.lastName || "",
          phoneNumber: data.phoneNumber || "",
          token: token,
        });
      } catch (error) {
        setErrorNotify(`Error fetching client data: ${error.message}`);
      } finally {
        setIsLoading(false);
      }
    };

    fetchClientData();
  }, []);

  const handleChangePassword = async (e) => {
    e.preventDefault();

    if (newPass !== confirmPass) {
      setErrorNotify("Passwords do not match.");
      return;
    }

    try {
      const verifyResponse = await fetch(`${_HttpClient}Users/VerifyPassword`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          email: clientData?.email,
          password: currentPass,
        }),
      });

      if (!verifyResponse.ok) {
        setErrorNotify("The current password is not valid.");
        return;
      }

      const changeResponse = await fetch(`${_HttpClient}Users/PutInfo`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          username: clientData?.email,
          password: newPass,
          token: localStorage.getItem("Token"),
          clientId: Number(localStorage.getItem("Id")),
        }),
      });

      if (changeResponse.ok) {
        setSuccessNotify("Password updated successfully.");
        window.localStorage.removeItem("Id");
        window.localStorage.removeItem("Token");
        navigate("/Login");
      } else {
        setErrorNotify("Failed to update password.");
      }
    } catch (error) {
      setErrorNotify(`Error: ${error.message}`);
    }
  };

  const handleUpdateInfo = async (e) => {
    e.preventDefault();

    try {
      const response = await fetch(`${_HttpClient}Clients/PutClient`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          ...personalInfo,
          clientID: Number(localStorage.getItem("Id")),
          token: localStorage.getItem("Token"),
        }),
      });

      if (response.ok) {
        setSuccessNotify("Personal information updated successfully.");
        setErrorNotify("");
      } else {
        setErrorNotify("Failed to update personal information.");
        setSuccessNotify("");
      }
    } catch (error) {
      setErrorNotify(`Error: ${error.message}`);
      setSuccessNotify("");
    }
  };

  if (isLoading) {
    return <p>Loading client data...</p>;
  }

  if (!clientData) {
    return <p>No client data found. Please try again.</p>;
  }

  return (
    <div className="profile-container">
      <div className="profile-card">
        <img
          src={
            "https://ramysalama-001-site1.qtempurl.com/" +
            clientData.personalImagePath
          }
          alt="Client Profile"
          className="profile-image"
        />
        <h2 className="client-name">{clientData.fullName || "Guest User"}</h2>
        <h4 className="client-balance">
          Balance: <span>{Number(clientData.balance || 0)} AED</span>
        </h4>
        <button className="update-info-button" onClick={toggleInfoPanel}>
          Update Personal Information
        </button>
        {isInfoPanelOpen && (
          <form className="info-form" onSubmit={handleUpdateInfo}>
            <input
              type="text"
              placeholder="First Name"
              value={personalInfo.firstName}
              onChange={(e) =>
                setPersonalInfo({ ...personalInfo, firstName: e.target.value })
              }
              required
            />
            <input
              type="text"
              placeholder="Middle Name"
              value={personalInfo.midName}
              onChange={(e) =>
                setPersonalInfo({ ...personalInfo, midName: e.target.value })
              }
            />
            <input
              type="text"
              placeholder="Last Name"
              value={personalInfo.lastName}
              onChange={(e) =>
                setPersonalInfo({ ...personalInfo, lastName: e.target.value })
              }
              required
            />
            <input
              type="text"
              placeholder="Phone Number"
              value={personalInfo.phoneNumber}
              onChange={(e) =>
                setPersonalInfo({
                  ...personalInfo,
                  phoneNumber: e.target.value,
                })
              }
              required
            />
            <button type="submit" className="submit-button">
              Update Information
            </button>
            {errorNotify && <p className="error-notify">{errorNotify}</p>}
            {successNotify && <p className="success-notify">{successNotify}</p>}
          </form>
        )}
        <button
          className="change-password-button"
          onClick={togglePasswordPanel}
        >
          Change Password
        </button>
        {isPasswordPanelOpen && (
          <form className="password-form" onSubmit={handleChangePassword}>
            <input
              type="password"
              placeholder="Current Password"
              value={currentPass}
              onChange={(e) => setCurrentPass(e.target.value)}
              required
            />
            <input
              type="password"
              placeholder="New Password"
              value={newPass}
              onChange={(e) => setNewPass(e.target.value)}
              required
            />
            <input
              type="password"
              placeholder="Confirm New Password"
              value={confirmPass}
              onChange={(e) => setConfirmPass(e.target.value)}
              required
            />
            <button type="submit" className="submit-button">
              Update Password
            </button>
            {errorNotify && <p className="error-notify">{errorNotify}</p>}
          </form>
        )}
        <button
          className="change-images-button"
          onClick={() => navigate("/UpdateImages")}
        >
          Change Images
        </button>
      </div>
    </div>
  );
}
