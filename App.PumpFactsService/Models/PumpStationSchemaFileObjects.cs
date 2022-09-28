using System;
using System.Collections.Generic;
using System.IO;
using App.Models;
using Lib.Log.Interface;

namespace App.PumpFactsService.Models
{
    /// <summary>
    /// Файловые объекты схем насосных станций
    /// </summary>
    public class PumpStationSchemaFileObjects
    {
        /// <summary>
        /// Каталог, где лежат json-файлы дескрипторов параметров, групп, схем и т.п.
        /// </summary>
        private readonly string baseDirectory;

        public readonly PumpStationDescriptor[] fsoPumpStations;
        public readonly ParameterDescriptorGroupDescriptor[] fsoGroups;
        public readonly ParameterDescriptorSchema[] fsoSchemas;
        public readonly ParameterDescriptor[] fsoParams;

        public PumpStationSchemaFileObjects(
            string _baseDirectory,
            ILog _logger
        )
        {
            baseDirectory = _baseDirectory;
            fsoPumpStations = loadArrayFromJsonFile<PumpStationDescriptor[]>($"{baseDirectory}\\stations.json");
            fsoGroups = loadArrayFromJsonFile<ParameterDescriptorGroupDescriptor[]>($"{baseDirectory}\\groups.json");
            fsoSchemas = loadArrayFromJsonFile<ParameterDescriptorSchema[]>($"{baseDirectory}\\schemas.json");
            fsoParams = loadArrayFromJsonFile<ParameterDescriptor[]>($"{baseDirectory}\\params.json");
        }

        /// <summary>
        /// Загрузить объект типа TDataType из json-файла
        /// </summary>
        /// <typeparam name="TDataType"></typeparam>
        /// <param name="jsonFileName"></param>
        /// <returns></returns>
        private TDataType loadArrayFromJsonFile<TDataType>(string jsonFileName)
        {
            using (TextReader textReader = new StreamReader(jsonFileName))
            {
                using (Newtonsoft.Json.JsonReader reader = new Newtonsoft.Json.JsonTextReader(textReader))
                {
                    Newtonsoft.Json.JsonSerializer jsonSerializer = new Newtonsoft.Json.JsonSerializer();
                    return jsonSerializer.Deserialize<TDataType>(reader);
                }
            }
        }

        /// <summary>
        /// Возвращает схему по имени
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="doThrowException"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ParameterDescriptorSchema getSchemaByName(string schemaName, bool doThrowException)
        {
            string schemaName_capitalized = schemaName.ToUpper();
            var schemas = fsoSchemas;
            foreach (var schema in schemas)
            {
                if (schema.name.ToUpper() == schemaName_capitalized)
                    return schema;
            }
            if (doThrowException)
                throw new Exception($"Схема не найдена: {schemaName}");
            else
                return null;
        }

        /// <summary>
        /// Возвращает группу по id
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="doThrowException"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ParameterDescriptorGroupDescriptor getGroupById(string groupId, bool doThrowException)
        {
            var groups = fsoGroups;
            foreach (var group in groups)
            {
                if (group.groupStringId == groupId)
                    return group;
            }
            if (doThrowException)
                throw new Exception($"Группа не найдена по id: {groupId}");
            else
                return null;
        }

        /// <summary>
        /// Возвращает параметр по id
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="doThrowException"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ParameterDescriptor getParameterById(string parameterId, bool doThrowException)
        {
            var parameters = fsoParams;
            foreach (var parameter in parameters)
            {
                if (parameter.parameterStringId == parameterId)
                    return parameter;
            }
            if (doThrowException)
                throw new Exception($"Параметр не найден по id: {parameterId}");
            else
                return null;
        }

        /// <summary>
        /// Возвращает все параметры указанной схемы
        /// </summary>
        /// <returns></returns>
        public List<ParameterDescriptor> getSchemaAllParams(string schemaName)
        {
            Dictionary<string, ParameterDescriptor> result = new Dictionary<string, ParameterDescriptor>();

            var schema = getSchemaByName(schemaName, true);

            // проходим по всем группам в схеме
            foreach (var groupId in schema.groupStringIdList)
            {
                ParameterDescriptorGroupDescriptor group = getGroupById(groupId, true);
                // проходим по всем параметрам в группе
                foreach (var parameterId in group.parameterStringIdList)
                {
                    // избегаем повторения одного и того же параметра в списке
                    if (!result.ContainsKey(parameterId))
                    {
                        ParameterDescriptor parameterDescriptor = getParameterById(parameterId, true);
                        parameterDescriptor.groupName_runTime = group.readableName;
                        result.Add(parameterId, parameterDescriptor);
                    }
                }
            }

            return new List<ParameterDescriptor>(result.Values);
        }
    }
}
