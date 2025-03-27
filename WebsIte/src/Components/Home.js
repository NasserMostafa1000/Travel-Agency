import { useState, useEffect } from "react";
import Visa from "./Visa";
import "../Styles/Home.css";
import AboutUs from "../Components/AboutUs";
var _HttpClient = "https://ramysalama-001-site1.qtempurl.com/api/";

export default function Home() {
  const [visas, setVisas] = useState([]);
  useEffect(() => {
    const fetchVisas = async () => {
      try {
        const response = await fetch(`${_HttpClient}Visas/GetAllVisas`);

        if (!response.ok) {
          throw new Error(`Failed to fetch visas: ${response.statusText}`);
        }

        const data = await response.json();
        setVisas(data);
      } catch (error) {
        console.error("Error fetching visas:", error.message);
      }
    };

    fetchVisas();
  }, []);

  return (
    <div>
      <div>
        <div>
          {visas.map(
            (visa) =>
              visa.moreDetails !== "غير متوفر" && (
                <Visa key={visa.visaId} VisaObj={visa} />
              )
          )}
        </div>

        {/* العنصر الذي سيتم مراقبته عندما يصل المستخدم إلى أسفل الصفحة */}
        <div id="bottom" style={{ height: "1px" }}></div>
      </div>
      <div className="about-us-container">
        <AboutUs />
      </div>
    </div>
  );
}
