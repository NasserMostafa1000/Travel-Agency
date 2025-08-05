export default function OrderCom({ orderObj }) {
  if (!orderObj) {
    return (
      <div className="animate-pulse bg-gray-100 rounded-lg p-6">
        <div className="h-6 bg-gray-300 rounded w-1/3 mb-4"></div>
        <div className="space-y-3">
          <div className="h-4 bg-gray-300 rounded w-full"></div>
          <div className="h-4 bg-gray-300 rounded w-2/3"></div>
          <div className="h-4 bg-gray-300 rounded w-3/4"></div>
          <div className="h-4 bg-gray-300 rounded w-1/2"></div>
        </div>
      </div>
    );
  }

  const statusClass = getStatusClass(orderObj.order_Status);
  const statusText = getStatusText(orderObj.order_Status);

  return (
    <div className="bg-white rounded-lg shadow-md overflow-hidden border border-gray-200 hover:shadow-lg transition-shadow duration-300 mb-4">
      {/* Order Header */}
      <div
        className={`px-6 py-4 border-b ${getStatusBorderColor(statusClass)}`}
      >
        <div className="flex justify-between items-center">
          <h2 className="text-lg font-semibold text-gray-800">
            Order #{orderObj.orderID}
          </h2>
          <span
            className={`px-3 py-1 rounded-full text-xs font-medium ${getStatusBadgeColor(
              statusClass
            )}`}
          >
            {statusText}
          </span>
        </div>
      </div>

      {/* Order Details */}
      <div className="px-6 py-4">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div className="flex items-start">
            <div className="mr-3 mt-0.5">
              <svg
                className="h-5 w-5 text-gray-500"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2"
                />
              </svg>
            </div>
            <div>
              <p className="text-sm text-gray-500">Item</p>
              <p className="font-medium text-gray-900">{orderObj.visa_Name}</p>
            </div>
          </div>

          <div className="flex items-start">
            <div className="mr-3 mt-0.5">
              <svg
                className="h-5 w-5 text-gray-500"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
            </div>
            <div>
              <p className="text-sm text-gray-500">Type</p>
              <p className="font-medium text-gray-900">
                {orderObj.orderType === "تجديد" ? "Renewal" : "New Issue"}
              </p>
            </div>
          </div>

          <div className="flex items-start">
            <div className="mr-3 mt-0.5">
              <svg
                className="h-5 w-5 text-gray-500"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
            </div>
            <div>
              <p className="text-sm text-gray-500">Rejection Reason</p>
              <p className="font-medium text-gray-900">
                {orderObj.rejectionReason || "No issues detected"}
              </p>
            </div>
          </div>

          <div className="flex items-start">
            <div className="mr-3 mt-0.5">
              <svg
                className="h-5 w-5 text-gray-500"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"
                />
              </svg>
            </div>
            <div>
              <p className="text-sm text-gray-500">Date</p>
              <p className="font-medium text-gray-900">{orderObj.date}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

// Helper functions
function getStatusClass(status) {
  switch (status) {
    case "تحت المعالجة":
      return "processing";
    case "مكتمل":
      return "complete";
    case "نواقص":
      return "bad-order";
    default:
      return "rejected";
  }
}

function getStatusText(status) {
  switch (status) {
    case "تحت المعالجة":
      return "Processing";
    case "مكتمل":
      return "Completed";
    case "نواقص":
      return "Bad Order";
    default:
      return "Rejected";
  }
}

function getStatusBadgeColor(status) {
  switch (status) {
    case "processing":
      return "bg-blue-100 text-blue-800";
    case "complete":
      return "bg-green-100 text-green-800";
    case "bad-order":
      return "bg-yellow-100 text-yellow-800";
    case "rejected":
      return "bg-red-100 text-red-800";
    default:
      return "bg-gray-100 text-gray-800";
  }
}

function getStatusBorderColor(status) {
  switch (status) {
    case "processing":
      return "border-blue-200";
    case "complete":
      return "border-green-200";
    case "bad-order":
      return "border-yellow-200";
    case "rejected":
      return "border-red-200";
    default:
      return "border-gray-200";
  }
}
