﻿namespace Xamarin.Dfm.Converter
{
    internal class BigEndianBitConverter : EndianBitConverter
    {
        internal BigEndianBitConverter() { }
        public override bool IsLittleEndian { get; } = false;
        public override byte[] GetBytes(short value)
        {
            return new[] { (byte)(value >> 8), (byte)value };
        }
        public override byte[] GetBytes(int value)
        {
            return new[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };
        }
        public override byte[] GetBytes(long value)
        {
            return new[] {
                (byte)(value >> 56), (byte)(value >> 48), (byte)(value >> 40), (byte)(value >> 32),
                (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value
            };
        }
        public override short ToInt16(byte[] value, int startIndex)
        {
            CheckArguments(value, startIndex, sizeof(short));
            return (short)((value[startIndex] << 8) | (value[startIndex + 1]));
        }
        public override int ToInt32(byte[] value, int startIndex)
        {
            CheckArguments(value, startIndex, sizeof(int));
            return (value[startIndex] << 24) | (value[startIndex + 1] << 16) | (value[startIndex + 2] << 8) | (value[startIndex + 3]);
        }
        public override long ToInt64(byte[] value, int startIndex)
        {
            CheckArguments(value, startIndex, sizeof(long));
            int highBytes = (value[startIndex] << 24) | (value[startIndex + 1] << 16) | (value[startIndex + 2] << 8) | (value[startIndex + 3]);
            int lowBytes = (value[startIndex + 4] << 24) | (value[startIndex + 5] << 16) | (value[startIndex + 6] << 8) | (value[startIndex + 7]);
            return (uint)lowBytes | ((long)highBytes << 32);
        }
    }
}