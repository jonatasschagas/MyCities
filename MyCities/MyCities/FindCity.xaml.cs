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
using MyCities.Services;
using MyCities.Util;
using System.IO;
using MyCities.Model;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MyCities
{
    public partial class FindCity : PhoneApplicationPage
    {
        public FindCity()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string cityName = cityNameTextBox.Text;
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