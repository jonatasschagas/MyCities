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
    public class UserCities
    {
        public static string KEY = "user_cities";

        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "cities")]
        public UserCity[] Cities { get; set; }
    }

    [DataContract]
    public class UserCity
    {
        [DataMember(Name = "name")]
        public string Name  { get; set; }
    }
}
