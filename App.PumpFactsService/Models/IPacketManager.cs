using System;
using System.Text;

using App.Models;
using Lib.Hardware.Interface;

namespace App.PumpFactsService.Models
{
    /// <summary>
    /// Менеджер пакетов
    /// </summary>
    public interface IPacketManager
    {
        /// <summary>
        /// Получить массив дескрипторов пакетов
        /// </summary>
        /// <param name="pumpStation"></param>
        /// <returns></returns>
        PacketDescriptor[] createPacketDescriptors(PumpStation pumpStation);


        /// <summary>
        /// Отправить запрос контроллеру по сети, получить объект (массив байт, слов и т.п)
        /// </summary>
        /// param name="packetDescriptor"
        /// param name="simpleControllerMemory"
        object getMemoryBlockFromController(
            PacketDescriptor packetDescriptor, 
            ISimpleControllerMemory simpleControllerMemory
        );

        /// <summary>
        /// Разобрать объект (массив байт, слов) в соответствии с дескриптором пакета и установить значения в PumpStation
        /// </summary>
        /// <param name="obj">объект, полученный в методе getMemoryBlockFromController</param>
        /// <param name="packetDescriptor"></param>
        /// <param name="pumpStation"></param>
        void setValuesFromMemoryBlock(
            object obj, 
            PacketDescriptor packetDescriptor, 
            PumpStation pumpStation
        );
    }
}