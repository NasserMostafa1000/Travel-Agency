import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { API_BASE_URL, ServerPath } from "../Constant";

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

  const togglePasswordPanel = () => {
    setIsPasswordPanelOpen((prev) => !prev);
    setErrorNotify("");
    setSuccessNotify("");
  };

  const toggleInfoPanel = () => {
    setIsInfoPanelOpen((prev) => !prev);
    setErrorNotify("");
    setSuccessNotify("");
  };

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
        const response = await fetch(`${API_BASE_URL}Clients/FindClient`, {
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

    setIsLoading(true);
    try {
      const verifyResponse = await fetch(
        `${API_BASE_URL}Users/VerifyPassword`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            email: clientData?.email,
            password: currentPass,
          }),
        }
      );

      if (!verifyResponse.ok) {
        setErrorNotify("The current password is not valid.");
        return;
      }

      const changeResponse = await fetch(`${API_BASE_URL}Users/PutInfo`, {
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
    } finally {
      setIsLoading(false);
    }
  };

  const handleUpdateInfo = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    try {
      const response = await fetch(`${API_BASE_URL}Clients/PutClient`, {
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
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-blue-500"></div>
      </div>
    );
  }

  if (!clientData) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="bg-white p-6 rounded-lg shadow-md text-center max-w-md">
          <h2 className="text-xl font-semibold text-gray-800 mb-4">
            No client data found
          </h2>
          <p className="text-gray-600 mb-4">
            Please try again or contact support.
          </p>
          <button
            onClick={() => window.location.reload()}
            className="bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded-lg transition"
          >
            Try Again
          </button>
        </div>
      </div>
    );
  }
  const imagePath = ServerPath + clientData.personalImagePath;
  return (
    <div className="min-h-screen bg-gray-50 py-8 px-4 sm:px-6 lg:px-8">
      <div className="max-w-3xl mx-auto">
        {/* Profile Card */}
        <div className="bg-white rounded-xl shadow-md overflow-hidden">
          {/* Profile Header */}
          <div className="bg-gradient-to-r from-blue-600 to-indigo-700 p-6 text-center">
            <div className="relative mx-auto w-32 h-32 rounded-full border-4 border-white shadow-lg">
              <img
                src={imagePath}
                alt="Client Profile"
                className="w-full h-full rounded-full object-cover"
                onError={(e) => {
                  e.target.onerror = null;
                  e.target.src = "https://via.placeholder.com/150";
                }}
              />
            </div>
            <h2 className="text-2xl font-bold text-white mt-4">
              {clientData.fullName || "Guest User"}
            </h2>
            <div className="mt-2 bg-white bg-opacity-20 inline-block px-4 py-1 rounded-full">
              <span className="text-white font-medium">
                Balance: {Number(clientData.balance || 0)} AED
              </span>
            </div>
          </div>

          {/* Profile Content */}
          <div className="p-6 space-y-6">
            {/* Notifications */}
            {errorNotify && (
              <div className="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 rounded">
                <p>{errorNotify}</p>
              </div>
            )}
            {successNotify && (
              <div className="bg-green-100 border-l-4 border-green-500 text-green-700 p-4 rounded">
                <p>{successNotify}</p>
              </div>
            )}

            {/* Action Buttons */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <button
                onClick={toggleInfoPanel}
                className={`py-3 px-4 rounded-lg font-medium transition ${
                  isInfoPanelOpen
                    ? "bg-indigo-600 text-white"
                    : "bg-gray-100 hover:bg-gray-200 text-gray-800"
                }`}
              >
                Update Personal Information
              </button>
              <button
                onClick={togglePasswordPanel}
                className={`py-3 px-4 rounded-lg font-medium transition ${
                  isPasswordPanelOpen
                    ? "bg-indigo-600 text-white"
                    : "bg-gray-100 hover:bg-gray-200 text-gray-800"
                }`}
              >
                Change Password
              </button>
              <button
                onClick={() => navigate("/UpdateImages")}
                className="py-3 px-4 bg-gray-100 hover:bg-gray-200 text-gray-800 rounded-lg font-medium transition md:col-span-2"
              >
                Change Profile Images
              </button>
            </div>

            {/* Personal Info Form */}
            {isInfoPanelOpen && (
              <form onSubmit={handleUpdateInfo} className="space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      First Name
                    </label>
                    <input
                      type="text"
                      value={personalInfo.firstName}
                      onChange={(e) =>
                        setPersonalInfo({
                          ...personalInfo,
                          firstName: e.target.value,
                        })
                      }
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                      required
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Middle Name
                    </label>
                    <input
                      type="text"
                      value={personalInfo.midName}
                      onChange={(e) =>
                        setPersonalInfo({
                          ...personalInfo,
                          midName: e.target.value,
                        })
                      }
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Last Name
                    </label>
                    <input
                      type="text"
                      value={personalInfo.lastName}
                      onChange={(e) =>
                        setPersonalInfo({
                          ...personalInfo,
                          lastName: e.target.value,
                        })
                      }
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                      required
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Phone Number
                    </label>
                    <input
                      type="text"
                      value={personalInfo.phoneNumber}
                      onChange={(e) =>
                        setPersonalInfo({
                          ...personalInfo,
                          phoneNumber: e.target.value,
                        })
                      }
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                      required
                    />
                  </div>
                </div>
                <button
                  type="submit"
                  disabled={isLoading}
                  className="w-full py-2 px-4 bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-lg transition disabled:opacity-50"
                >
                  {isLoading ? "Updating..." : "Update Information"}
                </button>
              </form>
            )}

            {/* Change Password Form */}
            {isPasswordPanelOpen && (
              <form onSubmit={handleChangePassword} className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Current Password
                  </label>
                  <input
                    type="password"
                    value={currentPass}
                    onChange={(e) => setCurrentPass(e.target.value)}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    New Password
                  </label>
                  <input
                    type="password"
                    value={newPass}
                    onChange={(e) => setNewPass(e.target.value)}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Confirm New Password
                  </label>
                  <input
                    type="password"
                    value={confirmPass}
                    onChange={(e) => setConfirmPass(e.target.value)}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    required
                  />
                </div>
                <button
                  type="submit"
                  disabled={isLoading}
                  className="w-full py-2 px-4 bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-lg transition disabled:opacity-50"
                >
                  {isLoading ? "Updating..." : "Update Password"}
                </button>
              </form>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
