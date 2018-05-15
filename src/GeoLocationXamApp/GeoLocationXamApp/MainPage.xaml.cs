using Microsoft.AppCenter.Analytics;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GeoLocationXamApp
{
    public partial class MainPage : ContentPage
    {
        private IGeolocator locator;
        public MainPage()
        {
            InitializeComponent();
            locator = CrossGeolocator.Current;
            StartListening();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            GetLocation();
        }

        public async void GetLocation()
        {
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            lblPositionStatus.Text = position.Timestamp.ToString();
            lblPositionLatitude.Text = position.Latitude.ToString();
            lblPositionLongitude.Text = position.Longitude.ToString();
        }

        public async Task StartListening()
        {
            if (locator.IsListening) return;

            await locator.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true);
            locator.PositionChanged += Locator_PositionChanged;
            locator.PositionError += Locator_PositionError;
        }

        private void Locator_PositionError(object sender, PositionErrorEventArgs e)
        {
            lblError.Text = e.Error.ToString();
        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Analytics.TrackEvent("Position changed");
            Debug.WriteLine($"Position changed: {e.Position.Latitude.ToString()}, {e.Position.Longitude.ToString()}");
            lblPositionLatitude.Text = e.Position.Latitude.ToString();
            lblPositionLongitude.Text = e.Position.Longitude.ToString();
        }


    }
}
