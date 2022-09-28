using System;

namespace Lib.Hardware.Impl.SimpleControllerMemory
{
    /// <summary>
    /// Конвертер типов памяти
    /// </summary>
    public static class MemoryTypeConverter 
    {
        public static OmronFinsTCP.Net.PlcMemory extension_ToOmronFinsTCPMemoryType(this Lib.Hardware.Interface.AbstractMemoryType memoryType)
        {
            switch (memoryType)
            {
                case Lib.Hardware.Interface.AbstractMemoryType.D: return OmronFinsTCP.Net.PlcMemory.DM;
                case Lib.Hardware.Interface.AbstractMemoryType.CIO: return OmronFinsTCP.Net.PlcMemory.CIO;
                case Interface.AbstractMemoryType.W: return OmronFinsTCP.Net.PlcMemory.WR;
                default: throw new Exception($"Данный тип памяти не поддерживается: {memoryType}");
            }
        }

        public static OmronControllerUdpService.OmronMemoryType extension_ToOmronSimpleMemoryType(this Lib.Hardware.Interface.AbstractMemoryType memoryType)
        {
            switch (memoryType)
            {
                case Lib.Hardware.Interface.AbstractMemoryType.D: return OmronControllerUdpService.OmronMemoryType.DM;
                case Lib.Hardware.Interface.AbstractMemoryType.CIO: return OmronControllerUdpService.OmronMemoryType.CIO;
                case Interface.AbstractMemoryType.W: return OmronControllerUdpService.OmronMemoryType.WR;
                default: throw new Exception($"Данный тип памяти не поддерживается: {memoryType}");
            }
        }

#if DEBUG
        public static PoohPlcLink.PoohFinsETN.MemoryTypes extension_ToPoohlinkMemoryType(this Lib.Hardware.Interface.AbstractMemoryType memoryType)
        {
            switch (memoryType)
            {
                case Lib.Hardware.Interface.AbstractMemoryType.D: return PoohPlcLink.PoohFinsETN.MemoryTypes.DM;
                case Lib.Hardware.Interface.AbstractMemoryType.CIO: return PoohPlcLink.PoohFinsETN.MemoryTypes.CIO;
                case Interface.AbstractMemoryType.W: return PoohPlcLink.PoohFinsETN.MemoryTypes.WR;
                default: throw new Exception($"Данный тип памяти не поддерживается: {memoryType}");
            }
        }
#endif
    }
}
