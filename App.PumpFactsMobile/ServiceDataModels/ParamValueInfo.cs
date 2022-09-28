using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App.PumpFactsServiceClient;

namespace App.PumpFactsMobile.ServiceDataModels
{
    /// <summary>
    /// Значение параметра
    /// </summary>
    public class ParamValueInfo
    {
        /// <summary>
        /// Объект из сервиса
        /// </summary>
        public ParameterValue_Client pv { get; set; }

        /// <summary>
        /// Значение с условиями
        /// </summary>
        public string StrValue
        {
            get
            {
                if (!pv.IsInitialized)
                    return "-";
                else
                {
                    if (pv.IsBoolean)
                    {
                        return pv.BoolValue ? "ДА" : "НЕТ";
                    }
                    else
                    {
                        double value = pv.IntValue * pv.RealValueFactor;
                        return value.ToString(String.IsNullOrEmpty(pv.Format) ? "0.0" : pv.Format);
                    }
                }
            }
        }
    }
}
