import { useState } from "react";
import { API_BASE_URL } from "../Constant";

export default function UpdatePersonalAndPassportImages() {
  const [personalImage, setPersonalImage] = useState(null);
  const [passportImage, setPassportImage] = useState(null);
  const [personalImageStatus, setPersonalImageStatus] = useState("");
  const [passportImageStatus, setPassportImageStatus] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handlePersonalImageChange = (e) => {
    setPersonalImage(e.target.files[0]);
    setPersonalImageStatus("");
  };

  const handlePassportImageChange = (e) => {
    setPassportImage(e.target.files[0]);
    setPassportImageStatus("");
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    let uploadSuccess = true;

    const clientId = window.localStorage.getItem("Id");

    if (personalImage) {
      const formData = new FormData();
      formData.append("imageFile", personalImage);

      try {
        const response = await fetch(
          `${API_BASE_URL}Clients/UploadPersonalImage?ClientId=${clientId}`,
          {
            method: "POST",
            body: formData,
          }
        );

        if (response.ok) {
          setPersonalImageStatus("Personal image uploaded successfully");
        } else {
          uploadSuccess = false;
          setPersonalImageStatus(
            `Failed to upload personal image: ${response.status}`
          );
        }
      } catch (error) {
        uploadSuccess = false;
        setPersonalImageStatus(`Error: ${error.message}`);
      }
    }

    if (passportImage) {
      const formData = new FormData();
      formData.append("imageFile", passportImage);

      try {
        const response = await fetch(
          `${API_BASE_URL}Clients/UploadPassportImage?ClientId=${clientId}`,
          {
            method: "POST",
            body: formData,
          }
        );

        if (response.ok) {
          setPassportImageStatus("Passport image uploaded successfully");
        } else {
          uploadSuccess = false;
          setPassportImageStatus(
            `Failed to upload passport image: ${response.status}`
          );
        }
      } catch (error) {
        uploadSuccess = false;
        setPassportImageStatus(`Error: ${error.message}`);
      }
    }

    setIsSubmitting(false);
    if (uploadSuccess) {
      alert("تم رفع الصور بنجاح");
    }
  };

  return (
    <div className="max-w-md mx-auto p-6 bg-white rounded-lg shadow-md">
      <h1 className="text-2xl font-bold text-center text-gray-800 mb-6">
        Set Your Images
      </h1>

      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Personal Image Upload */}
        <div className="space-y-3">
          <label className="block text-sm font-medium text-gray-700">
            Personal Image
          </label>
          <label
            htmlFor="personalImage"
            className="flex flex-col items-center justify-center w-full px-4 py-6 border-2 border-dashed border-gray-300 rounded-lg cursor-pointer hover:border-indigo-500 hover:bg-gray-50 transition-colors"
          >
            <svg
              className="w-12 h-12 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"
              ></path>
            </svg>
            <span className="mt-2 text-sm text-gray-600">
              {personalImage
                ? personalImage.name
                : "Click to select personal image"}
            </span>
            <input
              type="file"
              id="personalImage"
              accept="image/*"
              onChange={handlePersonalImageChange}
              className="hidden"
            />
          </label>
          {personalImageStatus && (
            <p
              className={`text-sm ${
                personalImageStatus.includes("successfully")
                  ? "text-green-600"
                  : "text-red-600"
              }`}
            >
              {personalImageStatus}
            </p>
          )}
        </div>

        {/* Passport Image Upload */}
        <div className="space-y-3">
          <label className="block text-sm font-medium text-gray-700">
            Passport Image
          </label>
          <label
            htmlFor="passportImage"
            className="flex flex-col items-center justify-center w-full px-4 py-6 border-2 border-dashed border-gray-300 rounded-lg cursor-pointer hover:border-indigo-500 hover:bg-gray-50 transition-colors"
          >
            <svg
              className="w-12 h-12 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"
              ></path>
            </svg>
            <span className="mt-2 text-sm text-gray-600">
              {passportImage
                ? passportImage.name
                : "Click to select passport image"}
            </span>
            <input
              type="file"
              id="passportImage"
              accept="image/*"
              onChange={handlePassportImageChange}
              className="hidden"
            />
          </label>
          {passportImageStatus && (
            <p
              className={`text-sm ${
                passportImageStatus.includes("successfully")
                  ? "text-green-600"
                  : "text-red-600"
              }`}
            >
              {passportImageStatus}
            </p>
          )}
        </div>

        {/* Instructions */}
        <div className="bg-yellow-50 border-l-4 border-yellow-400 p-4">
          <div className="flex">
            <div className="flex-shrink-0">
              <svg
                className="h-5 w-5 text-yellow-400"
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 20 20"
                fill="currentColor"
              >
                <path
                  fillRule="evenodd"
                  d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z"
                  clipRule="evenodd"
                />
              </svg>
            </div>
            <div className="ml-3">
              <h3 className="text-sm font-medium text-yellow-800">
                Upload Requirements
              </h3>
              <div className="mt-2 text-sm text-yellow-700">
                <ul className="list-disc pl-5 space-y-1">
                  <li>Only image files are accepted</li>
                  <li>The background must be completely white</li>
                  <li>The face must be clear and centered</li>
                </ul>
              </div>
            </div>
          </div>
        </div>

        {/* Submit Button */}
        <button
          type="submit"
          disabled={isSubmitting || (!personalImage && !passportImage)}
          className={`w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white ${
            isSubmitting ? "bg-indigo-400" : "bg-indigo-600 hover:bg-indigo-700"
          } focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 ${
            !personalImage && !passportImage
              ? "opacity-50 cursor-not-allowed"
              : ""
          }`}
        >
          {isSubmitting ? (
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
              Uploading...
            </>
          ) : (
            "Upload Images"
          )}
        </button>
      </form>
    </div>
  );
}
