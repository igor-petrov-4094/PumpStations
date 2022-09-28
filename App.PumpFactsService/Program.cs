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
                    Console.WriteLine("��������� ��� ��������");
                    return;
                }

                MyApplicationSetup.createMyApplication();

                var host = CreateHostBuilder(args).Build();

                MainNinjectConfiguration.hostApplicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

                host.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������ � main(): {ex.extension_Message()}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder result = Host.CreateDefaultBuilder(args);

            // ������� ���������� �����������, �.�. ��������� ����������� ����������� asp.net core ����� �������.
            // � �������� ����� ���� ����� �������� ���� ������
            result.ConfigureLogging(builder => builder.ClearProviders()/*.AddXXXLogger()*/); 

            result.ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                     ILog logger = NinjectConfig.NinjectKernel.kernel.Get<ILog>();
                     MyProfile myProfile = NinjectConfig.NinjectKernel.kernel.Get<MyProfile>();

                     string[] urls = myProfile.getApplicationAddresses();

                     logger.Info("������ ������� ��������� ������:");
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
