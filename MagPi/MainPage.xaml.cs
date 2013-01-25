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
using System.Windows.Navigation;
using System.Diagnostics;
using MagPi.Models;
using System.Xml.Linq;

namespace MagPi
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            WebClient issues = new WebClient();
            issues.DownloadStringCompleted += new DownloadStringCompletedEventHandler(issues_DownloadStringCompleted);
            issues.DownloadStringAsync(new Uri("http://feeds.feedburner.com/theMagPi"));

            WebClient news = new WebClient();
            news.DownloadStringCompleted += new DownloadStringCompletedEventHandler(news_DownloadStringCompleted);
            news.DownloadStringAsync(new Uri("http://feeds.feedburner.com/MagPi"));
        }

        void issues_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            prgLoadingIssues.Visibility = System.Windows.Visibility.Collapsed;

            if (e.Error != null)
                return;

            XElement xmlIssues = XElement.Parse(e.Result);

            lstIssues.ItemsSource = from issue in xmlIssues.Descendants("item")
                                     select new Issue
                                     {
                                         Title = issue.Element("title").Value.Split('-')[0],
                                         Date = issue.Element("title").Value.Split('-')[1]
                                     };
        }

        void news_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            prgLoadingNews.Visibility = System.Windows.Visibility.Collapsed;

            if (e.Error != null)
                return;

            XElement xmlNews = XElement.Parse(e.Result);

            lstNews.ItemsSource = from news in xmlNews.Descendants("item")
                                     select new News
                                     {
                                         Date = news.Element("title").Value,
                                         Content = news.Element("description").Value
                                     };
        }
    }
}