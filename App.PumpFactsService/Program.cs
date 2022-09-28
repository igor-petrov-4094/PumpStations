using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using Ninject;

using App.PumpFactsService.NinjectConfig.Configs;
using Lib.NoDepsUtils;
using App.Profiles;
using Lib.Log.Interface;

namespace App.PumpFactsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                AvoidRunProgramTwice avoidRunProgramTwice = new AvoidRunProgramTwice("8FCF8224-5471-40D2-B35F-2357EBB5AF75");
                if (!avoidRunProgramTwice.canRunApp())
                {
                    Console.WriteLine("Программа уже запущена");
                    return;
                }

                MyApplicationSetup.createMyApplication();

                var host = CreateHostBuilder(args).Build();

                MainNinjectConfiguration.hostApplicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

                host.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в main(): {ex.extension_Message()}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder result = Host.CreateDefaultBuilder(args);

            // очищаем провайдеры логирования, т.е. выключаем стандартное логирование asp.net core через консоль.
            // в принципе потом сюда можно добавить свой логгер
            result.ConfigureLogging(builder => builder.ClearProviders()/*.AddXXXLogger()*/); 

            result.ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                     ILog logger = NinjectConfig.NinjectKernel.kernel.Get<ILog>();
                     MyProfile myProfile = NinjectConfig.NinjectKernel.kernel.Get<MyProfile>();

                     string[] urls = myProfile.getApplicationAddresses();

                     logger.Info("Сервис слушает следующие адреса:");
                     foreach (var url in urls)
                         logger.Info($"  {url}");

                     webBuilder.UseUrls(urls);                 
                 }
            );

            result.UseConsoleLifetime();

            result.ConfigureServices((hostContext, services) =>
            {
#if DEBUG
                services.AddSwaggerGen();
#endif
            });

            return result;
        }
    }
}
