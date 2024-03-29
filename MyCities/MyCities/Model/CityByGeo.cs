﻿using System;
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
    public class CityByGeo
    {

        public static string KEY = "CityByGeo";

        [DataMember(Name = "name")]
        public string Name { get; set; }  
    }
}
