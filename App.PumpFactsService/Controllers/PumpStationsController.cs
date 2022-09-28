using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using Ninject;
using Newtonsoft.Json;

using App.Models;
using App.PumpFactsService.NinjectConfig;
using App.PumpFactsService.Models;

namespace App.PumpFactsService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PumpStationsController : ControllerBase
    {
        private readonly ILogger<PumpStationsController> _logger;

        public PumpStationsController(ILogger<PumpStationsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public DTOWithResultInfo<IEnumerable<PumpStationDescriptor_Client>> Get()
        {
            try
            {
                PumpStationList pumpStationList = NinjectKernel.kernel.Get<PumpStationList>();

                // копируем в краткую форму
                List<PumpStationDescriptor_Client> result = new List<PumpStationDescriptor_Client>(pumpStationList.Count);
                List<PumpStation> tempList = pumpStationList.createList();
                foreach (var pumpStation in tempList)
                {
                    result.Add(
                        new PumpStationDescriptor_Client()
                        {
                            stringId = pumpStation.pumpStationDescriptor.stringId,
                            readableName = pumpStation.pumpStationDescriptor.readableName,
                            lastAvailableTime = pumpStation.lastAvailableTime,
                            failures = pumpStation.getFailuresString(),
                            motorCurrentFactor = pumpStation.pumpStationDescriptor.motorCurrentFactor,
                        }
                    );
                }

                return DTOWithResultInfo<IEnumerable<PumpStationDescriptor_Client>>.createNormal(result);
            }
            catch(Exception ex)
            {
                return DTOWithResultInfo<IEnumerable<PumpStationDescriptor_Client>>.createFromException(ex);
            }
        }
    }
}
