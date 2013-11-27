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
using MyCities.Model;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Diagnostics;

namespace MyCities.Services
{
    public class BackendServicesStub
    {

        public static string GetCityUrl(string name)
        {
            return "http://54.204.10.235:8080/index/all_info/" + name;
        }

        public static string GetCityByGeoloc(string lat,string lon)
        {
            return "http://54.204.10.235:8080/get_city_by_geoloc/?lat=" + lat + "&lon=" + lon;
        }

        public static string GetUserUrl(string name)
        {
            return "http://54.204.10.235:8080/load_user/" + name;
        }

        public static string GetUserCitiesUrl(string name)
        {
            return "http://54.204.10.235:8080/load_user_cities/?name=" + HttpUtility.UrlEncode(name);
        }

        public static void SaveUserCitiesServer(string userId, string cityName)
        {
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            var uri = new Uri("http://54.204.10.235:8080/save_user_cities/", UriKind.Absolute);
            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}&{2}={3}", "name",HttpUtility.UrlEncode(userId),"city",HttpUtility.UrlEncode(cityName));

            webClient.Headers[HttpRequestHeader.ContentLength] = postData.Length.ToString();
            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(SaveUserCitiesServerCompleted);
            webClient.UploadStringAsync(uri, "POST", postData.ToString());
        }
       
        private static void SaveUserCitiesServerCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            MessageBox.Show("City added to the favorites!");
        }

        public static void SaveUserServer(User user)
        {
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(User));
            ser.WriteObject(stream1, user);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            var uri = new Uri("http://54.204.10.235:8080/save_user/", UriKind.Absolute);
            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "user_json", HttpUtility.UrlEncode(sr.ReadToEnd()));
           
            webClient.Headers[HttpRequestHeader.ContentLength] = postData.Length.ToString();
            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(SaveUserServerCompleted);
            webClient.UploadStringAsync(uri, "POST", postData.ToString());

        }

        private static void SaveUserServerCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            Debug.WriteLine("completed");
        }

    }
}
