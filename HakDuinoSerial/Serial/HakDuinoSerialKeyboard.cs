using HakDuinoSerial.Enum;
using HakDuinoSerial.Serial.Service;

namespace HakDuinoSerial.Serial
{
    /// <summary>
    /// Represents a keyboard controller for the HakDuino system, implementing the <see cref="IHakDuinoSerialKeyboard"/> interface.
    /// This class allows for various keyboard operations, including pressing and releasing keys, as well as typing text.
    /// It communicates with the underlying hardware through buffered commands over a specified COM port.
    /// </summary>
    public class HakDuinoSerialKeyboard : HakDuinoSerial, IHakDuinoSerialKeyboard
    {
        public HakDuinoSerialKeyboard(string COM_PORT, int BAUD_RATE = 250000) : base(COM_PORT, BAUD_RATE) { }

        public bool PressKey(EHakDuinoKeyboardButton key)
        {
            try
            {
                string command = $"K,P,{key}";
                BufferCommand(command);
                addActionToList($"Pressed key: {key}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool ReleaseKey(EHakDuinoKeyboardButton key)
        {
            try
            {
                string command = $"K,R,{key}";
                BufferCommand(command);
                addActionToList($"Released key: {key}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool TypeText(string text)
        {
            try
            {
                string command = $"K,T,{text}";
                BufferCommand(command);
                addActionToList($"Typed text: {text}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
