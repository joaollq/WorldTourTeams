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
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace WorldTourTeams
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class WorldTourTeams : Page
    {
        List<TeamInfo> teams;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public WorldTourTeams()
        {
            this.InitializeComponent();
            this.InitializeTeams();
            itemGridView.ItemsSource = teams;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            SettingsPane.GetForCurrentView().CommandsRequested += SettingCharmManager_CommandsRequested;
        }

        private void InitializeTeams() {
            teams = new List<TeamInfo>();
            String[] abbreviations = {"ALM","AST", "BEL", "BMC", "CAN", "FDJ", "GRS", "KAT", "LAM", "LTB", "MOV", "OPQ", "OGE", "ARG", "EUC", "SKY", "TSB", "TFR"};
            String[] names = { "Ag2r La Mondiale", "Astana Pro Team", "Belkin Pro Cycling Team", "BMC Racing Team", "Cannondale Pro Cycling", "FDJ.fr", "Garmin - Sharp", "Katusha Team", "Lampre - Merida", "Lotto - Belisol", "Movistar Team","OmegaPharma - Quick Step Cycling Team", "Orica - GreenEDGE", "Team Argos - Shimano", "Team Europcar", "Team Sky", "Team Tinkoff - Saxo", "Trek Factory Racing" };

            for (int i = 0; i < abbreviations.Length; i++) {
                teams.Add(new TeamInfo(names[i], "http://cqranking.com/men/images/Teams/2014/" + abbreviations[i] + ".gif", abbreviations[i]));
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }


        /// <summary>
        /// Invoked when an item within a group is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            TeamInfo info = ((TeamInfo)e.ClickedItem);
            this.Frame.Navigate(typeof(TeamRosterPage), info);
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        //3) Add my handler that shows the privacy text
        private void SettingCharmManager_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args) {
            args.Request.ApplicationCommands.Add(new SettingsCommand("privacypolicy", "Privacy policy", OpenPrivacyPolicy));
        }


        //4) Add OpenPrivacyPolicy method
        private async void OpenPrivacyPolicy(IUICommand command) {
            Uri uri = new Uri("https://dl.dropboxusercontent.com/u/12121505/privacyEnglish.html");
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }

   
}