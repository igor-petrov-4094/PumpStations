using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Hardware.Interface
{
    /// <summary>
    /// Интерфейс доступа к памяти контроллера
    /// </summary>
    public interface ISimpleControllerMemory
    {
        bool open();
        void close();
        bool isRunning();

        void setWord(AbstractMemoryType memoryType, short memno, short value);
        short getWord(AbstractMemoryType memoryType, short memno);
        short[] getWords(AbstractMemoryType memoryType, short memno, short count);

        void setDWord(AbstractMemoryType memoryType, short memno, UInt32 value);
        UInt32 getDWord(AbstractMemoryType memoryType, short memno);

        void setBit(AbstractMemoryType memoryType, short memno, short bitno, bool value);
        bool getBit(AbstractMemoryType memoryType, short memno, short bitno);
    }
}
