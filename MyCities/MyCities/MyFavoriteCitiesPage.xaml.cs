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
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyCities.Model;
using MyCities.Util;
using MyCities.Services;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace MyCities
{
    public partial class MyFavoriteCitiesPage : PhoneApplicationPage
    {

        private UserCities userCities;
        public MyFavoriteCitiesPage()
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
                string url = BackendServicesStub.GetUserCitiesUrl(user.Name);
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(getUserCitiesCallBack);
                webClient.DownloadStringAsync(new Uri(url, UriKind.Absolute));
                progressBar.Visibility = Visibility.Visible;
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show("Unable to load the cities.");
            }

            base.OnNavigatedTo(e);

        }

        private void getUserCitiesCallBack(object sender, DownloadStringCompletedEventArgs e)
        {
            progressBar.Visibility = Visibility.Collapsed;
            try
            {
                if ((e.Result != null) && (e.Error == null))
                {

                    string jsonString = e.Result.ToString();

                    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
                    {
                        var response = new DataContractJsonSerializer(typeof(UserCities));
                        userCities = (UserCities)response.ReadObject(ms);
                        MyFavoriteCitiesListBox.ItemsSource = userCities.Cities;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load your favorite cities.");
                return;
            }
        }

        private void MyFavoriteCitiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If real selection is happened, go to a AlbumPage
            if (MyFavoriteCitiesListBox.SelectedIndex == -1) return;
            int selectedCity = MyFavoriteCitiesListBox.SelectedIndex;
            string cityName = userCities.Cities[selectedCity].Name;
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

    }
}