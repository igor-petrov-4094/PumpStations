using System;
using System.Collections.Generic;
using System.Text;

using Ninject;

using App.Models;
using App.PumpFactsService.NinjectConfig;
using Lib.Hardware.Interface;
using Lib.Hardware.Impl.SimpleControllerMemory;

namespace App.PumpFactsService.Models
{
    /// <summary>
    /// Одна насосная станция с параметрами и значениями
    /// </summary>
    public class PumpStation
    {
        /// <summary>
        /// Дескриптор насосной станции
        /// </summary>
        public PumpStationDescriptor pumpStationDescriptor { get; private set; }

        /// <summary>
        /// Словарь id-параметра => объект значения параметра
        /// </summary>
        Dictionary<string, ParameterValue> parameterValues;

        /// <summary>
        /// Дескрипторы параметров
        /// </summary>
        List<ParameterDescriptor> parameterDescriptors;

        /// <summary>
        /// Монитор значений 
        /// </summary>
        ParamValueMonitor paramValueMonitor;

        /// <summary>
        /// Время последней доступности станции
        /// </summary>
        public DateTime? lastAvailableTime { get; set; } = null;

        public PumpStation(
            PumpStationDescriptor _pumpStationDescriptor,
            List<ParameterDescriptor> _parameterDescriptors
        )
        {
            pumpStationDescriptor = _pumpStationDescriptor;
            parameterDescriptors = _parameterDescriptors;

            parameterValues = new Dictionary<string, ParameterValue>();
            foreach (var desc in _parameterDescriptors)
            {
                var value = new ParameterValue(desc);
                parameterValues.Add(desc.parameterStringId, value);
            }

            IPacketManager packetManager = NinjectKernel.kernel.Get<IPacketManager>();

            ISimpleControllerMemory simpleControllerMemory;
            if (false)
            {
                simpleControllerMemory = new SimpleControllerMemory_OmronFinsTcpNet(
                    pumpStationDescriptor.ipAddress,
                    pumpStationDescriptor.port,
                    3000
                );
            }
            else
            {
                simpleControllerMemory = new SimpleControllerMemory_Simple(
                    pumpStationDescriptor.ipAddress,
                    pumpStationDescriptor.port,
                    5000,
                    5000
                );
            }


            paramValueMonitor = new ParamValueMonitor(
                this,
                packetManager,
                simpleControllerMemory
            );
        }

        /// <summary>
        /// Создает копию списка дескрипторов параметров
        /// </summary>
        /// <returns></returns>
        public List<ParameterDescriptor> createParameterDescriptorList()
        {
            return new List<ParameterDescriptor>(parameterDescriptors.ToArray());
        }

        /// <summary>
        /// Возвращает список аварий
        /// </summary>
        /// <returns></returns>
        public string getFailuresString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var val in parameterValues.Values)
            {
                if (
                    !string.IsNullOrWhiteSpace(val.parameterDescriptor.failureName)
                    &&
                    val.isBoolean 
                    && 
                    val.boolValue == true
                )
                {
                    sb.Append(val.parameterDescriptor.failureName);
                    sb.Append(", ");
                }
            }
            if (sb.Length > 0) // удаляем последние запятую и пробел
            {
                sb.Remove(sb.Length - 2, 2);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Создает копию списка значений параметров
        /// </summary>
        /// <returns></returns>
        public List<ParameterValue> createValueList()
        {
            return new List<ParameterValue>(parameterValues.Values);
        }

        /// <summary>
        /// Возвращает объект параметра по его идентификатору
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        public ParameterValue getValueById(string parameterId, bool nullAllowed = false)
        {
            if (!nullAllowed)
                return parameterValues[parameterId];
            else
            {
                if (!parameterValues.TryGetValue(parameterId, out ParameterValue result))
                    return null;
                else
                    return result;
            }
        }

        /// <summary>
        /// Останавливает монитор
        /// </summary>
        public void setCancelFlag()
        {
            paramValueMonitor.setCancelFlag();
        }

        /// <summary>
        /// Сообщает, работает ли монитор станции
        /// </summary>
        /// <returns></returns>
        public bool isMonitorRunning()
        {
            return paramValueMonitor.running;
        }
    }
}
