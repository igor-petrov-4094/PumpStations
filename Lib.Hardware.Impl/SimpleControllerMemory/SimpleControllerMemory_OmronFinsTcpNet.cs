using System;
using System.Threading;

using OmronFinsTCP.Net;

using Lib.NoDepsUtils;
using Lib.Hardware.Interface;

namespace Lib.Hardware.Impl.SimpleControllerMemory
{
    /// <summary>
    /// Доступ к памяти контроллера OMRON
    /// </summary>
    public class SimpleControllerMemory_OmronFinsTcpNet : ISimpleControllerMemory
    {
        EtherNetPLC etherNetPLC;
        string ip;
        short port;
        short timeout;
        bool running = false;

        public int retryCount = 3;

        public SimpleControllerMemory_OmronFinsTcpNet(string _ip, short _port, short _timeout)
        {
            ip = _ip;
            port = _port;
            timeout = _timeout;
        }

        public bool open()
        {
            etherNetPLC = new EtherNetPLC();
            running = (etherNetPLC.Link(ip, port, timeout) == 0);
            return running;
        }

        public void close()
        {
            try
            {
                if (running)
                    etherNetPLC.Close();
            }
            finally
            {
                running = false;
                etherNetPLC = null;
            }
        }

        public bool isRunning()
        {
            return running;
        }

        public bool getBit(AbstractMemoryType memoryType, short memno, short bitno)
        {
            for (int i = 0; i < retryCount; i++)
            {
                if (etherNetPLC.GetBitState(memoryType.extension_ToOmronFinsTCPMemoryType(), memno, bitno, out short bs) != 0)
                {
                    Thread.Sleep(1);
                }
                else
                    return (bs != 0);
            }
            throw new Exception($"getBit memno={memno}, bitno={bitno}");
        }

        public short getWord(AbstractMemoryType memoryType, short memno)
        {
            for (int i = 0; i < retryCount; i++)
            {
                if (etherNetPLC.ReadWord(memoryType.extension_ToOmronFinsTCPMemoryType(), memno, out short reData) != 0)
                {
                    Thread.Sleep(1);
                }
                else
                    return reData;
            }
            throw new Exception($"getWord memno={memno}");
        }

        public short[] getWords(AbstractMemoryType memoryType, short memno, short count)
        {
            for (int i = 0; i < retryCount; i++)
            {
                if (etherNetPLC.ReadWords(memoryType.extension_ToOmronFinsTCPMemoryType(), memno, count, out short[] reData) != 0)
                {
                    Thread.Sleep(1);
                }
                else
                    return reData;
            }
            throw new Exception($"getWords memno={memno}");
        }

        public void setBit(AbstractMemoryType memoryType, short memno, short bitno, bool value)
        {
            for (int i = 0; i < retryCount; i++)
            {
                if (etherNetPLC.SetBitState(memoryType.extension_ToOmronFinsTCPMemoryType(), memno, bitno, value == true ? BitState.ON : BitState.OFF) != 0)
                {
                    Thread.Sleep(1);
                }
                else
                    return;
            }
            throw new Exception($"setBit memno={memno}, bitno={bitno}, value={value}");
        }

        public void setWord(AbstractMemoryType memoryType, short memno, short value)
        {
            for (int i = 0; i < retryCount; i++)
            {
                if (etherNetPLC.WriteWord(memoryType.extension_ToOmronFinsTCPMemoryType(), memno, value) != 0)
                {
                    Thread.Sleep(1);
                }
                else
                    return;
            }
            throw new Exception($"setWord memno={memno}, value={value}");
        }

        public void setDWord(AbstractMemoryType memoryType, short memno, uint value)
        {
            short[] words = { (short)value.extension_Low(), (short)value.extension_High() };

            for (int i = 0; i < retryCount; i++)
            {
                if (etherNetPLC.WriteWords(memoryType.extension_ToOmronFinsTCPMemoryType(), memno, 2, words) != 0)
                {
                    Thread.Sleep(1);
                }
                else
                    return;
            }
            throw new Exception($"setDWord memno={memno}, value={value}");
        }

        public uint getDWord(AbstractMemoryType memoryType, short memno)
        {
            if ((memno & 1) != 0)
                throw new Exception("Адрес должен быть четным");

            for (int i = 0; i < retryCount; i++)
            {
                if (etherNetPLC.ReadWords(memoryType.extension_ToOmronFinsTCPMemoryType(), memno, 2, out short[] reData) != 0)
                {
                    Thread.Sleep(1);
                }
                else
                {
                    uint high = (ushort)reData[1];
                    uint low = (ushort)reData[0];
                    return (high << 16 | low);
                }
            }

            throw new Exception($"getDWord memno={memno}");
        }
    }
}