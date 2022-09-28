using Ninject.Modules;

using App.Profiles;
using Lib.Log.Interface;
using App.PumpFactsService.Models;
using Lib.Hardware.Interface;
using Lib.NoDepsUtils;
using Microsoft.Extensions.Hosting;

namespace App.PumpFactsService.NinjectConfig.Configs
{
    /// <summary>
    /// Класс конфигурации Ninject
    /// </summary>
    public class MainNinjectConfiguration : NinjectModule
    {
        public static MyProfile myProfile;
        public static ILog logger;
        public static PumpStationList pumpStationList;
        public static IPacketManager controllerMemoryManager;
        public static ConsoleUtils consoleUtils;
        public static IHostApplicationLifetime hostApplicationLifetime;

        public override void Load()
        {
            Bind<MyProfile>().ToMethod((context) => { return myProfile; });
            Bind<ILog>().ToMethod((context) => { return logger; });
            Bind<PumpStationList>().ToMethod((context) => { return pumpStationList; });
            Bind<IPacketManager>().ToMethod((context) => { return controllerMemoryManager; });
            Bind<ConsoleUtils>().ToMethod((context) => { return consoleUtils; });
            Bind<IHostApplicationLifetime>().ToMethod((context) => { return hostApplicationLifetime; });
        }
    }
}
