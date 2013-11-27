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
using System.Windows.Media.Imaging;

namespace MyCities
{
    public partial class WeatherPage : PhoneApplicationPage
    {
        PhoneApplicationService service = PhoneApplicationService.Current;
        private City city;

        public WeatherPage()
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
                    Uri uri = new Uri(Utils.ResolveWeatherIcon(city.Weather.CurrentWeather[0].Item.Condition.ConditionCode), UriKind.Absolute);
                    iconCurrWeather.Source = new BitmapImage(uri);
                    temp.Text = city.Weather.CurrentWeather[0].Item.Condition.ConditionTemp + "\u00B0C";
                    condition.Text = city.Weather.CurrentWeather[0].Item.Condition.ConditionText;
                    ForecastListBox.ItemsSource = city.Weather.Forecast.Skip(1).Take(city.Weather.Forecast.Length - 1).ToArray();
                }
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show("Unable to load the Forecast.");
            }

            base.OnNavigatedTo(e);

        }

    }
}