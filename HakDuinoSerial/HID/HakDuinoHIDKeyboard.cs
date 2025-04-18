using HakDuinoSerial.Enum;
using HakDuinoSerial.HID.Service;

namespace HakDuinoSerial.HID
{
    /// <summary>
    /// Represents a keyboard controller for the HakDuino system, implementing the <see cref="IHakDuinoHIDKeyboard"/> interface.
    /// This class allows for various keyboard operations, including pressing, releasing keys, and typing text.
    /// It communicates with the underlying hardware through HID commands.
    /// </summary>
    public class HakDuinoHIDKeyboard : HakDuinoHID, IHakDuinoHIDKeyboard
    {
        public HakDuinoHIDKeyboard(int VendorID, int ProductID) : base(VendorID, ProductID) { }

        public bool PressKey(EHakDuinoKeyboardButton key)
        {
            try
            {
                string command = $"P,{key.ToString()}";
                SendCustomCommand(command);
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
                string command = $"R,{key.ToString()}";
                SendCustomCommand(command);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool TypeKey(EHakDuinoKeyboardButton key)
        {
            try
            {
                PressKey(key);
                ReleaseKey(key);
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
                foreach (char c in text)
                {
                    EHakDuinoKeyboardButton key = GetKeyFromChar(c);
                    TypeKey(key);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private EHakDuinoKeyboardButton GetKeyFromChar(char c)
        {
            if (char.IsLetter(c))
            {
                char upperChar = char.ToUpper(c);
                if (Enum.EHakDuinoKeyboardButton.IsDefined(typeof(EHakDuinoKeyboardButton), upperChar))
                {
                    return (EHakDuinoKeyboardButton)Enum.EHakDuinoKeyboardButton.Parse(typeof(EHakDuinoKeyboardButton), upperChar.ToString());
                }
            }

            if (char.IsDigit(c))
            {
                string digitString = "Number" + c;
                if (Enum.EHakDuinoKeyboardButton.IsDefined(typeof(EHakDuinoKeyboardButton), digitString))
                {
                    return (EHakDuinoKeyboardButton)Enum.EHakDuinoKeyboardButton.Parse(typeof(EHakDuinoKeyboardButton), digitString);
                }
            }

            switch (c)
            {
                case ' ':
                    return EHakDuinoKeyboardButton.Space;
                case '\n':
                    return EHakDuinoKeyboardButton.Enter;
                case '\b':
                    return EHakDuinoKeyboardButton.Backspace;
                default:
                    throw new ArgumentException("Unsupported character: " + c);
            }
        }
    }
}
