import React, { useEffect, useState } from "react";
import "../Styles/Visa.css";

const _HttpClient = "https://ramysalama-001-site1.qtempurl.com/api/";
export default function Visa({ VisaObj }) {
  const [clientData, setClientData] = useState(null);
  const [errorNotify, setErrorNotify] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const token = localStorage.getItem("Token");
  const clientId = Number(localStorage.getItem("Id"));
  useEffect(() => {
    const fetchClientData = async () => {
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
          throw new Error(
            `Failed to fetch client data: ${response.statusText}`
          );
        }

        const data = await response.json();
        setClientData(data);
      } catch (error) {
        setErrorNotify(`Error fetching client data: ${error.message}`);
      } finally {
        setIsLoading(false);
      }
    };

    fetchClientData();
  }, []);

  async function handleIsuueClick(Price, id) {
    const token = localStorage.getItem("Token");
    if (token == null) {
      setErrorNotify("Not Authorized You Must Login First");
      window.location.pathname = "/Login";
      return;
    }
    //this if statment handle PayWith Internal Balance
    if (Number(clientData.balance) >= Price) {
      const response = await fetch(
        `${_HttpClient}InternalBalance/PayWithInternalBalance`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            Token: token,
            ClientID: clientId,
            Amount: Price,
            VisaId: id,
            OrderTypeId: 1,
          }),
        }
      );
      //this if statment handle add the order if the Client have enough internal balance
      if (response.ok) {
        alert("Order Has Been Added Successfully");
      } else {
        setErrorNotify("Error in payment:", response.statusText);
      }
    } else {
      window.location.pathname = "/PayWithVisa";
    }
  }

  async function handleRenewClick(Price, id) {
    const token = localStorage.getItem("Token");
    if (token == null) {
      setErrorNotify("Not Authorized You Must Login First");
      window.location.pathname = "/Login";
      return;
    }
    if (Number(clientData.balance) >= Price) {
      const response = await fetch(
        `${_HttpClient}InternalBalance/PayWithInternalBalance`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            Token: token,
            ClientID: clientId,
            Amount: Price,
            VisaId: id,
            OrderTypeId: 2,
          }),
        }
      );
      if (response.ok) {
        alert("Order Has Been Added Successfully");
      } else {
        setErrorNotify("Error in payment:", response.statusText);
      }
    } else {
      window.location.pathname = "/PayWithVisa";
    }
  }

  if (isLoading) {
    return <div>Loading...</div>;
  }

  return (
    <>
      {errorNotify && (
        <div className="error-notify" style={{ color: "black" }}>
          {errorNotify}
        </div>
      )}

      <div className="visa-container">
        <div className="visa-image">
          <img
            src={
              "https://ramysalama-001-site1.qtempurl.com/" + VisaObj.imagePath
            }
            alt={VisaObj.name}
          />
        </div>
        <div className="visa-details">
          <h2 className="visa-name"> {VisaObj.name}</h2>
          <div className="visa-info">
            <div className="visa-price">
              <span className="label">Issuance Price:</span>
              <span className="amount">
                {" "}
                {VisaObj.issuancePrice}{" "}
                <span style={{ color: "white" }}>AED</span>
              </span>
            </div>
            {VisaObj.renewalPrice !== 0 && (
              <div className="visa-price">
                <span className="label">Renewal Price:</span>
                <span className="amount">
                  {" "}
                  {VisaObj.renewalPrice}{" "}
                  <span style={{ color: "white" }}>AED</span>
                </span>
              </div>
            )}
            <div className="visa-duration">
              <span className="label">Duration: </span>
              <span className="duration">
                {" "}
                {VisaObj.period} <span style={{ color: "white" }}>Month/s</span>{" "}
              </span>
            </div>
          </div>

          <div className="visa-extra-info">
            <h3 className="extra-info-title">About:-</h3>
            <p className="extra-info-description">{VisaObj.moreDetails}</p>
          </div>

          <div className="visa-buttons">
            <button
              className="visa-button issue-button"
              onClick={() =>
                handleIsuueClick(
                  Number(VisaObj.issuancePrice),
                  Number(VisaObj.visaId)
                )
              }
            >
              Issue
            </button>

            {VisaObj.renewalPrice !== 0 && (
              <button
                className="visa-button renew-button"
                onClick={() =>
                  handleRenewClick(
                    Number(VisaObj.renewalPrice),
                    Number(VisaObj.visaId)
                  )
                }
              >
                Renew
              </button>
            )}
          </div>
        </div>
      </div>
    </>
  );
}
