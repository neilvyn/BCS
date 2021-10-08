using System;
using Xamarin.Forms;

namespace BCSTech.Services.Predefined
{
    public class Constants
    {
        #region urls
        private const string Local_BaseUrl = "127.0.0.1";
        private const string Exam_BaseUrl = "https://discoverycodetest.azurewebsites.net";

#if DEBUG
        private static string BaseUrl = Local_BaseUrl;
#elif STAGING
        private static string BaseUrl = Exam_BaseUrl;
#elif RELEASE
        private static string BaseUrl = Exam_BaseUrl;
#else
        private static string BaseUrl = Exam_BaseUrl;
#endif

        public static string BASE_ADDRESS = BaseUrl;
        private static string ROUTE_API = "/api";
        private static string ROUTE_NPS = "/NPS";

        public static string URL_CUSTOMERS = BASE_ADDRESS + ROUTE_API + ROUTE_NPS + "/Customers";
        public static string URL_RESPONSE = BASE_ADDRESS + ROUTE_API + ROUTE_NPS + "/Response";
        #endregion

        #region keys
        public static string AUTH_HEADER = "SampleHeaderAuthTokenPass@1234";
        #endregion

        #region default_flag_responses
        public static int FLAG_SUCCESS = 200;
        public static int FLAG_NOT_FOUND = 404;
        public static int FLAG_VALIDATION_ERROR = 400;
        public static int FLAG_SERVER_ERROR = 500;
        public static int FLAG_SESSION_EXPIRED = 401;
        #endregion

        #region alert_messages
        public static string CriticalTitleAlert = "Error";
        public static string DefaultTitleAlert = "Alert Message";
        public static string AlertPositiveLabel = "Got It!";
        public static string DefaultSuccessAlert = "Updated Successfully";

        public static AlertMessage HOST_UNREACHABLE = new AlertMessage { Title = "Host Unreachable", Description = "The URL cannot be reached and seems to be unavailable. Please try again later." };
        public static AlertMessage NO_CONNECTION = new AlertMessage { Title = "Failed Connection", Description = "Please check your internet connection and try again." };
        #endregion

        #region colors
        public static readonly Color HEADER_COLOR = Color.FromHex("d1d1d1");
        public static readonly Color BUTTON_COLOR = Color.FromHex("097bed");
        public static readonly Color ALERT_COLOR = Color.Red;
        #endregion

        #region screen_pages
        public static string NAVIGATION_PAGE = "NavigationPage";
        public static string SearchPage = "SearchPage";
        public static string CustomerDetailPage = "CustomerDetailPage";
        #endregion
    }

    public class AlertMessage
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
