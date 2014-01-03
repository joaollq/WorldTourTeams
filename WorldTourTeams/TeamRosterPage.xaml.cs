using WorldTourTeams.Common;
using WorldTourTeams.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.ApplicationSettings;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace WorldTourTeams {
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class TeamRosterPage : Page {
        private TeamInfo team;
        private HttpClient client;
        List<RiderInfo> teamRoster;


        public TeamRosterPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            team = e.Parameter as TeamInfo;
            pageTitle.Text = team.teamName;

            GetPhotos();
        }

        private async void GetPhotos() {

            HttpResponseMessage response = null;
            Stream result = null;
            string parsedResult = "";
            teamRoster = new List<RiderInfo>();

            client = new HttpClient();
            try {
                response = await client.GetAsync("http://cqranking.com/men/asp/gen/team_photos.asp?year=2014&teamcode=" + team.teamAbbreviation.ToLower());
                result = await response.Content.ReadAsStreamAsync();
                parsedResult = ParseHtmlResponse(result, parsedResult);

                string[] lines = parsedResult.Split('\n');
                int i;

                for (i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("img src='../../images/Riders/"))
                    {
                        break;
                    }
                }


                ParseRiderInfo(lines, i);
                teamView.ItemsSource = teamRoster;       
            } catch (Exception) {
                var dialog = new Windows.UI.Popups.MessageDialog("You must be connected to the internet to use this app");


                dialog.Commands.Add(new UICommand("Close", new UICommandInvokedHandler(this.CloseApp)));
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;
                dialog.ShowAsync();
            }



                    }

        private static string ParseHtmlResponse(Stream result, string parsedResult) {
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[8192];
            string tempString = null;
            int count = 0;
            do {
                count = result.Read(buf, 0, buf.Length);
                if (count != 0) {
                    tempString = Encoding.GetEncoding("Windows-1252").GetString(buf, 0, count);
                    sb.Append(tempString);
                }
            }
            while (count > 0);

            parsedResult = sb.ToString();
            return parsedResult;
        }

        private void ParseRiderInfo(string[] lines, int i) {
            string[] riders = Regex.Split(lines[i], "</td>");

            foreach (string rider in riders) {
                if (rider.Contains("img src=")) {
                    string image = "http://cqranking.com/men/images/Riders/2014/" + Regex.Match(rider, @"[A-Za-z0-9\-]+.jpg").Value;
                    string name = Regex.Match(rider, @"[A-Z\u00C0-\u00DC ]+ [A-Z]{1}[a-z\u00E0-\u00FD]+").Value;
                    teamRoster.Add(new RiderInfo(name, image));

                }
            }
        }

        private void CloseApp(IUICommand command) {
            Application.Current.Exit();
        }

        class RiderInfo {
            public String riderPhoto { get; set; }
            public String riderName { get; set; }

            public RiderInfo(String riderName, String riderPhoto) {
                this.riderPhoto = riderPhoto;
                this.riderName = riderName;
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(WorldTourTeams));
        }
    }
}