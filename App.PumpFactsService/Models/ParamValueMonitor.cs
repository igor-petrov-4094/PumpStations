using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Ninject;

using Lib.Hardware.Interface;
using Lib.Log.Interface;
using App.PumpFactsService.NinjectConfig;
using App.Models;

namespace App.PumpFactsService.Models
{
    /// <summary>
    /// Поддерживает актуальные значения параметров насосной станции
    /// </summary>
    public class ParamValueMonitor
    {
        PumpStation pumpStation;
        ISimpleControllerMemory simpleControllerMemory;
        IPacketManager packetManager;
        List<ParameterDescriptor> parameterDescriptorList;

        PacketDescriptor[] packetDescriptors;

        CancellationToken cancellationToken;
        CancellationTokenSource cancellationTokenSource;

        ILog logger;
        Evaluator evaluator;

        public bool running { get; private set; }

        public ParamValueMonitor(
            PumpStation _pumpStation, 
            IPacketManager _packetManager,
            ISimpleControllerMemory _simpleControllerMemory
        )
        {
            running = false;

            logger = NinjectKernel.kernel.Get<ILog>();

            pumpStation = _pumpStation;
            packetManager = _packetManager;
            simpleControllerMemory = _simpleControllerMemory;

            packetDescriptors = packetManager.createPacketDescriptors(pumpStation);
            parameterDescriptorList = pumpStation.createParameterDescriptorList();

            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            evaluator = new Evaluator(
                (expr) => pumpStation.getValueById(expr)               
            );

            Task.Run(taskCycle, cancellationToken);
        }

        private void taskCycle()
        {
            PumpStationDescriptor psd = pumpStation.pumpStationDescriptor;

            string logPrefix = $"[{psd.stringId}] ";

            running = true;
            try
            {
                // пока не запрошена отмена
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        if (keepConnection())
                        {
                            //logger.Info($"{logPrefix}Есть подключение: {psd.ipAddress}:{psd.port}");

                            // проходимся по всем дескрипторам пакетов
                            foreach (var packetDescriptor in packetDescriptors)
                            {
                                try
                                {
                                    // получаем данные пакета 
                                    //logger.Info($"{logPrefix}Получаем данные...");
                                    object obj = packetManager.getMemoryBlockFromController(packetDescriptor, simpleControllerMemory);

                                    // устанавливаем время последней активности
                                    pumpStation.lastAvailableTime = DateTime.Now;

                                    // устанавливаем значения
                                    //logger.Info($"{logPrefix}Делаем разбор данных...");
                                    packetManager.setValuesFromMemoryBlock(obj, packetDescriptor, pumpStation);
                                }
                                catch (Exception ex)
                                {
                                    logger.Error($"{logPrefix}Ошибка получения данных от контроллера", ex, null, LogOptions.DoNotLogStackTraceOnError);
                                }
                            }

                            // устанавливаем вычисляемые параметры
                            try
                            {
                                // проходимся по всем дескрипторам пакетов (ищем вычисляемые)
                                foreach (var parameterDescriptor in parameterDescriptorList)
                                {
                                    if (parameterDescriptor.isComputable)
                                    {
                                        ParameterValue parameterValue = pumpStation.getValueById(parameterDescriptor.parameterStringId);
                                        evaluator.evaluate(
                                            parameterValue,
                                            parameterDescriptor.expression
                                        );
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.Error($"{logPrefix}Ошибка вычисления выражения в {nameof(taskCycle)}", ex);
                            }
                        }
                        else
                        {
                            logger.Warn($"{logPrefix}Нет подключения: {psd.ipAddress}:{psd.port}");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"{logPrefix}Ошибка в {nameof(taskCycle)}", ex);
                    }

                    Thread.Sleep(3000);
                }
            }
            finally
            {
                running = false;
            }
        }

        /// <summary>
        /// Поддерживает открытым соединение с контроллером 
        /// </summary>
        /// <returns></returns>
        private bool keepConnection()
        {
            try
            {
                if (simpleControllerMemory.isRunning())
                    return true;

                if (!simpleControllerMemory.open())
                    return false;

                for (int i = 0; i < 3; i++)
                {
                    Thread.Sleep(1000);
                    if (simpleControllerMemory.isRunning())
                        return true;
                }
            }
            catch(Exception ex)
            {
                logger.Error($"Ошибка в {nameof(keepConnection)}", ex); 
            }
            return false;
        }

        /// <summary>
        /// устанавливает сигнал остановки монитора
        /// </summary>
        public void setCancelFlag()
        {
            cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Сообщает, была ли запрошена остановка
        /// </summary>
        /// <returns></returns>
        public bool isCancelled()
        {
            return cancellationToken.IsCancellationRequested;
        }
    }
}
