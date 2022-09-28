using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using App.PumpFactsMobile.Utils;
using App.PumpFactsServiceClient;

namespace App.PumpFactsMobile.ViewModels
{
    /// <summary>
    /// Базовый класс модели для получения данных от сервиса PFServiceClient, с автообновлением коллекции 
    /// </summary>
    /// <typeparam name="DataType"></typeparam>
    public class AutoRefreshableCollectionViewModel<DataType> : INotifyPropertyChanged
    {
        private HttpClient httpClient;
        private PFServiceClient pfServiceClient;
        private int refrehshTimeInSeconds;
        private Timer __timer;

        #region Делегаты
        public object[] getDataDelegateParams;
        public delegate ICollection<DataType> GetDataDelegate(PFServiceClient _pfServiceClient, object[] _getDataDelegateParams, out bool _ok);
        public GetDataDelegate getDataDelegate { get; set; }

        public delegate string GetObjectKeyDelegate(object obj);
        public GetObjectKeyDelegate getObjectKeyDelegate { get; set; }
        #endregion

        #region IsRefreshing и IsRefreshFailed 
        bool _isRefreshing = false;
        bool _isRefreshFailed = false;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public bool IsRefreshFailed
        {
            get => _isRefreshFailed;
            set
            {
                _isRefreshFailed = value;
                OnPropertyChanged(nameof(IsRefreshFailed));
            }
        }
        #endregion

        public ObservableCollection<DataType> Items { get; private set; }
        Dictionary<string, int> stationIndexes;

        #region Конструктор/деструктор
        public AutoRefreshableCollectionViewModel()
        {
            stationIndexes = new Dictionary<string, int>();
            Items = new ObservableCollection<DataType>();

            httpClient = new HttpClient();
            pfServiceClient = new PFServiceClient(Singleton.getServiceAddress(), httpClient);
        }

        public void setParams(ContentPage _contentPage, int _refreshTimeInSeconds)
        {
            refrehshTimeInSeconds = _refreshTimeInSeconds;

            _contentPage.Disappearing += _contentPage_Disappearing;
            _contentPage.Appearing += _contentPage_Appearing;
        }

        private void _contentPage_Appearing(object sender, EventArgs e)
        {
            startTimer();
        }

        private void _contentPage_Disappearing(object sender, EventArgs e)
        {
            stopTimer();
        }

        ~AutoRefreshableCollectionViewModel()
        {
            __timer?.Dispose();
            __timer = null;
        }
        #endregion

        void clearData()
        {
            lock (Items)
            {
                lock (stationIndexes)
                {
                    Items.Clear();
                    stationIndexes.Clear();
                }
            }
        }

        void GetData()
        {
            try
            {
                if (getDataDelegate == null || getObjectKeyDelegate == null)
                    return;

                IsRefreshFailed = false;
                try
                {
                    IsRefreshing = true;
                    try
                    {
                        ICollection<DataType> data = getDataDelegate(pfServiceClient, getDataDelegateParams, out bool ok);
                        if (!ok)
                        {
                            IsRefreshFailed = true;
                            return;
                        }

                        try
                        {
                            foreach (var item in data)
                            {
                                string key = getObjectKeyDelegate(item);
                                if (stationIndexes.TryGetValue(key, out int index))
                                    Items[index] = item;
                                else
                                {
                                    lock (Items)
                                    {
                                        lock (stationIndexes)
                                        {
                                            int newIndex = Items.Count;
                                            Items.Add(item);
                                            stationIndexes.Add(key, newIndex);
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            OnPropertyChanged(nameof(Items));
                        }
                    }
                    finally
                    {
                        IsRefreshing = false;
                    }
                }
                catch
                {
                    IsRefreshFailed = true;
                }
            }
            finally
            {
                // программируем срабатывание таймера через refrehshTimeInSeconds секунд
                __timer.Change(
                    TimeSpan.FromSeconds(refrehshTimeInSeconds),
                    Timeout.InfiniteTimeSpan
                );
            }
        }

        protected bool getItemByKey(string key, out DataType result)
        {
            if (stationIndexes.TryGetValue(key, out int index))
            {
                result = Items[index];
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public void startTimer()
        {
            stopTimer();

            // создаем таймер, который сразу же срабатывает
            __timer = new Timer(
                new TimerCallback((s) => Task.Run(() => GetData())), 
                null, 
                TimeSpan.Zero, 
                Timeout.InfiniteTimeSpan
            );
        }

        public void stopTimer()
        {
            __timer?.Change(Timeout.Infinite, Timeout.Infinite);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
