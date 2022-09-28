using System;
using System.Collections.Generic;
using System.Text;

using OmronControllerUdpService;

using Lib.Hardware.Interface;
using Lib.Log.Interface;

namespace Lib.Hardware.Impl.SimpleControllerMemory
{
    public class SimpleControllerMemory_Simple : ISimpleControllerMemory
    {
        Omron omron;

        public SimpleControllerMemory_Simple(string ipAddress, short port, int receiveTimeout, int sendTimeout)
        {
            omron = new Omron(ipAddress, port, receiveTimeout, sendTimeout);
        }

        public void setDebugLogger(ILog debugLogger)
        {
            omron.setDebugLogger(debugLogger);
        }

        public bool isRunning()
        {
            return true;
        }

        public bool open()
        {
            return true;
        }

        public void close()
        {
        }

        public bool getBit(Interface.AbstractMemoryType memoryType, short memno, short bitno)
        {
            SendResultTyped<bool> result = omron.readBit(
                MemoryTypeConverter.extension_ToOmronSimpleMemoryType(memoryType),
                memno,
                (byte)bitno
            );
            result.throwExceptionIfError();
            return result.value;
        }

        public short getWord(Interface.AbstractMemoryType memoryType, short memno)
        {
            SendResultTyped<ushort> result = omron.readWord(
                memoryType.extension_ToOmronSimpleMemoryType(),
                memno
            );
            result.throwExceptionIfError();
            return (short)result.value;
        }
        
        public uint getDWord(Interface.AbstractMemoryType memoryType, short memno)
        {
            SendResultTyped<uint> result = omron.readDWord(
                memoryType.extension_ToOmronSimpleMemoryType(),
                memno
            );
            result.throwExceptionIfError();
            return (uint)result.value;
        }

        public short[] getWords(Interface.AbstractMemoryType memoryType, short memno, short count)
        {
            SendResultTyped<short[]> sendResult = omron.readWords(
                memoryType.extension_ToOmronSimpleMemoryType(),
                memno,
                count
            );
            sendResult.throwExceptionIfError();

            return sendResult.value;
        }

        public void setBit(Interface.AbstractMemoryType memoryType, short memno, short bitno, bool value)
        {
        }

        public void setDWord(Interface.AbstractMemoryType memoryType, short memno, uint value)
        {
        }

        public void setWord(Interface.AbstractMemoryType memoryType, short memno, short value)
        {
        }
    }
}
