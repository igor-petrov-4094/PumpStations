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
    public class StationParamsController : ControllerBase
    {
        public StationParamsController()
        {
        }

        [HttpGet]
        public DTOWithResultInfo<IEnumerable<ParameterValue_Client>> Get(string stationName)
        {
            try
            {
                PumpStationList pumpStationList = NinjectKernel.kernel.Get<PumpStationList>();

                PumpStation pumpStation = pumpStationList.getByStringId(stationName);

                var paramValues = pumpStation.createValueList();

                var result = new List<ParameterValue_Client>();
                foreach(var paramValue in paramValues)
                {
                    // если параметр - ток мотора, его нужно еще умножить на коэффициент тока мотора станции (который у разных станций разный)
                    bool isMotorCurrentParameter = (paramValue.parameterDescriptor.parameterStringId == "pch.motor_current");
                    float factor = paramValue.parameterDescriptor.realValueFactor;
                    if (isMotorCurrentParameter)
                        factor = factor * pumpStation.pumpStationDescriptor.motorCurrentFactor;

                    result.Add(
                        new ParameterValue_Client()
                        {
                            // вычисленный
                            realValueFactor = factor,

                            // из дескриптора
                            format = paramValue.parameterDescriptor.format,
                            isTech = paramValue.parameterDescriptor.isTech,
                            parameterStringId = paramValue.parameterDescriptor.parameterStringId,
                            readableName = paramValue.parameterDescriptor.readableName,

                            // из значения
                            isInitialized = paramValue.isInitialized,
                            intValue = paramValue.intValue,
                            isBoolean = paramValue.isBoolean,
                            boolValue = paramValue.boolValue,
                        }
                    ); ;
                }

                return DTOWithResultInfo<IEnumerable<ParameterValue_Client>>.createNormal(
                    result
                );
            }
            catch(Exception ex)
            {
                return DTOWithResultInfo<IEnumerable<ParameterValue_Client>>.createFromException(ex);
            }
        }
    }
}
