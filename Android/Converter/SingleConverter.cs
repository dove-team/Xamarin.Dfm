using System.Runtime.InteropServices;

namespace Xamarin.Dfm.Converter
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct SingleConverter
    {
        [FieldOffset(0)]
        private readonly int intValue;
        [FieldOffset(0)]
        private readonly float floatValue;
        internal SingleConverter(int intValue)
        {
            floatValue = 0;
            this.intValue = intValue;
        }
        internal SingleConverter(float floatValue)
        {
            intValue = 0;
            this.floatValue = floatValue;
        }
        internal int GetIntValue() => intValue;
        internal float GetFloatValue() => floatValue;
    }
}