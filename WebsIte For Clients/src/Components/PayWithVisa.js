import "../Styles/PayWithVisa.css";
import React, { useState } from "react";

export default function PayWithVisa() {
  const [cardNumber, setCardNumber] = useState("");
  const [expiryDate, setExpiryDate] = useState("");
  const [cvc, setCvc] = useState("");
  const [name, setName] = useState("");
  const [isSuccess, setIsSuccess] = useState(null);

  const handleSubmit = (e) => {
    e.preventDefault();
    // Here you can add the actual payment logic
    if (cardNumber && expiryDate && cvc && name) {
      setIsSuccess(true);
    } else {
      setIsSuccess(false);
    }
  };

  const handleExpiryDateChange = (e) => {
    let value = e.target.value;

    // Remove non-numeric characters
    value = value.replace(/\D/g, "");

    // Add "/" after the month if there are two digits
    if (value.length > 2) {
      value = value.slice(0, 2) + "/" + value.slice(2, 4);
    }

    setExpiryDate(value); // Update state
  };

  return (
    <div className="payment-container">
      <div className="payment-card">
        <h2>It will be available in the near future</h2>
        <img src="VisaLogo.png" alt="VisaLogo" />
        <form onSubmit={handleSubmit}>
          <div className="input-group">
            <label>Cardholder Name</label>
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="Enter cardholder name"
              required
            />
          </div>

          <div className="input-group">
            <label>Card Number</label>
            <input
              type="text"
              value={cardNumber}
              onChange={(e) => setCardNumber(e.target.value)}
              placeholder="Enter card number"
              maxLength="16"
              required
            />
          </div>

          <div className="input-group">
            <label>Expiry Date</label>
            <input
              type="text"
              value={expiryDate}
              onChange={handleExpiryDateChange} // Use the modified function
              placeholder="MM/YY"
              maxLength="5"
              required
            />
          </div>

          <div className="input-group">
            <label>CVC</label>
            <input
              type="text"
              value={cvc}
              onChange={(e) => setCvc(e.target.value)}
              placeholder="CVC"
              maxLength="3"
              required
            />
          </div>

          <button type="submit" className="pay-button">
            Pay
          </button>
        </form>

        {isSuccess !== null && (
          <div className={`status ${isSuccess ? "success" : "error"}`}>
            {isSuccess
              ? "Payment Successful!"
              : "Please fill in all fields correctly"}
          </div>
        )}
      </div>
    </div>
  );
}
