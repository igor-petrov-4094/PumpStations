using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using App.PumpFactsServiceClient;
using App.PumpFactsMobile.Utils;
using App.PumpFactsMobile.ServiceDataModels;

namespace App.PumpFactsMobile.ViewModels
{
    public class PumpStationListPageViewModel : AutoRefreshableCollectionViewModel<PumpStationInfo>
    {
        public PumpStationListPageViewModel()
        {
            this.getDataDelegate = getData;
            this.getObjectKeyDelegate = getObjectKey;
            this.getDataDelegateParams = null;
        }

        ICollection<PumpStationInfo> getData(PFServiceClient pfServiceClient, object[] objParams, out bool ok)
        {
            var res = pfServiceClient.PumpStationsAsync().Result;
            ok = res.Ok;
            if (!ok)
                return null;

            List<PumpStationInfo> pumpStations = new List<PumpStationInfo>(res.ResultValue.Count);
            foreach(var item in res.ResultValue)
                pumpStations.Add(new PumpStationInfo() { psd = item });

            return pumpStations;
        }

        string getObjectKey(object obj)
        {
            return ((PumpStationInfo)obj).psd.StringId;
        }
    }
}

