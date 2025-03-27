import { useEffect, useState } from "react";
import OrderCom from "./OrderCom";
import "../Styles/Orders.css";

const httpClient = "https://ramysalama-001-site1.qtempurl.com/api/";

export default function Orders() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchOrders = async () => {
      const token = localStorage.getItem("Token");
      const clientId = Number(localStorage.getItem("Id"));

      if (!token || !clientId || isNaN(clientId)) {
        setError("Missing authentication details. Please login again.");
        setLoading(false);
        return;
      }

      try {
        const response = await fetch(
          `${httpClient}Orders/FindOrdersBelongsToSpecificClient`,
          {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify({
              token,
              clientId, // لا داعي لاستخدام Number هنا لأنه تم تحويله مسبقًا
            }),
          }
        );

        if (response.ok) {
          const data = await response.json();
          setOrders(data);
        } else {
          setError(`No Orders`);
        }
      } catch (err) {
        setError("An error occurred while fetching orders.");
      } finally {
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  if (loading) {
    return <p>Loading orders...</p>;
  }

  if (error) {
    return <p className="error-message">{error}</p>;
  }

  if (orders.length === 0) {
    return <p>No orders found.</p>;
  }

  return (
    <div className="Order-list">
      {orders
        .sort((a, b) => b.orderID - a.orderID) // ترتيب تنازلي حسب orderID
        .map((order) => (
          <OrderCom key={order.orderID} orderObj={order} />
        ))}
    </div>
  );
}
