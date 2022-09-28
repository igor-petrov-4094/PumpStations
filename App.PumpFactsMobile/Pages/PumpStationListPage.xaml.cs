using App.PumpFactsMobile.ServiceDataModels;
using App.PumpFactsMobile.ViewModels;
using App.PumpFactsServiceClient;

namespace App.PumpFactsMobile.Pages
{
    public partial class PumpStationListPage : ContentPage
    {
        public PumpStationListPage()
        {
            InitializeComponent();

            var model = new PumpStationListPageViewModel();
            model.setParams(this, 10);
            this.BindingContext = model;
        }

        async private void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            var pumpStationInfo = ((ListView)sender).SelectedItem as PumpStationInfo;
            if (pumpStationInfo != null)
            {
                var page = new PumpStationDetailPage();
                page.Title = pumpStationInfo.psd.ReadableName;
                var model = new PumpStationDetailPageViewModel(); 
                model.setParams(page, 10, pumpStationInfo);
                page.BindingContext = model;             
                await Navigation.PushAsync(page);
            }
        }
    }
}