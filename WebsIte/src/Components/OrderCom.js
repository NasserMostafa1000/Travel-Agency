import React from "react";
import "../Styles/OrderCom.css";

export default function OrderCom({ orderObj }) {
  if (!orderObj) {
    return <h2>Loading...</h2>;
  }
  return (
    <div className="order-card">
      <div className="order-header">
        <h2>Order #{orderObj.orderID}</h2>
        <p className={`order-status ${getStatusClass(orderObj.order_Status)}`}>
          {getStatusText(orderObj.order_Status)}
        </p>
      </div>
      <div className="order-details">
        <p>
          <strong>Item:</strong> {orderObj.visa_Name}
        </p>
        <p>
          <strong>Type:</strong>{" "}
          {orderObj.orderType === "تجديد" ? "renew" : "issue"}
        </p>
        <p>
          <strong>Rejection Reason:</strong>{" "}
          {orderObj.rejectionReason || "No issues detected"}
        </p>
        <p>
          <strong>Date:</strong> {orderObj.date}
        </p>
      </div>
    </div>
  );
}

function getStatusClass(status) {
  switch (status) {
    case "تحت المعالجة":
      return "processing";
    case "مكتمل":
      return "complete";
    case "نواقص":
      return "bad-order";
    default:
      return "Rejected";
  }
}

function getStatusText(status) {
  switch (status) {
    case "تحت المعالجة":
      return "Processing";
    case "مكتمل":
      return "Complete";
    case "نواقص":
      return "Bad Order";
    default:
      return "Rejected";
  }
}
