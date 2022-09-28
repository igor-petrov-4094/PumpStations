using App.PumpFactsMobile.ServiceDataModels;
using App.PumpFactsMobile.Utils;
using App.PumpFactsMobile.ViewModels;

using App.PumpFactsServiceClient;

namespace App.PumpFactsMobile.Pages
{
	public partial class PumpStationDetailPage : ContentPage
	{
		public PumpStationDetailPage()
		{
			InitializeComponent();
		}

		async private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
		{
            PumpStationDetailPageViewModel currentModel = this.BindingContext as PumpStationDetailPageViewModel;
            PumpStationInfo pumpStationInfo = currentModel.pumpStationInfo;

            var page = new PumpStationLeftPage();
            page.Title = pumpStationInfo.psd.ReadableName;

            var newModel = new PumpStationLeftPageViewModel();
            newModel.setParams(page, 10, pumpStationInfo);
            page.BindingContext = newModel;
            await Navigation.PushAsync(page);
        }
    }
}