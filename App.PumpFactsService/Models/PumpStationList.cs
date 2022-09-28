using System.Collections.Generic;

namespace App.PumpFactsService.Models
{
    /// <summary>
    /// Список станций
    /// </summary>
    public class PumpStationList
    {
        /// <summary>
        /// Словарь id_станции=>объект
        /// </summary>
        private readonly Dictionary<string, PumpStation> pumpStationDictionary = new Dictionary<string, PumpStation>();

        public PumpStationList(PumpStationSchemaFileObjects pumpStationSchemaFileObjects)
        {
            foreach (var station in pumpStationSchemaFileObjects.fsoPumpStations)
            {
                PumpStation pumpStation = new PumpStation(station, pumpStationSchemaFileObjects.getSchemaAllParams(station.schemaName));
                pumpStationDictionary.Add(station.stringId, pumpStation);
            }
        }

        public int Count => pumpStationDictionary.Count;

        /// <summary>
        /// Получить станцию по Id
        /// </summary>
        /// <param name="stringId"></param>
        /// <returns></returns>
        public PumpStation getByStringId(string stringId)
        {
            return pumpStationDictionary[stringId];
        }

        /// <summary>
        /// Получить копию списка станций
        /// </summary>
        /// <returns></returns>
        public List<PumpStation> createList()
        {
            return new List<PumpStation>(pumpStationDictionary.Values);
        }

        /// <summary>
        /// Останавливает мониторы у всех станций
        /// </summary>
        public void setCancelFlag()
        {
            foreach (var pumpStation in pumpStationDictionary.Values)
                pumpStation?.setCancelFlag();
        }

        /// <summary>
        /// Сообщает кол-во работающих мониторов
        /// </summary>
        /// <returns></returns>
        public int getRunningMonitorCount()
        {
            int result = 0;
            foreach (var pumpStation in pumpStationDictionary.Values)
                if (pumpStation.isMonitorRunning())
                    result++;
            return result;
        }
    }
}
