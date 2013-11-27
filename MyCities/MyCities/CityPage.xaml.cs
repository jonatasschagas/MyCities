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
using MyCities.Model;
using Microsoft.Phone.Shell;
using MyCities.Util;
using System.Windows.Media.Imaging;
using MyCities.Services;

namespace MyCities
{
    public partial class CityPage : PhoneApplicationPage
    {
        PhoneApplicationService service = PhoneApplicationService.Current;
        private City city;
       
        public CityPage()
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
                city = ApplicationStateHelper.LoadCityFromAppState();

                if (city != null)
                {
                    cityNameTitle.Text = city.Name;
                    Uri uri = new Uri(city.Pictures[0].Image, UriKind.Absolute);
                    imageCity.Source = new BitmapImage(uri);
                }
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show(Utils.GetMessage("UnableToRetrieveWeather"));
            }

            base.OnNavigatedTo(e);

        }

        private void picturesTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ApplicationStateHelper.SaveCityToAppState(city);
            NavigationService.Navigate(new Uri("/PicturesPage.xaml", UriKind.Relative));
        }

        private void descriptionTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ApplicationStateHelper.SaveCityToAppState(city);
            NavigationService.Navigate(new Uri("/DescriptionPage.xaml", UriKind.Relative));
        }

        private void weatherTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ApplicationStateHelper.SaveCityToAppState(city);
            NavigationService.Navigate(new Uri("/WeatherPage.xaml", UriKind.Relative));
        }

        private void addToFavoritesTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            User user = ApplicationStateHelper.LoadUserFromIsolatedStorage();
            string cityName = city.Name;
            BackendServicesStub.SaveUserCitiesServer(user.Name, cityName);
        }

        private void mapTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ApplicationStateHelper.SaveCityToAppState(city);
            NavigationService.Navigate(new Uri("/MapPage.xaml", UriKind.Relative));
        }

    }
}