using HakDuinoSerial.Enum;

namespace HakDuinoSerial.Service
{
    /// <summary>
    /// Defines the interface for keyboard operations in the HakDuino system.
    /// This interface specifies methods for simulating keyboard actions,
    /// including pressing and releasing keys, as well as typing text.
    /// Implementations of this interface should provide the necessary logic 
    /// to communicate with the underlying hardware for these operations.
    /// </summary>
    internal interface IHakDuinoKeyboard
    {
        /// <summary>
        /// Sends a command to press a specific key.
        /// </summary>
        /// <param name="key">The key to be pressed.</param>
        /// <returns>True if the command was successfully buffered; otherwise, false.</returns>
        bool PressKey(EHakDuinoKeyboardButton key);

        /// <summary>
        /// Sends a command to release a specific key.
        /// </summary>
        /// <param name="key">The key to be released.</param>
        /// <returns>True if the command was successfully buffered; otherwise, false.</returns>
        bool ReleaseKey(EHakDuinoKeyboardButton key);

        /// <summary>
        /// Sends a command to type a specified string of text.
        /// </summary>
        /// <param name="text">The text to be typed.</param>
        /// <returns>True if the command was successfully buffered; otherwise, false.</returns>
        bool TypeText(string text);
    }
}
