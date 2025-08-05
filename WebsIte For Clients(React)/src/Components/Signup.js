import { useState } from "react";
import { API_BASE_URL } from "../Constant";

export default function SignUp() {
  const [Firstname, setFirstName] = useState("");
  const [Midname, setMidName] = useState("");
  const [Lastname, setLastName] = useState("");
  const [Phone, setPhone] = useState("");
  const [Email, SetEmail] = useState("");
  const [Password, setPassword] = useState("");
  const [repeatPassword, setrepeatPassword] = useState("");
  const [notification, setNotification] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const OnSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setNotification("");

    // Client-side validation
    if (Password !== repeatPassword) {
      setNotification("Passwords do not match");
      setIsLoading(false);
      return;
    }

    if (Password.length < 6) {
      setNotification("Password must be at least 6 characters");
      setIsLoading(false);
      return;
    }

    if (!Email.endsWith("@gmail.com")) {
      setNotification("Email should end with @gmail.com");
      setIsLoading(false);
      return;
    }

    try {
      const response = await fetch(`${API_BASE_URL}Clients/PostNewClient`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          firstName: Firstname,
          midName: Midname,
          lastName: Lastname,
          email: Email,
          password: Password,
          phoneNumber: Phone,
        }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.errorMessage || "Something went wrong.");
      }

      const data = await response.json();
      localStorage.setItem("Id", Number(data.id));
      setNotification("Registration successful! Redirecting...");

      setTimeout(() => {
        window.location.pathname = "/UpdateImages";
      }, 1500);
    } catch (error) {
      setNotification(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
      <div className="sm:mx-auto sm:w-full sm:max-w-md">
        <div className="flex justify-center">
          <img
            src="/SalamaImage.webp"
            alt="Karm Al-Salama Logo"
            className="h-32 w-32 rounded-full object-cover"
          />
        </div>
        <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">
          Create your account 🔑
        </h2>
        <p className="mt-2 text-center text-sm text-gray-600">
          Join us today and start your journey
        </p>
      </div>

      <div className="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
        <div className="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
          {notification && (
            <div
              className={`rounded-md p-4 mb-6 ${
                notification.includes("successful") ||
                notification === "Done Successfully"
                  ? "bg-green-50 text-green-800"
                  : "bg-red-50 text-red-800"
              }`}
            >
              <p className="text-sm font-medium">{notification}</p>
            </div>
          )}

          <form className="space-y-6" onSubmit={OnSubmit}>
            <div className="grid grid-cols-1 gap-6 sm:grid-cols-2">
              <div>
                <label
                  htmlFor="FirstName"
                  className="block text-sm font-medium text-gray-700"
                >
                  First Name
                </label>
                <input
                  type="text"
                  id="FirstName"
                  value={Firstname}
                  onChange={(e) => setFirstName(e.target.value)}
                  required
                  className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                  placeholder="First Name"
                />
              </div>

              <div>
                <label
                  htmlFor="MidName"
                  className="block text-sm font-medium text-gray-700"
                >
                  Middle Name
                </label>
                <input
                  type="text"
                  id="MidName"
                  value={Midname}
                  onChange={(e) => setMidName(e.target.value)}
                  required
                  className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                  placeholder="Middle Name"
                />
              </div>
            </div>

            <div>
              <label
                htmlFor="LastName"
                className="block text-sm font-medium text-gray-700"
              >
                Last Name
              </label>
              <input
                type="text"
                id="LastName"
                value={Lastname}
                onChange={(e) => setLastName(e.target.value)}
                required
                className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                placeholder="Last Name"
              />
            </div>

            <div>
              <label
                htmlFor="Phone"
                className="block text-sm font-medium text-gray-700"
              >
                Phone Number
              </label>
              <input
                type="text"
                id="Phone"
                value={Phone}
                onChange={(e) => setPhone(e.target.value)}
                required
                className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                placeholder="Phone Number"
              />
            </div>

            <div>
              <label
                htmlFor="Email"
                className="block text-sm font-medium text-gray-700"
              >
                Email Address
              </label>
              <input
                type="email"
                id="Email"
                value={Email}
                onChange={(e) => SetEmail(e.target.value)}
                required
                className={`mt-1 block w-full border ${
                  Email && !Email.endsWith("@gmail.com")
                    ? "border-red-300"
                    : "border-gray-300"
                } rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm`}
                placeholder="example@gmail.com"
              />
              {Email && !Email.endsWith("@gmail.com") && (
                <p className="mt-1 text-sm text-red-600">
                  Email should end with @gmail.com
                </p>
              )}
            </div>

            <div>
              <label
                htmlFor="Password"
                className="block text-sm font-medium text-gray-700"
              >
                Password
              </label>
              <input
                type="password"
                id="Password"
                value={Password}
                onChange={(e) => setPassword(e.target.value)}
                required
                className={`mt-1 block w-full border ${
                  Password && Password.length < 6
                    ? "border-red-300"
                    : "border-gray-300"
                } rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm`}
                placeholder="Password"
              />
              {Password && Password.length < 6 && (
                <p className="mt-1 text-sm text-red-600">
                  Password must be at least 6 characters
                </p>
              )}
            </div>

            <div>
              <label
                htmlFor="RepeatPassword"
                className="block text-sm font-medium text-gray-700"
              >
                Repeat Password
              </label>
              <input
                type="password"
                id="RepeatPassword"
                value={repeatPassword}
                onChange={(e) => setrepeatPassword(e.target.value)}
                required
                className={`mt-1 block w-full border ${
                  repeatPassword && Password !== repeatPassword
                    ? "border-red-300"
                    : "border-gray-300"
                } rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm`}
                placeholder="Repeat Password"
              />
              {repeatPassword && Password !== repeatPassword && (
                <p className="mt-1 text-sm text-red-600">
                  Passwords do not match
                </p>
              )}
            </div>

            <div>
              <button
                type="submit"
                disabled={isLoading}
                className={`w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 ${
                  isLoading ? "opacity-70 cursor-not-allowed" : ""
                }`}
              >
                {isLoading ? (
                  <>
                    <svg
                      className="animate-spin -ml-1 mr-3 h-5 w-5 text-white"
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 24 24"
                    >
                      <circle
                        className="opacity-25"
                        cx="12"
                        cy="12"
                        r="10"
                        stroke="currentColor"
                        strokeWidth="4"
                      ></circle>
                      <path
                        className="opacity-75"
                        fill="currentColor"
                        d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                      ></path>
                    </svg>
                    Creating account...
                  </>
                ) : (
                  "Sign Up"
                )}
              </button>
            </div>
          </form>

          <div className="mt-6 text-center">
            <p className="text-sm text-gray-600">
              Already have an account?{" "}
              <a
                href="/login"
                className="font-medium text-indigo-600 hover:text-indigo-500"
              >
                Log in
              </a>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
