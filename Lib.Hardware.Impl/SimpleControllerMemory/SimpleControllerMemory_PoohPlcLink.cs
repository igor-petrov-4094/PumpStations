using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if DEBUG
using PoohPlcLink;

using Lib.Hardware.Interface;

namespace Lib.Hardware.Impl.SimpleControllerMemory
{
    public class SimpleControllerMemory_PoohPlcLink : ISimpleControllerMemory
    {
        PoohFinsETN poohFinsETN;
        bool running = false;

        string PLC_IPAddress;
        short PLC_UDPPort;
        short PLC_NetNo;
        short PLC_NodeNo;
        short PC_NetNo;
        short PC_NodeNo;

        short TimeOutMSec;

        public bool doNotTestBitOnOpen = false;

        public SimpleControllerMemory_PoohPlcLink(
            string _PLC_IPAddress,
            short _PLC_UDPPort,
            short _PLC_NetNo,
            short _PLC_NodeNo,
            short _PC_NetNo,
            short _PC_NodeNo,
            short _TimeOutMSec
          
        )
        {
            PLC_IPAddress = _PLC_IPAddress;
            PLC_UDPPort = _PLC_UDPPort;
            PLC_NetNo = _PLC_NetNo;
            PLC_NodeNo = _PLC_NodeNo;
            PC_NetNo = _PC_NetNo;
            PC_NodeNo = _PC_NodeNo;
            TimeOutMSec = _TimeOutMSec;
        }

        public bool open()
        {
            poohFinsETN = new PoohFinsETN();

            poohFinsETN.PLC_IPAddress = PLC_IPAddress;
            poohFinsETN.PLC_UDPPort = (ushort)PLC_UDPPort;
            poohFinsETN.PLC_NetNo = (byte)PLC_NetNo;
            poohFinsETN.PLC_NodeNo = (byte)PLC_NodeNo;
            poohFinsETN.PC_NetNo = (byte)PC_NetNo;
            poohFinsETN.PC_NodeNo = (byte)PC_NodeNo;
            poohFinsETN.TimeOutMSec = TimeOutMSec;
            try
            {
                if (doNotTestBitOnOpen == false)
                    getBit(AbstractMemoryType.CIO, 300, 0);
                running = true;
                return true;
            }
            catch
            {
                running = false;
                return false;
            }
        }

        public void close()
        {
            if (poohFinsETN == null)
                return;

            try
            {
                running = false;
                poohFinsETN.PLC_IPAddress = "0.0.0.0";
                poohFinsETN.PLC_UDPPort = 0;
                poohFinsETN.PLC_NetNo = 0;
                poohFinsETN.PLC_NodeNo = 0;
                poohFinsETN.PC_NetNo = 0;
                poohFinsETN.PC_NodeNo = 0;
                poohFinsETN.TimeOutMSec = 1;
                poohFinsETN.Dispose();
            }
            catch
            {

            }
            poohFinsETN = null;
        }

        public bool isRunning()
        {
            return running;
        }

        public void setWord(AbstractMemoryType memoryType, short memno, short value)
        {
            poohFinsETN.WriteMemoryWord(memoryType.extension_ToPoohlinkMemoryType(), memno, value, PoohFinsETN.DataTypes.SignBIN);
        }

        public short getWord(AbstractMemoryType memoryType, short memno)
        {
            return (short)poohFinsETN.ReadMemoryWord(memoryType.extension_ToPoohlinkMemoryType(), memno, PoohFinsETN.DataTypes.SignBIN);
        }

        public short[] getWords(AbstractMemoryType memoryType, short memno, short count)
        {
            int[] ints = poohFinsETN.ReadMemoryWord(memoryType.extension_ToPoohlinkMemoryType(), memno, count, PoohFinsETN.DataTypes.SignBIN);

            // преобразуем int[] в short[]
            short[] result = new short[ints.Length];
            for(int i=0; i<ints.Length;i++)
                result[i] = (short)ints[i];

            return result;
        }

        public void setDWord(AbstractMemoryType memoryType, short memno, uint value)
        {
            poohFinsETN.WriteMemoryDWord(memoryType.extension_ToPoohlinkMemoryType(), memno, value, PoohFinsETN.DataTypes.SignBIN);
        }

        public uint getDWord(AbstractMemoryType memoryType, short memno)
        {
            return (uint)poohFinsETN.ReadMemoryWord(memoryType.extension_ToPoohlinkMemoryType(), memno, PoohFinsETN.DataTypes.UnSignBIN);
        }

        public void setBit(AbstractMemoryType memoryType, short memno, short bitno, bool value)
        {
            poohFinsETN.WriteMemoryBit(memoryType.extension_ToPoohlinkMemoryType(), memno, bitno, value);
        }

        public bool getBit(AbstractMemoryType memoryType, short memno, short bitno)
        {
            return poohFinsETN.ReadMemoryBit(memoryType.extension_ToPoohlinkMemoryType(), memno, bitno);
        }
    }
}
#endif