using Microsoft.Extensions.Hosting;
using System.Threading;

using Ninject;

using App.Profiles;
using App.PumpFactsService.Models;
using App.PumpFactsService.NinjectConfig.Configs;
using App.PumpFactsService.NinjectConfig;
using Lib.Log.Impl;
using Lib.Log.Interface;
using Lib.NoDepsUtils;
using Lib.Profiles;

namespace App.PumpFactsService
{
    public static class MyApplicationSetup
    {
        static string producerName = "";
        static string appName = "";

        static void onControlC()
        {
            ILog logger = NinjectKernel.kernel.Get<ILog>();
            logger.Warn("<Нажатие Control-C>");

            PumpStationList pumpStationList = NinjectKernel.kernel.Get<PumpStationList>();
            logger.Warn("Устанавливаем флаг остановки...");
            pumpStationList.setCancelFlag();

            while (true)
            {
                int runningMonitorCount = pumpStationList.getRunningMonitorCount();
                if (runningMonitorCount == 0)
                    break;
                logger.Info($"Остановка... Работает мониторов: {runningMonitorCount}");
                Thread.Sleep(1000);
            }

            logger.Info("Все мониторы остановлены.");

            IHostApplicationLifetime hostApplicationLifetime = NinjectKernel.kernel.Get<IHostApplicationLifetime>();
            logger.Warn("Остановка приложения...");
            hostApplicationLifetime?.StopApplication();
        }

        static void setupNames()
        {
            producerName = "UTM";
            appName = "PumpFacts";
        }

        static void setupProfile()
        {
            // ------------------------------------------------------------------
            // создаем профиль и устанавливаем в Ninject
            MyProfile myProfile = new MyProfile(producerName, appName);

            MainNinjectConfiguration.myProfile = myProfile; // назначаем профиль
        }

        static void setupLoggers()
        {
            MyProfile myProfile = NinjectKernel.kernel.Get<MyProfile>();

            // ------------------------------------------------------------------
            // создаем логгеры и устанавливаем в Ninject
            string nlogConfig = myProfile.getNlogConfigFilename();

            ILog nlogBasedLogger = new Log_NlogBased(producerName, appName, myProfile.profileId, "", nlogConfig);

            ILog log_Console = new Log_Console();

            ILog log_MultiLog = new Log_MultiLog(nlogBasedLogger, log_Console);

            ILog logger = log_MultiLog;

            MainNinjectConfiguration.logger = logger; // назначаем логгер
        }

        static void setupPacketManager()
        {
            // ------------------------------------------------------------------
            // создаем IPacketManager
            IPacketManager controllerMemoryManager = new PacketManager_Omron();
            MainNinjectConfiguration.controllerMemoryManager = controllerMemoryManager;
        }

        static void setupPumpStationList()
        {
            MyProfile myProfile = NinjectKernel.kernel.Get<MyProfile>();
            ILog logger = NinjectKernel.kernel.Get<ILog>();

            // ------------------------------------------------------------------
            // создаем PumpStationList и устанавливаем в Ninject
            PumpStationSchemaFileObjects pumpStationSchemaFileObjects = new PumpStationSchemaFileObjects(
                myProfile.getPumpStationParamsDirectory(),
                logger
            );
            PumpStationList pumpStationList = new PumpStationList(pumpStationSchemaFileObjects);

            MainNinjectConfiguration.pumpStationList = pumpStationList; // назначаем список станций
        }

        static void setupConsole()
        {
            // настраиваем консоль
            ConsoleUtils.disableCloseButtonOnConsoleWindow();

            ConsoleUtils consoleUtils = new ConsoleUtils();
            consoleUtils.setCancelEvent(onControlC);

            MainNinjectConfiguration.consoleUtils = consoleUtils;

            ILog logger = NinjectKernel.kernel.Get<ILog>();
            logger.Info("Чтобы остановить сервис, нажмите Ctrl+C");
            //Thread.Sleep(3000);
        }

        public static void createMyApplication()
        {
            setupNames();
            setupProfile();
            setupLoggers();
            setupPacketManager();
            setupPumpStationList();
            setupConsole();
        }
    }
}
