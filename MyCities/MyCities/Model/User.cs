using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;

namespace MyCities.Model
{
    [DataContract]
    public class User
    {

        public static string KEY = "user";

        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "password")]
        public string Password { get; set; }
        [DataMember(Name = "first_name")]
        public string FirstName { get; set; } 
        [DataMember(Name = "last_name")]
        public string LastName { get; set; } 
        [DataMember(Name = "fb_id")]
        public string FbId { get; set; }
        [DataMember(Name = "login_state")]
        public string LoginState { get; set; }
    }
}
