import { useState, useEffect } from "react";
import Visa from "./Visa";
import AboutUs from "../Components/AboutUs";
import { API_BASE_URL, ServerPath, SiteName } from "../Constant";
import { useNavigate } from "react-router-dom";
import * as signalR from "@microsoft/signalr";

import {
  FiClock,
  FiShield,
  FiCreditCard,
  FiSmile,
  FiRefreshCw,
} from "react-icons/fi";

export default function Home() {
  const [visas, setVisas] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();
  const fetchVisas = async () => {
    try {
      setIsLoading(true);
      const response = await fetch(`${API_BASE_URL}Visas/GetAllVisas`);
      if (!response.ok)
        throw new Error(`Failed to fetch visas: ${response.statusText}`);
      const data = await response.json();
      setVisas(data);
    } catch (error) {
      console.error("Error fetching visas:", error.message);
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    // 1. نجيب الفيزات أول مرة
    fetchVisas();

    // 2. نبدأ اتصال SignalR
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${ServerPath}/VisaHub`) // رابط الهب
      .withAutomaticReconnect()
      .build();

    connection
      .start()
      .then(() => {
        console.log("Connected to VisaHub");
      })
      .catch((err) => {
        console.error("Connection failed: ", err);
      });

    // 3. استقبال رسالة التحديث
    connection.on(
      "ReceiveVisaPriceUpdate",
      (name, issuancePrice, renewalPrice, visaId) => {
        console.log("Visa price updated. Refetching...");
        fetchVisas(); // إعادة جلب البيانات
      }
    );

    // 4. تنظيف الاتصال لما الكمبوننت يتشال
    return () => {
      connection.stop();
    };
  }, []);
  if (isLoading) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gradient-to-br from-blue-50 to-indigo-50">
        <div className="text-center">
          <div className="animate-spin rounded-full h-16 w-16 border-t-4 border-b-4 border-indigo-600 mx-auto mb-4"></div>
          <p className="text-indigo-800 font-medium">Loading visas...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex justify-center items-center min-h-screen px-4 bg-gradient-to-br from-blue-50 to-indigo-50">
        <div className="bg-white border border-red-200 text-red-700 px-6 py-6 rounded-2xl max-w-md w-full text-center shadow-lg">
          <h3 className="font-bold text-xl mb-3 text-indigo-900">
            An error occurred!
          </h3>
          <p className="mb-5 text-gray-600">{error}</p>
          <button
            onClick={() => window.location.reload()}
            className="bg-gradient-to-r from-indigo-600 to-purple-600 hover:from-indigo-700 hover:to-purple-700 text-white py-2 px-8 rounded-full font-medium shadow-md transition-all duration-300 hover:shadow-lg"
          >
            <FiRefreshCw className="inline mr-2" />
            Retry
          </button>
        </div>
      </div>
    );
  }

  const availableVisas = visas.filter(
    (visa) => visa.moreDetails !== "غير متوفر"
  );

  return (
    <div className="bg-gradient-to-br from-blue-50 to-indigo-50 min-h-screen font-['Inter']">
      {/* Hero Section */}
      <section className="relative bg-gradient-to-r from-indigo-600 to-purple-600 text-white py-28 px-4 overflow-hidden">
        <div className="absolute inset-0 opacity-20">
          <div className="absolute inset-0 bg-[url('https://img.icons8.com/ios-filled/50/FFFFFF/visa.png')] bg-repeat bg-center bg-[length:100px_100px]"></div>
        </div>
        <div className="container mx-auto text-center relative z-10 max-w-4xl">
          <h1 className="text-4xl sm:text-5xl md:text-6xl font-bold mb-6 leading-tight font-['Poppins']">
            Explore the World <br />
            With Ease
          </h1>
          <p className="text-xl sm:text-2xl mb-10 max-w-2xl mx-auto opacity-90">
            Fast, secure and reliable visa services for your next adventure
          </p>
          <div className="flex flex-col sm:flex-row justify-center gap-4">
            <button
              onClick={() => navigate("/login")}
              className="bg-white text-indigo-600 hover:bg-gray-100 font-semibold py-4 px-10 rounded-full shadow-xl transition-all duration-300 hover:scale-105 text-lg"
            >
              Get Started Now
            </button>
            <button
              onClick={() =>
                document
                  .getElementById("visas")
                  .scrollIntoView({ behavior: "smooth" })
              }
              className="bg-transparent border-2 border-white hover:bg-white hover:bg-opacity-10 font-semibold py-4 px-10 rounded-full shadow-xl transition-all duration-300 hover:scale-105 text-lg"
            >
              Explore Visas
            </button>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="container mx-auto py-16 px-4">
        <div className="text-center mb-16">
          <h2 className="text-3xl sm:text-4xl font-bold text-gray-800 mb-4 font-['Poppins']">
            Why Choose Us?
          </h2>
          <p className="text-lg text-gray-600 max-w-2xl mx-auto">
            We make visa applications simple, fast and stress-free
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
          <div className="bg-white p-8 rounded-2xl shadow-lg hover:shadow-xl transition-all duration-300 border border-gray-100">
            <div className="bg-indigo-100 w-16 h-16 rounded-full flex items-center justify-center mb-6 mx-auto">
              <FiClock className="text-3xl text-indigo-600" />
            </div>
            <h3 className="text-xl font-bold text-center mb-3 text-gray-800">
              Fast Processing
            </h3>
            <p className="text-gray-600 text-center">
              Get your visa approved in record time with our expedited services.
            </p>
          </div>

          <div className="bg-white p-8 rounded-2xl shadow-lg hover:shadow-xl transition-all duration-300 border border-gray-100">
            <div className="bg-blue-100 w-16 h-16 rounded-full flex items-center justify-center mb-6 mx-auto">
              <FiShield className="text-3xl text-blue-600" />
            </div>
            <h3 className="text-xl font-bold text-center mb-3 text-gray-800">
              Secure & Reliable
            </h3>
            <p className="text-gray-600 text-center">
              Your data is protected with bank-level security measures.
            </p>
          </div>

          <div className="bg-white p-8 rounded-2xl shadow-lg hover:shadow-xl transition-all duration-300 border border-gray-100">
            <div className="bg-purple-100 w-16 h-16 rounded-full flex items-center justify-center mb-6 mx-auto">
              <FiCreditCard className="text-3xl text-purple-600" />
            </div>
            <h3 className="text-xl font-bold text-center mb-3 text-gray-800">
              Flexible Payments
            </h3>
            <p className="text-gray-600 text-center">
              Multiple payment options including installments for your
              convenience.
            </p>
          </div>
        </div>
      </section>

      {/* Visa Cards Section */}
      <section id="visas" className="container mx-auto py-16 px-4">
        <div className="text-center mb-16">
          <h2 className="text-3xl sm:text-4xl font-bold text-gray-800 mb-4 font-['Poppins']">
            Available Visas
          </h2>
          <p className="text-lg text-gray-600 max-w-2xl mx-auto">
            Select from our range of visa options tailored for your needs
          </p>
        </div>

        {availableVisas.length > 0 ? (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
            {availableVisas.map((visa) => (
              <div key={visa.visaId} className="relative">
                <Visa VisaObj={visa} />
              </div>
            ))}
          </div>
        ) : (
          <div className="text-center py-16 bg-white rounded-2xl shadow-lg max-w-2xl mx-auto border border-gray-100">
            <div className="inline-block p-6">
              <div className="text-indigo-100 mb-6">
                <FiSmile className="h-20 w-20 mx-auto" />
              </div>
              <h3 className="text-2xl font-medium text-gray-700 mb-3 font-['Poppins']">
                New visas coming soon!
              </h3>
              <p className="text-gray-500 max-w-md mx-auto">
                We're currently updating our visa offerings. Check back later
                for exciting new destinations.
              </p>
              <button
                onClick={() => window.location.reload()}
                className="mt-6 bg-gradient-to-r from-indigo-500 to-blue-500 hover:from-indigo-600 hover:to-blue-600 text-white py-2 px-8 rounded-full font-medium shadow-md transition-all duration-300"
              >
                Refresh Page
              </button>
            </div>
          </div>
        )}
      </section>

      {/* Testimonials Section */}
      <section className="bg-indigo-50 py-16 px-4">
        <div className="container mx-auto">
          <div className="text-center mb-16">
            <h2 className="text-3xl sm:text-4xl font-bold text-gray-800 mb-4 font-['Poppins']">
              What Our Clients Say
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              Hear from travelers who've used our services
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            <div className="bg-white p-8 rounded-2xl shadow-md">
              <div className="flex items-center mb-4">
                <div className="text-yellow-400 text-xl">★★★★★</div>
              </div>
              <p className="text-gray-600 mb-6">
                "The visa process was incredibly smooth and faster than I
                expected. Highly recommend their services!"
              </p>
              <div className="flex items-center">
                <div className="bg-indigo-100 w-12 h-12 rounded-full flex items-center justify-center mr-4">
                  <span className="text-indigo-600 font-bold">AS</span>
                </div>
                <div>
                  <h4 className="font-bold text-gray-800">Ahmed Samir</h4>
                  <p className="text-sm text-gray-500">Business Traveler</p>
                </div>
              </div>
            </div>

            <div className="bg-white p-8 rounded-2xl shadow-md">
              <div className="flex items-center mb-4">
                <div className="text-yellow-400 text-xl">★★★★★</div>
              </div>
              <p className="text-gray-600 mb-6">
                "I was nervous about applying for my first visa, but their team
                guided me through every step. Excellent service!"
              </p>
              <div className="flex items-center">
                <div className="bg-indigo-100 w-12 h-12 rounded-full flex items-center justify-center mr-4">
                  <span className="text-indigo-600 font-bold">MN</span>
                </div>
                <div>
                  <h4 className="font-bold text-gray-800">Mariam Nasser</h4>
                  <p className="text-sm text-gray-500">Tourist</p>
                </div>
              </div>
            </div>

            <div className="bg-white p-8 rounded-2xl shadow-md">
              <div className="flex items-center mb-4">
                <div className="text-yellow-400 text-xl">★★★★☆</div>
              </div>
              <p className="text-gray-600 mb-6">
                "Quick turnaround time and responsive customer support. Will
                definitely use again for my next trip."
              </p>
              <div className="flex items-center">
                <div className="bg-indigo-100 w-12 h-12 rounded-full flex items-center justify-center mr-4">
                  <span className="text-indigo-600 font-bold">YK</span>
                </div>
                <div>
                  <h4 className="font-bold text-gray-800">Youssef Kamal</h4>
                  <p className="text-sm text-gray-500">Student</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* About Us Section */}
      <section className="py-16 px-4 bg-white">
        <div className="container mx-auto">
          <AboutUs />
        </div>
      </section>

      {/* CTA Section */}
      <section className="bg-gradient-to-r from-indigo-600 to-purple-600 text-white py-16 px-4">
        <div className="container mx-auto text-center max-w-4xl">
          <h2 className="text-3xl sm:text-4xl font-bold mb-6 font-['Poppins']">
            Ready to Start Your Journey?
          </h2>
          <p className="text-xl mb-8 opacity-90 max-w-2xl mx-auto">
            Apply for your visa today and experience the difference
          </p>
          <button
            onClick={() => navigate("/login")}
            className="bg-white text-indigo-600 hover:bg-gray-100 font-semibold py-4 px-12 rounded-full shadow-xl transition-all duration-300 hover:scale-105 text-lg"
          >
            Apply Now
          </button>
        </div>
      </section>

      {/* Footer */}
      <footer className="bg-gray-900 text-white py-12">
        <div className="container mx-auto px-4">
          <div className="flex flex-col md:flex-row justify-between items-center">
            <div className="mb-6 md:mb-0">
              <h3 className="text-2xl font-bold font-['Poppins'] mb-2">
                {SiteName}
              </h3>
              <p className="text-gray-400">
                Making travel accessible to everyone
              </p>
            </div>

            <div className="flex flex-col sm:flex-row gap-6 text-center sm:text-left">
              <div>
                <h4 className="font-bold mb-3 text-gray-300">Services</h4>
                <ul className="space-y-2">
                  <li>
                    <a
                      href="#"
                      className="text-gray-400 hover:text-white transition"
                    >
                      Tourist Visas
                    </a>
                  </li>
                  <li>
                    <a
                      href="#"
                      className="text-gray-400 hover:text-white transition"
                    >
                      Business Visas
                    </a>
                  </li>
                  <li>
                    <a
                      href="#"
                      className="text-gray-400 hover:text-white transition"
                    >
                      Student Visas
                    </a>
                  </li>
                </ul>
              </div>

              <div>
                <h4 className="font-bold mb-3 text-gray-300">Company</h4>
                <ul className="space-y-2">
                  <li>
                    <a
                      href="#"
                      className="text-gray-400 hover:text-white transition"
                    >
                      About Us
                    </a>
                  </li>
                  <li>
                    <a
                      href="#"
                      className="text-gray-400 hover:text-white transition"
                    >
                      Contact
                    </a>
                  </li>
                  <li>
                    <a
                      href="#"
                      className="text-gray-400 hover:text-white transition"
                    >
                      Careers
                    </a>
                  </li>
                </ul>
              </div>

              <div>
                <h4 className="font-bold mb-3 text-gray-300">Legal</h4>
                <ul className="space-y-2">
                  <li>
                    <a
                      href="/OurPrivacyAndTerms"
                      className="text-gray-400 hover:text-white transition"
                    >
                      Terms
                    </a>
                  </li>
                  <li>
                    <a
                      href="OurPrivacyAndTerms"
                      className="text-gray-400 hover:text-white transition"
                    >
                      Privacy
                    </a>
                  </li>
                  <li>
                    <a
                      href="#"
                      className="text-gray-400 hover:text-white transition"
                    >
                      Cookies
                    </a>
                  </li>
                </ul>
              </div>
            </div>
          </div>

          <div className="border-t border-gray-800 mt-10 pt-6 text-center text-gray-400">
            <p>
              © {new Date().getFullYear()} {SiteName}. All rights reserved.
            </p>
          </div>
        </div>
      </footer>
    </div>
  );
}
