using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MyCities.Util
{
    public class Utils
    {
        /// <summary>
        /// Helper method that returns the string from the resource file based on the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetMessage(string key)
        {
            return MyCities.Localization.Localization.ResourceManager.GetString(key,
                MyCities.Localization.Localization.Culture);
        }

        private const string BACKEND_URL = "http://54.204.10.235";
        public static string ResolveWeatherIcon(string codestr)
        {
            int code = Convert.ToInt16(codestr);
            if (code >= 0 && code <= 4 || code >= 37 && code <= 38)
            {
                return BACKEND_URL + "/Images/storm.png";
            }

            if (code >= 5 && code <= 14 || code == 45 || code == 39)
            {
                return BACKEND_URL + "/Images/rainy.png";
            }

            if (code >= 15 && code <= 16)
            {
                return BACKEND_URL + "/Images/snow.png";
            }

            if (code == 17 || code == 18)
            {
                return BACKEND_URL + "/Images/hail.png";
            }

            if (code >= 19 && code <= 22)
            {
                return BACKEND_URL + "/Images/fog.png";
            }

            if (code >= 23 && code <= 30)
            {
                return BACKEND_URL + "/Images/partly-cloudy.png";
            }

            if (code == 31 || code == 32)
            {
                return BACKEND_URL + "/Images/sunny.png";
            }

            if (code == 33 || code == 34)
            {
                return BACKEND_URL + "/Images/fair.png";
            }

            return null;
        }

        public static string GetDeviceId()
        {
            object uniqueID;
            if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueID) == true)
            {
                byte[] bID = (byte[])uniqueID;
                string deviceID = Convert.ToBase64String(bID);   // There you go
                return deviceID;
            }
            return null;
        }

    }
}
