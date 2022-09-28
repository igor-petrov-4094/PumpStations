using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject;

using App.PumpFactsService.NinjectConfig.Configs;

namespace App.PumpFactsService.NinjectConfig
{       
    /// <summary>
    /// Класс для создания объектов по интерфейсу
    /// </summary>
    public class NinjectKernel
    {
        public static IKernel kernel = new StandardKernel(new MainNinjectConfiguration());
    }
}
