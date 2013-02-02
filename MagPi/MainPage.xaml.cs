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
using System.Text.RegularExpressions;
using Microsoft.Phone.Shell;

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
            Refresh();
        }

        void issues_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            prgLoadingIssues.Visibility = System.Windows.Visibility.Collapsed;

            if (e.Error != null)
                return;

            XElement xmlIssues = XElement.Parse(e.Result);

            foreach (XElement issue in xmlIssues.Descendants("item"))
            {
                Match match = Regex.Match(issue.Element("description").Value, @"img src='(.*?)'", RegexOptions.IgnoreCase);

                Debug.WriteLine("Title {0}", issue.Element("title").Value.Split('-')[0]);
                Debug.WriteLine("Date {0}", issue.Element("title").Value.Split('-')[1]);
                Debug.WriteLine("Image {0}", match.Groups[1].Value);
                Debug.WriteLine("Pdf {0}", issue.Element("link").Value);
            }

            lstIssues.ItemsSource = from issue in xmlIssues.Descendants("item")
                                     select new Issue
                                     {
                                         Title = issue.Element("title").Value.Split('-')[0],
                                         Date = issue.Element("title").Value.Split('-')[1],
                                         ImageUrl = Regex.Match(issue.Element("description").Value, @"img src='(.*?)'", RegexOptions.IgnoreCase).Groups[1].Value,
                                         PdfUrl = issue.Element("link").Value,
                                     };
        }

        void news_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            prgLoadingNews.Visibility = System.Windows.Visibility.Collapsed;

            if (e.Error != null)
                return;

            XElement xmlNews = XElement.Parse(e.Result);

            foreach (XElement news in xmlNews.Descendants("item"))
            {
                Debug.WriteLine("Description {0}", news.Element("description").Value);
                Debug.WriteLine("Date {0}", String.Format("{0:MM/dd/yyyy}", DateTime.Parse(news.Element("pubDate").Value.Remove(news.Element("pubDate").Value.IndexOf(" +")))));
            }

            lstNews.ItemsSource = from news in xmlNews.Descendants("item")
                                     select new News
                                     {
                                         Date = String.Format("{0:MM/dd/yyyy}", DateTime.Parse(news.Element("pubDate").Value.Remove(news.Element("pubDate").Value.IndexOf(" +")))),
                                         Content = news.Element("description").Value
                                     };
        }

        private void RefreshEverything_Click(object sender, EventArgs e)
        {
            prgLoadingIssues.Visibility = System.Windows.Visibility.Visible;
            prgLoadingNews.Visibility = System.Windows.Visibility.Visible;
            Refresh();
        }

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MagPiAbout.xaml", UriKind.Relative));
        }

        private void Refresh()
        {
            WebClient issues = new WebClient();
            issues.DownloadStringCompleted += new DownloadStringCompletedEventHandler(issues_DownloadStringCompleted);
            issues.DownloadStringAsync(new Uri("http://feeds.feedburner.com/theMagPi"));

            WebClient news = new WebClient();
            news.DownloadStringCompleted += new DownloadStringCompletedEventHandler(news_DownloadStringCompleted);
            news.DownloadStringAsync(new Uri("http://feeds.feedburner.com/MagPi"));
        }

        private void OpenIssue(object sender, GestureEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            Issue issue = (Issue)listBox.SelectedItem;

            if (issue != null)
            {
                if(!string.IsNullOrEmpty(issue.Title))
                    PhoneApplicationService.Current.State["Title"] = issue.Title;
                if (!string.IsNullOrEmpty(issue.Title))
                    PhoneApplicationService.Current.State["Date"] = issue.Date;
                if (!string.IsNullOrEmpty(issue.Title))
                    PhoneApplicationService.Current.State["ImageUrl"] = issue.ImageUrl;
                if (!string.IsNullOrEmpty(issue.Title))
                    PhoneApplicationService.Current.State["PdfUrl"] = issue.PdfUrl;

                NavigationService.Navigate(new Uri("/MagPiIssue.xaml", UriKind.Relative));
            }
        }
    }
}