using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Abstractions;

using Ninject;

using Lib.Hardware.Interface;
using Lib.Log.Interface;
using Lib.EHandling.Interface;
using App.Models;
using App.PumpFactsService.NinjectConfig;

namespace App.PumpFactsService.Models
{
    /// <summary>
    /// Менеджер пакетов для памяти контроллера OMRON
    /// </summary>
    public class PacketManager_Omron : IPacketManager
    {
        ILog logger = null;

        public PacketDescriptor[] createPacketDescriptors(PumpStation pumpStation)
        {
            // номер пакета => PacketDescriptor
            Dictionary<int, PacketDescriptor> result = new Dictionary<int, PacketDescriptor>();
            
            logger = logger ?? NinjectKernel.kernel.Get<ILog>();

            var pdl = pumpStation.createParameterDescriptorList();
            foreach (var parameterDescriptor in pdl)
            {
                if (parameterDescriptor.isComputable)
                    continue;

                if (parameterDescriptor.packetNo == null)
                {  
                    logger.Warn($"Номер пакета не установлен, параметр не будет получен в пакете: {parameterDescriptor.parameterStringId}", EventCodeDesc_PumpFactsService.NoPacketNumberValueError);
                    continue;
                }

                if (!parseAddress(parameterDescriptor.cellAddress, out AbstractMemoryType memoryType, 
                    out int address, out int bitno, out bool bitNoFieldSet))
                {
                    logger.Error($"Ошибка разбора адреса ячейки, параметр пропущен: {parameterDescriptor.parameterStringId}", EventCodeDesc_PumpFactsService.CellAddressParseError);
                    continue;
                }

                PacketDescriptor packetDescriptor;
                if (result.TryGetValue((int)parameterDescriptor.packetNo, out packetDescriptor))
                {
                    // если память другая, ошибка
                    if (memoryType != packetDescriptor.memory)
                    {
                        logger.Error("Тип памяти пакета не соотв. типу памяти параметра", EventCodeDesc_PumpFactsService.InvalidParameterDescriptor);
                        continue;
                    }

                    // изменяем startAddress или endAddress
                    packetDescriptor.extendAddresses(address);

                    // добавляем дескриптор
                    packetDescriptor.unsortedParameterList.Add(parameterDescriptor);
                }
                else
                {
                    packetDescriptor = new PacketDescriptor()
                    {
                        memory = memoryType,
                        startAddress = address,
                        endAddress = address,
                    };
                    packetDescriptor.unsortedParameterList.Add(parameterDescriptor);

                    result.Add((int)parameterDescriptor.packetNo, packetDescriptor);
                }
            }

            var packetDescriptors = (new List<PacketDescriptor>(result.Values)).ToArray();

            // логирование
            foreach (var pd in packetDescriptors)
            {
                //logger.Info(pd.debug);
            }

            return packetDescriptors;
        }

        /// <summary>
        /// Разбираем адрес (W100, W100.1, 100.1, D100 и т.п.)
        /// </summary>
        /// <param name="textAddress"></param>
        /// <param name="memoryType"></param>
        /// <param name="address"></param>
        /// <param name="bitNo"></param>
        /// <returns></returns>
        private static bool parseAddress(           
            string textAddress,
            out AbstractMemoryType memoryType, 
            out int address, 
            out int bitNo,
            out bool bitNoFieldSet
        )
        {
            memoryType = AbstractMemoryType.D;
            address = 0;
            bitNo = 0;
            bitNoFieldSet = false;

            if (string.IsNullOrEmpty(textAddress))
                return false;

            // проверяем, есть ли поле бита
            string[] splitted = textAddress.Split('.');
            if (splitted.Length > 2)
                return false;
            if (splitted.Length == 2)
            {
                if (!int.TryParse(splitted[1], out bitNo))
                    return false;
                else
                    bitNoFieldSet = true;
            }

            string addrWithOptionalLetter = splitted[0];
            string numericAddress = addrWithOptionalLetter;

            if (char.IsLetter(addrWithOptionalLetter[0]))
            {
                if (textAddress[0] == 'D' || textAddress[0] == 'd')
                    memoryType = AbstractMemoryType.D;
                else
                if (textAddress[0] == 'W' || textAddress[0] == 'w')
                    memoryType = AbstractMemoryType.W;
                else
                    return false;

                numericAddress = addrWithOptionalLetter.Substring(1);
            }
            else
            {
                memoryType = AbstractMemoryType.CIO;
            }

            return int.TryParse(numericAddress, out address);
        }

        public object getMemoryBlockFromController(
            PacketDescriptor packetDescriptor,
            ISimpleControllerMemory simpleControllerMemory
        )
        {
            if (!packetDescriptor.isValid(out string message))
                throw new Exception($"Неверный дескриптор пакета: <{message}>");

            logger = logger ?? NinjectKernel.kernel.Get<ILog>();
            //logger.Info($"MEM: {packetDescriptor.memory}, ADR: {packetDescriptor.startAddress}, LEN: {packetDescriptor.getPacketLengthInWords()}");

            short[] result = simpleControllerMemory.getWords(
                (AbstractMemoryType) packetDescriptor.memory,
                (short)packetDescriptor.startAddress,
                (short)packetDescriptor.getPacketLengthInWords()
            );
            return result;
        }

        public void setValuesFromMemoryBlock(
            object obj, 
            PacketDescriptor packetDescriptor, 
            PumpStation pumpStation
        )
        {
            short[] memory = (short[])obj; // в таком виде данные отдает getMemoryBlockFromController

            // проходимся по дескрипторам параметров
            foreach (var parameterDescriptor in packetDescriptor.unsortedParameterList)
            {
                // вычисляемые здесь не устанавливаем
                if (parameterDescriptor.isComputable)
                    continue;

                if (!parseAddress(parameterDescriptor.cellAddress, out AbstractMemoryType memoryType, 
                    out int address, out int bitno, out bool bitNoFieldSet))
                    throw new EventCodeException("Ошибка разбора адреса ячейки", EventCodeDesc_PumpFactsService.InvalidParameterDescriptor);
                else
                {
                    ParameterValue parameterValue = pumpStation.getValueById(parameterDescriptor.parameterStringId);
                    parameterValue.isBoolean = bitNoFieldSet;
                    short shortData = memory[address - packetDescriptor.startAddress];
                    if (bitNoFieldSet)
                    {
                        byte byteData = (byte)(shortData >> bitno);
                        byteData = (byte)(byteData & 1);
                        bool bitValue = byteData == 1 ? true : false;
                        parameterValue.boolValue = bitValue; // установили битовое значение
                    }
                    else
                    {
                        parameterValue.intValue = shortData; // установили 16-битное значение
                    }
                }
            }
        }
    }
}
