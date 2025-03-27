import { useState } from "react";
import "../Styles/UpdateImages.css";

const _HttpClient = "https://ramysalama-001-site1.qtempurl.com/api/";

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
          `${_HttpClient}Clients/UploadPersonalImage?ClientId=${clientId}`,
          {
            method: "POST",
            body: formData,
          }
        );

        if (response.ok) {
          setPersonalImageStatus("Personal image uploaded successfully ✅");
        } else {
          uploadSuccess = false;
          setPersonalImageStatus(
            `Failed to upload personal image ❌: ${response.status}`
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
          `${_HttpClient}Clients/UploadPassportImage?ClientId=${clientId}`,
          {
            method: "POST",
            body: formData,
          }
        );

        if (response.ok) {
          setPassportImageStatus("Passport image uploaded successfully ✅");
        } else {
          uploadSuccess = false;
          setPassportImageStatus(
            `Failed to upload passport image ❌: ${response.status}`
          );
        }
      } catch (error) {
        uploadSuccess = false;
        setPassportImageStatus(`Error: ${error.message}`);
      }
    }

    setIsSubmitting(false);
    if (uploadSuccess) {
      alert("تم بنجاح");
    }
  };

  return (
    <div className="update-images-container">
      <h1>Set Your Images</h1>
      <form onSubmit={handleSubmit}>
        <div className="image-upload">
          <div className="image-upload-section">
            <label htmlFor="personalImage" className="upload-button">
              Choose Personal Image
            </label>
            <input
              type="file"
              id="personalImage"
              accept="image/*"
              onChange={handlePersonalImageChange}
              style={{ display: "none" }}
            />
            {personalImage && (
              <p style={{ color: "green" }}>Selected: {personalImage.name}</p>
            )}
            {personalImageStatus && (
              <p
                style={{
                  color: personalImageStatus.includes("✅") ? "green" : "red",
                }}
              >
                {personalImageStatus}
              </p>
            )}
            {/* Instructions for personal image */}
            <ul className="image-instructions"></ul>
          </div>

          <div className="image-upload-section">
            <label htmlFor="passportImage" className="upload-button">
              Choose Passport Image
            </label>
            <input
              type="file"
              id="passportImage"
              accept="image/*"
              onChange={handlePassportImageChange}
              style={{ display: "none" }}
            />
            {passportImage && (
              <p style={{ color: "green" }}>Selected: {passportImage.name}</p>
            )}
            {passportImageStatus && (
              <p
                style={{
                  color: passportImageStatus.includes("✅") ? "green" : "red",
                }}
              >
                {passportImageStatus}
              </p>
            )}
          </div>
          <ul className="image-instructions">
            <li>Accept only images</li>
            <li>The background must be completely white.</li>
            <li>The face must be clear and centered.</li>
          </ul>
        </div>

        <button type="submit" className="submit-button" disabled={isSubmitting}>
          {isSubmitting ? "Uploading..." : "Upload Images"}
        </button>
      </form>
    </div>
  );
}
