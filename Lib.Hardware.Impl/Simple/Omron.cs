using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Lib.NoDepsUtils;
using Lib.Log.Interface;

namespace OmronControllerUdpService
{
    public class Omron
    {
        /// <summary>
        /// Логгер для отладки
        /// </summary>
        ILog debugLogger;

        /// <summary>
        /// Целевой адрес
        /// </summary>
        IPEndPoint remoteEP;

        /// <summary>
        /// Таймаут на принятие данных
        /// </summary>
        int receiveTimeoutMSec;

        /// <summary>
        /// Таймаут на отправку данных
        /// </summary>
        int sendimeoutMSec;

        /// <summary>
        /// Здесь хранятся принятые по сети данные
        /// </summary>
        byte[] inputData = new byte[3000];

        /// <summary>
        /// Структура для преобразования MemoryType в реальное число
        /// </summary>
        string[] memoryTypeWord = new string[100];

        public Omron(string _ipAddress, int _udpPort, int _receiveTimeoutMSec, int _sendTimeoutMSec)
        {
            remoteEP = new IPEndPoint(IPAddress.Parse(_ipAddress), _udpPort);
            receiveTimeoutMSec = _receiveTimeoutMSec;
            sendimeoutMSec = _sendTimeoutMSec;

            memoryTypeWord[0] = "82";
            memoryTypeWord[1] = "B0";
            memoryTypeWord[2] = "B1";
            memoryTypeWord[3] = "B2";
            memoryTypeWord[10] = "A0";
            memoryTypeWord[11] = "A1";
            memoryTypeWord[12] = "A2";
            memoryTypeWord[13] = "A3";
            memoryTypeWord[14] = "A4";
            memoryTypeWord[15] = "A5";
            memoryTypeWord[16] = "A6";
            memoryTypeWord[17] = "A7";
            memoryTypeWord[18] = "A8";
            memoryTypeWord[19] = "A9";
            memoryTypeWord[20] = "AA";
            memoryTypeWord[21] = "AB";
            memoryTypeWord[22] = "AC";
        }

        /// <summary>
        /// Отправка данных
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <returns></returns>
        private SendResult sendCommand(StringBuilder stringBuilder)
        {
            SendResult sendResult = new SendResult();
            sendResult.result = false;
            sendResult.data = null;
            sendResult.message = null;
            sendResult.endCode = 0;
            sendResult.commandCode = 0;
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, this.receiveTimeoutMSec);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, this.sendimeoutMSec);

                    StringBuilder sb = new StringBuilder();
                    sb.Append("80"); // Information control field (ICF)
                    sb.Append("00"); // Reverved (RSV)
                    sb.Append("02"); // Gateway count (GCT)
                    sb.Append("00"); // PLC_NetNo (DNA, destination network address)
                    sb.Append("00"); // PLC_NodeNo (DA1, destination node address)
                    sb.Append("00"); // ??? (DA2, destination unit address)
                    sb.Append("00"); // PC_NetNo (SNA, source network address)
                    sb.Append("01"); // PC_NodeNo (SA1, source node address)
                    sb.Append("00"); // ?? (SA2, source unit address)
                    sb.Append("00"); // (SID, service id)
                    sb.Append(stringBuilder);

                    string hexString = sb.ToString();

                    byte[] outputData = Hex.HexStringToByteArray(hexString);

                    // логируем только если разработчик установил это поле
                    if (debugLogger != null)
                    {
                        debugLogger.Info($"Sent: {hexString}");
                    }

                    socket.SendTo(outputData, outputData.Length, SocketFlags.None, (EndPoint)remoteEP);

