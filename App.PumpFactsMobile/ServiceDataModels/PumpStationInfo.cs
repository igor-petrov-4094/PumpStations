using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App.PumpFactsServiceClient;

namespace App.PumpFactsMobile.ServiceDataModels
{
    /// <summary>
    /// Мера времени недоступности станции
    /// </summary>
    internal enum PumpStationUnavailibilityTimeGrade
    {
        // члены являются индексами массива!
        Normal_30SecondAndBelow = 0,
        Unknown = 1,
        Warn_LessThan1Minute = 2,
        Warn_LessThan2Minute = 3,
        Warn_LessThan3Minute = 4,
        Error_3MinuteAndMore = 5,
    }

    /// <summary>
    /// Информация об станции
    /// </summary>
    public class PumpStationInfo
    {
        /// <summary>
        /// Объект из сервиса
        /// </summary>
        public PumpStationDescriptor_Client psd { get; set; }

        /// <summary>
        /// Цвета для PumpStationUnavailibilityTimeGrade
        /// </summary>
        private static Color[] gradeColors = new Color[]
        {
            Colors.Green,
            Colors.Gray,
            Colors.Yellow,
            Colors.Orange,
            Colors.Brown,
            Colors.Red
        };

        /// <summary>
        /// Мера времени недоступности станции
        /// </summary>
        private PumpStationUnavailibilityTimeGrade PumpStationUnavailibilityTimeGrade
        {
            get
            {
                if (psd.LastAvailableTime == null)
                    return PumpStationUnavailibilityTimeGrade.Unknown;

                DateTime? lastAvailableTime = LastAvailableTime_DateTime;

                double differenceInSeconds = (DateTime.Now - (DateTime)lastAvailableTime).TotalSeconds;

                if (differenceInSeconds < 30)
                    return PumpStationUnavailibilityTimeGrade.Normal_30SecondAndBelow;

                if (differenceInSeconds < 60 * 1)
                    return PumpStationUnavailibilityTimeGrade.Warn_LessThan1Minute;

                if (differenceInSeconds < 60 * 2)
                    return PumpStationUnavailibilityTimeGrade.Warn_LessThan2Minute;

                if (differenceInSeconds < 60 * 3)
                    return PumpStationUnavailibilityTimeGrade.Warn_LessThan3Minute;

                return PumpStationUnavailibilityTimeGrade.Error_3MinuteAndMore;
            }
        }

        /// <summary>
        /// Время последней доступности (как время)
        /// </summary>
        private DateTime? LastAvailableTime_DateTime
        {
            get
            {
                if (psd.LastAvailableTime == null)
                    return null;
                else
                    return ((DateTimeOffset)psd.LastAvailableTime).ToLocalTime().DateTime;
            }
        }

        /// <summary>
        /// Время последней доступности (как строка). 
        /// </summary>
        public string LastAvailableTime_String
        {
            get
            {
                if (PumpStationUnavailibilityTimeGrade == PumpStationUnavailibilityTimeGrade.Normal_30SecondAndBelow)
                    return "";

                DateTime? _lastAvailableTime_DateTime = LastAvailableTime_DateTime;
                if (_lastAvailableTime_DateTime == null)
                    return "?";
                else
                {
                    var d = (DateTime)_lastAvailableTime_DateTime;
                    if (d.Date == DateTime.Now.Date)
                        return d.ToString("HH:mm:ss");
                    else
                        return d.ToString("yyyy.MM.dd HH:mm:ss");
                }
            }
        }

        public bool ShowLastAvailableTime
        {
            get
            {
                return !string.IsNullOrWhiteSpace(LastAvailableTime_String);
            }
        }

        /// <summary>
        /// Цвет меры времени недоступности станции
        /// </summary>
        public Color PumpStationUnavailibilityTimeColor
        {
            get
            {
                int index = (int)PumpStationUnavailibilityTimeGrade;
                if (index < 0 || index >= gradeColors.Length)
                    return Colors.Gray;
                else
                    return gradeColors[index];
            }
        }

        /// <summary>
        /// Признак что список аварий не пуст
        /// </summary>
        public bool FailuresNotEmpty
        {
            get
            {
                return !string.IsNullOrWhiteSpace(psd.Failures);
            }
        }
    }
}
