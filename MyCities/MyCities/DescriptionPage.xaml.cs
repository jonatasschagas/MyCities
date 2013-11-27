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
    public partial class DescriptionPage : PhoneApplicationPage
    {

        PhoneApplicationService service = PhoneApplicationService.Current;
        private City city;
       
        public DescriptionPage()
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
                    string description = "<html><body style='background-color:black;color:white;' >" + city.Description + "</body></html>";
                    var htmlCode = HttpUtility.HtmlDecode(description);
                    webBrowser1.NavigateToString(htmlCode);
                }
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show("Unable to display the description of the city");
            }

            base.OnNavigatedTo(e);
        }

    }
}