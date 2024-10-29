using HakDuinoSerial.Service;

namespace HakDuinoSerial
{
    internal class HakDuinoKeyboard : HakDuinoSerial, IHakDuinoKeyboard
    {
        public HakDuinoKeyboard(string COM_PORT, int BAUD_RATE = 250000) : base(COM_PORT, BAUD_RATE) { }
    }
}
