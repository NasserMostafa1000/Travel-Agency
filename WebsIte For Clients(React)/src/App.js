import { Routes, Route, Navigate } from "react-router-dom";
import UpdateImages from "./Components/UploadAndUpdateImages";
import Home from "./Components/Home";
import MyProfile from "./Components/MyProfile";
import "./Styles/Index.css";
import Orders from "./Components/Orders";
import Login from "./Components/Log in";
import SignUp from "./Components/Signup";
import PayWithVisa from "./Components/PayWithVisa";
import OurPrivacyAndTerms from "./Components/ourPrivacyAndTerms";
import ForgetPassword from "./Components/ForgetPassword";
import NavBar from "./Components/NavBar";
export default function App() {
  return (
    <div className="flex flex-col min-h-screen">
      {/* Navbar دايمًا فوق */}
      <NavBar />

      {/* المحتوى المتغير */}
      <main className="flex-1 overflow-y-auto px-4 py-6">
        <Routes>
          <Route path="/" element={<Navigate to="/Home" />} />
          <Route path="/Home" element={<Home />} />
          <Route path="/Signup" element={<SignUp />} />
          <Route path="/OurPrivacyAndTerms" element={<OurPrivacyAndTerms />} />
          <Route path="/PayWithVisa" element={<PayWithVisa />} />
          <Route path="/Login" element={<Login />} />
          <Route path="/UpdateImages" element={<UpdateImages />} />
          <Route path="/LogOut" element={<Login />} />
          <Route path="/MyProfile" element={<MyProfile />} />
          <Route path="/Orders" element={<Orders />} />
          <Route path="/forgot-password" element={<ForgetPassword />} />
        </Routes>
      </main>
    </div>
  );
}
