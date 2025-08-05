import { useEffect, useState } from "react";
import { API_BASE_URL, ServerPath } from "../Constant";
import {
  FaPassport,
  FaSyncAlt,
  FaRegClock,
  FaMoneyBillWave,
  FaCalendarAlt,
} from "react-icons/fa";
import {
  MdOutlineTravelExplore,
  MdOutlinePayment,
  MdErrorOutline,
} from "react-icons/md";
import { GiPriceTag } from "react-icons/gi";

export default function Visa({ VisaObj }) {
  const [clientData, setClientData] = useState(null);
  const [errorNotify, setErrorNotify] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [isHovered, setIsHovered] = useState(false);
  const token = localStorage.getItem("Token");
  const clientId = Number(localStorage.getItem("Id"));

  useEffect(() => {
    const fetchClientData = async () => {
      if (!token || !clientId) {
        setErrorNotify("You must be logged in first.");
        return;
      }

      setIsLoading(true);
      try {
        const response = await fetch(`${API_BASE_URL}Clients/FindClient`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ clientId, token }),
        });

        if (!response.ok) throw new Error("Failed to load client data");

        const data = await response.json();
        setClientData(data);
      } catch (error) {
        setErrorNotify(error.message);
      } finally {
        setIsLoading(false);
      }
    };

    fetchClientData();
  }, [token, clientId]);

  const VisaImg = ServerPath + VisaObj.imagePath;

  const handlePayment = async (typeId, price, visaId) => {
    if (!token) {
      window.location.pathname = "/Login";
      return;
    }

    if (Number(clientData?.balance) >= price) {
      try {
        const response = await fetch(
          `${API_BASE_URL}InternalBalance/PayWithInternalBalance`,
          {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
              Token: token,
              ClientID: clientId,
              Amount: price,
              VisaId: visaId,
              OrderTypeId: typeId,
            }),
          }
        );

        if (response.ok) {
          alert("Payment completed successfully");
        } else {
          const errorData = await response.json();
          setErrorNotify(errorData.message || "An error occurred");
        }
      } catch (error) {
        setErrorNotify("Network error: " + error.message);
      }
    } else {
      window.location.pathname = "/PayWithVisa";
    }
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-t-4 border-b-4 border-indigo-600"></div>
      </div>
    );
  }

  return (
    <div
      className="bg-white rounded-2xl shadow-lg hover:shadow-xl transition-all duration-300 overflow-hidden border border-gray-100 h-full flex flex-col group"
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
    >
      {/* Image with Badge */}
      <div className="relative h-48 overflow-hidden">
        <img
          src={VisaImg}
          alt={VisaObj.name}
          className={`w-full h-full object-cover transition-transform duration-500 ${
            isHovered ? "scale-105" : ""
          }`}
          onError={(e) => {
            e.target.onerror = null;
            e.target.src =
              "https://images.unsplash.com/photo-1501594907352-04cda38ebc29?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80";
          }}
        />
        <div className="absolute inset-0 bg-gradient-to-t from-black/40 to-transparent"></div>

        {/* Visa Type Badge */}
        <div className="absolute top-4 right-4 bg-indigo-600 text-white px-3 py-1 rounded-full text-xs font-medium shadow-md">
          {VisaObj.visaType || "Visa"}
        </div>
      </div>

      {/* Details */}
      <div className="p-6 flex-grow flex flex-col">
        <div className="flex-grow">
          {/* Title */}
          <div className="flex items-center gap-3 mb-4">
            <div className="bg-indigo-100 p-2 rounded-full">
              <MdOutlineTravelExplore className="text-xl text-indigo-600" />
            </div>
            <h2 className="text-xl font-bold text-gray-800 font-['Poppins']">
              {VisaObj.name}
            </h2>
          </div>

          {/* Pricing Grid */}
          <div className="grid grid-cols-2 gap-3 mb-5">
            <div className="bg-indigo-50 p-3 rounded-xl border border-indigo-100">
              <div className="flex items-center gap-2 mb-1">
                <GiPriceTag className="text-indigo-500" />
                <p className="text-xs text-gray-500">Issuance</p>
              </div>
              <p className="font-bold text-indigo-700">
                {VisaObj.issuancePrice}{" "}
                <span className="text-gray-500">AED</span>
              </p>
            </div>

            {VisaObj.renewalPrice !== 0 && (
              <div className="bg-green-50 p-3 rounded-xl border border-green-100">
                <div className="flex items-center gap-2 mb-1">
                  <FaSyncAlt className="text-green-500 text-sm" />
                  <p className="text-xs text-gray-500">Renewal</p>
                </div>
                <p className="font-bold text-green-700">
                  {VisaObj.renewalPrice}{" "}
                  <span className="text-gray-500">AED</span>
                </p>
              </div>
            )}

            <div className="bg-gray-50 p-3 rounded-xl border border-gray-100 col-span-2">
              <div className="flex items-center gap-2 mb-1">
                <FaRegClock className="text-gray-500" />
                <p className="text-xs text-gray-500">Duration</p>
              </div>
              <p className="font-medium">
                {VisaObj.period} <span className="text-gray-500">months</span>
              </p>
            </div>
          </div>

          {/* Description */}
          <div className="bg-blue-50/30 p-3 rounded-lg mb-4 border border-blue-100">
            <p className="text-gray-600 text-sm line-clamp-3">
              {VisaObj.moreDetails}
            </p>
          </div>
        </div>

        {/* Buttons */}
        <div className="mt-auto grid grid-cols-2 gap-3">
          <button
            onClick={() =>
              handlePayment(
                1,
                Number(VisaObj.issuancePrice),
                Number(VisaObj.visaId)
              )
            }
            className="flex items-center justify-center gap-2 bg-gradient-to-r from-indigo-600 to-blue-600 hover:from-indigo-700 hover:to-blue-700 text-white py-3 px-4 rounded-xl font-medium transition-all duration-200 text-sm shadow-md hover:shadow-lg"
          >
            <MdOutlinePayment className="text-base" />
            <span>Issue Now</span>
          </button>

          {VisaObj.renewalPrice !== 0 && (
            <button
              onClick={() =>
                handlePayment(
                  2,
                  Number(VisaObj.renewalPrice),
                  Number(VisaObj.visaId)
                )
              }
              className="flex items-center justify-center gap-2 bg-gradient-to-r from-green-600 to-emerald-600 hover:from-green-700 hover:to-emerald-700 text-white py-3 px-4 rounded-xl font-medium transition-all duration-200 text-sm shadow-md hover:shadow-lg"
            >
              <FaSyncAlt className="text-base" />
              <span>Renew</span>
            </button>
          )}
        </div>
      </div>

      {/* Error Message */}
      {errorNotify && (
        <div className="bg-red-50 border-l-4 border-red-500 text-red-700 p-3 mx-5 mb-4 rounded-lg flex items-start gap-2">
          <MdErrorOutline className="text-lg mt-0.5 flex-shrink-0" />
          <span className="text-sm">{errorNotify}</span>
        </div>
      )}
    </div>
  );
}
