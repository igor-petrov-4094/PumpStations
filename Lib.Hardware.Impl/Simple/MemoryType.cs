namespace OmronControllerUdpService
{
    public enum OmronMemoryType : short
    {
        DM = 0,
        CIO = 1,
        WR = 2,
        HR = 3,
        EM0 = 10, // 0x000A
        EM1 = 11, // 0x000B
        EM2 = 12, // 0x000C
        EM3 = 13, // 0x000D
        EM4 = 14, // 0x000E
        EM5 = 15, // 0x000F
        EM6 = 16, // 0x0010
        EM7 = 17, // 0x0011
        EM8 = 18, // 0x0012
        EM9 = 19, // 0x0013
        EMA = 20, // 0x0014
        EMB = 21, // 0x0015
        EMC = 22, // 0x0016
    }
}
