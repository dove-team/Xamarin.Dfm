﻿using System;
using System.Diagnostics;
using Xamarin.Dfm.Converter;

namespace Xamarin.Dfm
{
    public struct DanmakuProtocol
    {
        /// <summary>
        /// 消息总长度 (协议头 + 数据长度)
        /// </summary>
        public int PacketLength;
        /// <summary>
        /// 消息头长度 (固定为16[sizeof(DanmakuProtocol)])
        /// </summary>
        public short HeaderLength;
        /// <summary>
        /// 消息版本号
        /// </summary>
        public short Version;
        /// <summary>
        /// 消息类型
        /// </summary>
        public int Action;
        /// <summary>
        /// 参数, 固定为1
        /// </summary>
        public int Parameter;
        public static DanmakuProtocol FromBuffer(byte[] buffer)
        {
            try
            {
                if (buffer.Length < 16)
                    throw new ArgumentException();
                return new DanmakuProtocol()
                {
                    PacketLength = EndianBitConverter.BigEndian.ToInt32(buffer, 0),
                    HeaderLength = EndianBitConverter.BigEndian.ToInt16(buffer, 4),
                    Version = EndianBitConverter.BigEndian.ToInt16(buffer, 6),
                    Action = EndianBitConverter.BigEndian.ToInt32(buffer, 8),
                    Parameter = EndianBitConverter.BigEndian.ToInt32(buffer, 12),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("FromBuffer:" + ex.ToString());
                return default;
            }
        }
    }
}