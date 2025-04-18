using HakDuinoSerial.Enum;

namespace HakDuinoSerial.HID.Service
{
    /// <summary>
    /// Defines an interface for simulating keyboard operations via an HID connection to an Arduino device.
    /// This interface specifies methods for pressing and releasing keys, typing individual keys, 
    /// and sending strings of text. Implementations should provide the necessary logic to 
    /// communicate with the HID hardware for these operations.
    /// </summary>
    internal interface IHakDuinoHIDKeyboard
    {
        /// <summary>
        /// Sends a command to press a specified key.
        /// </summary>
        /// <param name="key">The key to press.</param>
        /// <returns>True if the key press command was sent successfully; otherwise, false.</returns>
        bool PressKey(EHakDuinoKeyboardButton key);

        /// <summary>
        /// Sends a command to release a specified key.
        /// </summary>
        /// <param name="key">The key to release.</param>
        /// <returns>True if the key release command was sent successfully; otherwise, false.</returns>
        bool ReleaseKey(EHakDuinoKeyboardButton key);

        /// <summary>
        /// Sends a command to type (press and release) a specified key.
        /// </summary>
        /// <param name="key">The key to send a stroke for.</param>
        /// <returns>True if the key stroke command was sent successfully; otherwise, false.</returns>
        bool TypeKey(EHakDuinoKeyboardButton key);

        /// <summary>
        /// Sends a string of text by simulating key presses for each character.
        /// </summary>
        /// <param name="text">The text to type.</param>
        /// <returns>True if the text was sent successfully; otherwise, false.</returns>
        bool TypeText(string text);
    }
}
