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
using MyCities.Util;

namespace MyCities.Model
{
    [DataContract]
    public class City
    {

        public static string KEY = "city";

        [DataMember(Name = "weather")]
        public Weather Weather { get; set; }
        [DataMember(Name = "pictures")]
        public Picture[] Pictures { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [DataContract]
    public class Picture
    {
        [DataMember(Name = "image_thumb")]
        public string ImageThumb { get; set; }

        [DataMember(Name = "image")]
        public string Image { get; set; }
    }

    [DataContract]
    public class Forecast
    {
        public string ImageForecast
        {
            get
            {
                return Utils.ResolveWeatherIcon(Code);
            }
            set
            {
            }
        }
        public string Temp
        {
            get
            {
                return "H:" + High + " \u00B0C, L: " + Low + "\u00B0C";
            }
        }
        [DataMember(Name = "code")]
        public string Code { get; set; }
        [DataMember(Name = "text")]
        public string Text { get; set; }
        [DataMember(Name = "high")]
        public string High { get; set; }
        [DataMember(Name = "low")]
        public string Low { get; set; }
        [DataMember(Name = "date")]
        public string Date { get; set; }
        [DataMember(Name = "day")]
        public string Day { get; set; }
    }

    [DataContract]
    public class Weather
    {
        [DataMember(Name = "woeid")]
        public string Woeid { get; set; }
        [DataMember(Name = "weather")]
        public CurrentWeather[] CurrentWeather;
        [DataMember(Name = "forecast")]
        public Forecast[] Forecast;
        [DataMember(Name = "lat")]
        public double Lat { get; set; }
        [DataMember(Name = "lon")]
        public double Lon { get; set; }    
    }

    [DataContract]
    public class CurrentWeather
    {
        [DataMember(Name = "item")]
        public CurrentWeatherItem Item { get; set; }
    }

    [DataContract]
    public class CurrentWeatherItem
    {
        [DataMember(Name = "condition")]
        public CurrentWeatherItemCondition Condition { get; set; }
    }

    [DataContract]
    public class CurrentWeatherItemCondition
    {
        [DataMember(Name = "text")]
        public string ConditionText { get; set; }

        [DataMember(Name = "temp")]
        public string ConditionTemp
        {
            get;
            set;
        }

        [DataMember(Name = "code")]
        public string ConditionCode { get; set; }
    }
}
