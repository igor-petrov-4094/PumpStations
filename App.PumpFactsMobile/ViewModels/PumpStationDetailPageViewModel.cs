using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Maui.Controls;

using App.PumpFactsMobile.ServiceDataModels;
using App.PumpFactsMobile.Utils;
using App.PumpFactsServiceClient;

namespace App.PumpFactsMobile.ViewModels 
{
    public class PumpStationDetailPageViewModel : AutoRefreshableCollectionViewModel<ParamValueInfo>
    {
        public PumpStationInfo pumpStationInfo { get; private set; }

        public PumpStationDetailPageViewModel()
        {
            getDataDelegate = getData;
            getObjectKeyDelegate = getObjectKey;
            getDataDelegateParams = null;
        }

        public void setParams(
            ContentPage _contentPage, 
            int _refreshTimeInSeconds, 
            PumpStationInfo _pumpStationInfo
        )
        {
            base.setParams(_contentPage, _refreshTimeInSeconds);

            pumpStationInfo = _pumpStationInfo;
        }

        private string getObjectKey(object obj)
        {
            return ((ParamValueInfo)obj).pv.ParameterStringId;
        }

        ICollection<ParamValueInfo> getData(PFServiceClient pfServiceClient, object[] objects, out bool ok)
        {
            var res = pfServiceClient.StationParamsAsync(pumpStationInfo.psd.StringId).Result;
            ok = res.Ok;
            if (!ok)
                return null;

            // преобразуем List<ParameterValue_Client> в List<ParamValueInfo>
            List<ParamValueInfo> result = new List<ParamValueInfo>(res.ResultValue.Count);
            foreach (var item in res.ResultValue)
            {
                if (!item.IsTech)
                {
                    result.Add(new ParamValueInfo()
                    {
                        pv = item,
                    }
                    );
                }
            }

            return result;
        }
    }
}
