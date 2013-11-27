using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MyCities.Services;
using MyCities.Model;
using System.Device.Location;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using MyCities.Util;

namespace MyCities
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher watcher;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the city from the state and populates the screen
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            try
            {
                User user = ApplicationStateHelper.LoadUserFromIsolatedStorage();

                if (user != null)
                {
                    ContentPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    LoginPanel.Visibility = Visibility.Visible;
                }
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show("Unable to load the user.");
            }

            base.OnNavigatedTo(e);

        }

        private void findCityTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FindCity.xaml", UriKind.Relative));
        }

        private void btnFacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FacebookLoginPage.xaml", UriKind.Relative));
        }

        private void myFavoriteCitiesTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MyFavoriteCitiesPage.xaml", UriKind.Relative));
        }

        private void whereAmITextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            if (watcher == null)
            {
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                watcher.MovementThreshold = 20;
                watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            }
            watcher.Start();
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    MessageBox.Show("Location Service is not enabled on the device");
                    break;

                case GeoPositionStatus.NoData:
                    MessageBox.Show(" The Location Service is working, but it cannot get location data.");
                    break;
            }
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (e.Position.Location.IsUnknown)
            {
                return;
            }

            string lat = e.Position.Location.Latitude.ToString();
            string lon = e.Position.Location.Longitude.ToString();
            watcher.Stop();

            string url = BackendServicesStub.GetCityByGeoloc(lat, lon);
            try
            {
                WebClient client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(searchCityByGeoCallBack);
                client.DownloadStringAsync(new Uri(url, UriKind.Absolute));
                progressBar.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                progressBar.Visibility = Visibility.Collapsed;
                MessageBox.Show("City could not be found!");
                return;
            }
        }

        private void searchCityByGeoCallBack(object sender, DownloadStringCompletedEventArgs e)
        {
            progressBar.Visibility = Visibility.Collapsed;
            try
            {
                if ((e.Result != null) && (e.Error == null))
                {

                    string jsonString = e.Result.ToString();

                    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
                    {
                        var response = new DataContractJsonSerializer(typeof(CityByGeo));
                        CityByGeo cityByGeo = (CityByGeo)response.ReadObject(ms);

                        string cityName = cityByGeo.Name;
                        string url = BackendServicesStub.GetCityUrl(cityName);
                        try
                        {
                            WebClient client = new WebClient();
                            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(searchCityCallBack);
                            client.DownloadStringAsync(new Uri(url, UriKind.Absolute));
                            progressBar.Visibility = Visibility.Visible;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("City could not be found!");
                            return;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("City could not be found!");
                return;
            }
        }

        private void searchCityCallBack(object sender, DownloadStringCompletedEventArgs e)
        {
            progressBar.Visibility = Visibility.Collapsed;
            try
            {
                if ((e.Result != null) && (e.Error == null))
                {

                    string jsonString = e.Result.ToString();

                    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
                    {
                        var response = new DataContractJsonSerializer(typeof(City));
                        City city = (City)response.ReadObject(ms);
                        // saving state and moving to the next page
                        ApplicationStateHelper.SaveCityToAppState(city);
                        NavigationService.Navigate(new Uri("/CityPage.xaml", UriKind.Relative));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("City could not be found!");
                return;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string user = userText.Text;
            string password = passwordText.Text;

            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var uri = new Uri("http://54.204.10.235:8080/login/", UriKind.Absolute);
                StringBuilder postData = new StringBuilder();
                postData.AppendFormat("{0}={1}&{2}={3}", "user", HttpUtility.UrlEncode(user), "password", HttpUtility.UrlEncode(password));

                webClient.Headers[HttpRequestHeader.ContentLength] = postData.Length.ToString();
                webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(loginComplete);
                webClient.UploadStringAsync(uri, "POST", postData.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Login. Try again...");
                return;
            }
        }

        private void loginComplete(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                if ((e.Result != null) && (e.Error == null))
                {

                    string jsonString = e.Result.ToString();

                    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
                    {
                        var response = new DataContractJsonSerializer(typeof(User));
                        User user = (User)response.ReadObject(ms);

                        if (user.LoginState == "ok")
                        {
                            // saving state and moving to the next page
                            ApplicationStateHelper.SaveUserToIsolatedStorage(user);
                            LoginPanel.Visibility = Visibility.Collapsed;
                            ContentPanel.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            MessageBox.Show("Invalid user and password.");
                        }
                        
                    }
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid User and Password!");
            }
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SignInPage.xaml", UriKind.Relative));
        }
    }
}