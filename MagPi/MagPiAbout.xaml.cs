using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Reflection;

namespace MagPi
{
    public partial class MagPiAbout : PhoneApplicationPage
    {
        public MagPiAbout()
        {
            InitializeComponent();
            var thisApp = Assembly.GetExecutingAssembly();
            AssemblyName name = new AssemblyName(thisApp.FullName);
            versionText.Text = "v. " + name.Version.ToString();
        }
    }
}