using System;

using Lib.Profiles;

namespace App.Profiles
{
    /// <summary>
    /// Расширение Profile: дополнительные методы и поля
    /// </summary>
    public class MyProfile : Profile
    {
        public MyProfile(string _producerName, string _appName, string _profileId = null) : base(_producerName, _appName, _profileId)
        {
        }

        public string getNlogConfigFilename()
        {
            return getFullnameByShortName("nlog.config"); 
        }

        public string getPumpStationParamsDirectory()
        {
            return getFullnameByShortName("PumpStationParams");
        }

        public string[] getApplicationAddresses()
        {          
            return getFileContentsByShortName("pumpservice.address.txt").Split(';', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
