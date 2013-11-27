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
using MyCities.Util;
using MyCities.Model;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace MyCities
{
    public partial class PicturesPage : PhoneApplicationPage
    {
        PhoneApplicationService service = PhoneApplicationService.Current;
        private City city;
        
        public PicturesPage()
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
                    ImagesListBox.ItemsSource = city.Pictures;
                }
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show("Unable to load the Images.");
            }

            base.OnNavigatedTo(e);

        }

        private void ImageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If real selection is happened, go to a AlbumPage
            if (ImagesListBox.SelectedIndex == -1) return;
            int selectedPicture = ImagesListBox.SelectedIndex;
            this.NavigationService.Navigate(new Uri("/ShowPicturePage.xaml?selectedPicture=" + selectedPicture, UriKind.Relative));
        }

    }
}