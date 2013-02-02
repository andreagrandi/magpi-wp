using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace MagPi
{
    public partial class MagPiIssue : PhoneApplicationPage
    {
        private string title;
        private string date;
        private string image;
        private string pdfurl;

        public MagPiIssue()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (PhoneApplicationService.Current.State.ContainsKey("Title"))
                title = (string)PhoneApplicationService.Current.State["Title"];
            if (PhoneApplicationService.Current.State.ContainsKey("Date"))
                date = (string)PhoneApplicationService.Current.State["Date"];
            if (PhoneApplicationService.Current.State.ContainsKey("ImageUrl"))
                image = (string)PhoneApplicationService.Current.State["ImageUrl"];
            if (PhoneApplicationService.Current.State.ContainsKey("PdfUrl"))
                pdfurl = (string)PhoneApplicationService.Current.State["PdfUrl"];

            if (!string.IsNullOrEmpty(title) & !string.IsNullOrEmpty(date))
                issueTitle.Text = title + " - " + date;

            if (!string.IsNullOrEmpty(image))
                issueImage.Source = new BitmapImage(new Uri(image, UriKind.RelativeOrAbsolute)); ;
        }
    }
}