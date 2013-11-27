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
    public partial class ShowPicturePage : PhoneApplicationPage
    {

        PhoneApplicationService service = PhoneApplicationService.Current;
        private City city;
        private int selectedPicture;
        private GestureListener gestureListener;

        public ShowPicturePage()
        {
            InitializeComponent();

            // Initialize GestureListener
            gestureListener = GestureService.GetGestureListener(ContentPanel);
            // Handle Dragging (to show next or previous image from Album)
            gestureListener.DragCompleted += new EventHandler<DragCompletedGestureEventArgs>(gestureListener_DragCompleted);
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
                selectedPicture = Convert.ToInt16(NavigationContext.QueryString["selectedPicture"]); 

                if (city != null)
                {
                    LoadImage();
                }
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show("Unable to load the Images.");
            }

            base.OnNavigatedTo(e);

        }

        // Gesture - Drag is complete
        void gestureListener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
        {   

            // Left or Right
            if (e.HorizontalChange > 0)
            {
                // previous image (or last if first is shown)
                selectedPicture--;
                if (selectedPicture < 0) selectedPicture = city.Pictures.Length - 1;
            }
            else
            {
                // next image (or first if last is shown)
                selectedPicture++;
                if (selectedPicture > (city.Pictures.Length - 1)) selectedPicture = 0;
            }
            
            LoadImage();
        }

        private void LoadImage()
        {
            BitmapImage bitmapImage = picture.Source as BitmapImage;
            if (bitmapImage != null)
            {
                bitmapImage.UriSource = null;
                picture.Source = null;
            }
            
            Uri uri = new Uri(city.Pictures[selectedPicture].Image, UriKind.Absolute);
            picture.Source = new BitmapImage(uri);
        }

    }

}