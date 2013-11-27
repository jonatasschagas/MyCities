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
using Microsoft.Phone.Controls.Maps.Platform;
using Microsoft.Phone.Controls.Maps;

namespace MyCities
{
    public partial class MapPage : PhoneApplicationPage
    {
        PhoneApplicationService service = PhoneApplicationService.Current;
        private City city;

        public MapPage()
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
                    // Update the map to show the current location
                    Location ppLoc = new Location();
                    ppLoc.Latitude = city.Weather.Lat;
                    ppLoc.Longitude = city.Weather.Lon;
                    map.SetView(ppLoc, 10);

                    //update pushpin location and show
                    MapLayer.SetPosition(ppLocation, ppLoc);
                    ppLocation.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show("Unable to load the Images.");
            }

            base.OnNavigatedTo(e);

        }

    }
}