                    int receivedCount = socket.Receive(inputData);
                    if (receivedCount < 14)
                    {
                        sendResult.message = $"Размер данных < 14 (=={receivedCount})";
                        return sendResult;
                    }
                    else
                    {
                        sendResult.commandCode = Transform.getUshortFromByteBuffer(ref inputData, 10);
                        sendResult.endCode = Transform.getUshortFromByteBuffer(ref inputData, 12);

                        sendResult.data = new byte[receivedCount-14];
                        Array.Copy(inputData, 14, sendResult.data, 0, receivedCount-14);

                        // логируем только если разработчик установил это поле
                        if (debugLogger != null)
                        {
                            debugLogger.Info($"Received: {sendResult.data.extension_ByteArrayToHexString()}");
                        }

                        sendResult.result = true;

                        return sendResult;
                    }
                }
            }
            catch(Exception ex)
            {
                sendResult.message = ex.Message;
                return sendResult;
            }
        }

        /// <summary>
        /// Чтение памяти
        /// </summary>
        /// <param name="memoryType">Тип памяти</param>
        /// <param name="offset">Смещение</param>
        /// <param name="size">Длина</param>
        /// <param name="bitNo">Номер бита (по умолчанию 0)</param>
        /// <returns>Структуру SendResult</returns>
        private SendResult readMemory(OmronMemoryType memoryType, short offset, short size, byte bitNo = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0101");
            sb.Append(memoryTypeWord[(int)memoryType]);
            sb.Append($"{offset:X4}0{bitNo}".PadLeft(6, '0'));
            sb.AppendFormat("{0,4:X4}", size);

            return sendCommand(sb);
        }

        /// <summary>
        /// Чтение слова (16 бит)
        /// </summary>
        /// <param name="memoryType">Тип памяти</param>
        /// <param name="offset">Смещение</param>
        /// <returns></returns>
        public SendResultTyped<ushort> readWord(OmronMemoryType memoryType, short offset)
        {
            SendResult tempResult = readMemory(memoryType, offset, 1);

            return new SendResultTyped<ushort>(
                tempResult,
                tempResult.goodResult 
                ? Transform.getUshortFromByteBuffer(ref tempResult.data, 0)
                : (ushort)0
            );
        }

        public SendResultTyped<short[]> readWords(OmronMemoryType memoryType, short offset, short count)
        {
            SendResult sendResult = readMemory(memoryType, offset, count);
            if (!sendResult.goodResult)
                return new SendResultTyped<short[]>(sendResult, null);
            
            // преобразуем byte[] в ushort[]
            short[] result = new short[sendResult.data.Length / 2];
            Buffer.BlockCopy(sendResult.data, 0, result, 0, sendResult.data.Length);

            // меняем порядок байт
            for(int i=0; i<result.Length; i++)
                result[i] = (short)Transform.swapBytes((ushort)result[i]);

            return new SendResultTyped<short[]>(sendResult, result);
        }

        /// <summary>
        /// Чтение бита
        /// </summary>
        /// <param name="memoryType">Тип памяти</param>
        /// <param name="offset">Смещение</param>
        /// <param name="bitNo">Номер бита</param>
        /// <returns></returns>
        public SendResultTyped<bool> readBit(OmronMemoryType memoryType, short offset, byte bitNo)
        {
            SendResultTyped<ushort> tempResult = readWord(memoryType, offset);

            return new SendResultTyped<bool>(
                tempResult.result,
                tempResult.result.goodResult
                ? Transform.getBitFromWord(tempResult.value, bitNo)
                : false
            );
        }

        /// <summary>
        /// Чтение двойного слова
        /// </summary>
        /// <param name="memoryType">Тип памяти</param>
        /// <param name="offset">Смещение</param>
        /// <returns></returns>
        public SendResultTyped<uint> readDWord(OmronMemoryType memoryType, short offset)
        {
            SendResult tempResult = readMemory(memoryType, offset, 2);

            return new SendResultTyped<uint>(
                tempResult,
                tempResult.goodResult
                ? Transform.getUInt32FromByteBuffer(ref tempResult.data, 0)
                : (uint)0
            );
        }

        /// <summary>
        /// Установка логгера для отладки
        /// </summary>
        /// <param name="_debugLogger"></param>
        public void setDebugLogger(ILog _debugLogger)
        {
            debugLogger = _debugLogger;
        }
    }
}
