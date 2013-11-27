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
using System.Text;
using System.IO;
using MyCities.Model;
using System.Runtime.Serialization.Json;
using MyCities.Util;

namespace MyCities
{
    public partial class SignInPage : PhoneApplicationPage
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click_1(object sender, RoutedEventArgs e)
        {
            string user = userTextBox.Text;
            string password = passwordText.Text;
            progressBarLogin.Visibility = Visibility.Visible;
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var uri = new Uri("http://54.204.10.235:8080/signin/", UriKind.Absolute);
                StringBuilder postData = new StringBuilder();
                postData.AppendFormat("{0}={1}&{2}={3}", "user", HttpUtility.UrlEncode(user), "password", HttpUtility.UrlEncode(password));

                webClient.Headers[HttpRequestHeader.ContentLength] = postData.Length.ToString();
                webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(signinComplete);
                webClient.UploadStringAsync(uri, "POST", postData.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Sign In.");
                return;
            }

        }

        private void signinComplete(object sender, UploadStringCompletedEventArgs e)
        {
            progressBarLogin.Visibility = Visibility.Collapsed;
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
                            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        }
                        else
                        {
                            MessageBox.Show("Unable to Sign In. Try again...");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Sign In.");
            }
        }


    }
}