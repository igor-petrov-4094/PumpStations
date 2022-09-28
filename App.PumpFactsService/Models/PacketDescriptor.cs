using System.Collections.Generic;

using App.Models;
using Lib.Hardware.Interface;

namespace App.PumpFactsService.Models
{
    /// <summary>
    /// Дескриптор пакета. В один пакет помещаются данные ячеек, близко или последовательно находящиеся в памяти
    /// </summary>
    public class PacketDescriptor
    {
        /// <summary>
        /// Тип памяти
        /// </summary>
        public AbstractMemoryType? memory { get; set; }

        /// <summary>
        /// Начальный адрес
        /// </summary>
        public int startAddress { get; set; }

        /// <summary>
        /// Конечный адрес
        /// </summary>
        public int endAddress { get; set; }

        /// <summary>
        /// Неотсортированный список параметров, данные которых содержатся в пакете
        /// </summary>
        public List<ParameterDescriptor> unsortedParameterList { get; private set; }

        public PacketDescriptor()
        {
            unsortedParameterList = new List<ParameterDescriptor>();
        }

        /// <summary>
        /// Расширяет начальный или конечный адрес
        /// </summary>
        /// <param name="_address"></param>
        public void extendAddresses(int _address)
        {
            if (_address < startAddress)
                startAddress = _address;

            if (_address > endAddress)
                endAddress = _address;
        }

        public bool isValid(out string message)
        {
            if (memory == null)
            {
                message = "Не установлен тип памяти";
                return false;
            }

            if (endAddress < startAddress)
            {
                message = "endAddress < startAddress";
                return false;
            }

            message = "OK";
            return true;
        }

        /// <summary>
        /// Возвращает длину пакета по формуле (end-start)+1
        /// </summary>
        /// <returns></returns>
        public int getPacketLengthInWords()
        {
            return endAddress - startAddress + 1;
        }

        public string debug
        {
            get
            {
                string res = "";
                res += $"Mem: {this.memory}, Adr: {this.startAddress}---{this.endAddress}, Params: ";
                foreach(var param in unsortedParameterList)
                {
                    res += param.parameterStringId + ",";
                }
                return res;
            }
        }
    }
}