namespace HakDuinoSerial.HID.Service
{
    /// <summary>
    /// Defines an interface for interacting with an Arduino device over an HID connection.
    /// This interface provides methods to manage the connection state, send custom commands,
    /// and retrieve device information such as command history and device details.
    /// </summary>
    internal interface IHakDuinoHID
    {
        /// <summary>
        /// Opens a connection to the HID device.
        /// </summary>
        /// <returns>True if the connection is successfully opened; otherwise, false.</returns>
        bool OpenConnection();

        /// <summary>
        /// Sends a custom command to the HID device immediately.
        /// </summary>
        /// <param name="command">The command string to send to the device.</param>
        /// <returns>True if the command was sent successfully; otherwise, false.</returns>
        bool SendCustomCommand(string command);

        /// <summary>
        /// Retrieves information about the connected HID device.
        /// </summary>
        /// <returns>A string containing details about the connected HID device, such as vendor and product ID.</returns>
        string GetHIDDeviceInfo();

        /// <summary>
        /// Retrieves a history of commands sent to the HID device.
        /// </summary>
        /// <returns>A list of strings representing the command history.</returns>
        List<string> GetCommandHistory();

        /// <summary>
        /// Retrieves a history of commands sent to the HID device.
        /// </summary>
        /// <returns>A list of strings representing the command history.</returns>
        bool CloseConnection();
    }
}
