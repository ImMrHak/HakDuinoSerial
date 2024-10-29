using HakDuinoSerial.Enum;
using System.IO.Ports;

namespace HakDuinoSerial.Service
{
    /// <summary>
    /// Defines the contract for interacting with an Arduino over a serial connection.
    /// This interface provides methods for establishing and closing the connection, 
    /// executing buffered commands, controlling mouse actions (movement, clicks, scrolling), 
    /// and retrieving device information.
    /// </summary>
    internal interface IHakDuinoSerial
    {
        /// <summary>
        /// Opens a connection to the serial port for communication with the Arduino.
        /// </summary>
        /// <returns>True if the connection was successfully opened; otherwise, false.</returns>
        bool OpenConnection();

        /// <summary>
        /// Sends a custom command directly to the Arduino.
        /// </summary>
        /// <param name="command">The custom command to send.</param>
        /// <returns>True if the command was successfully sent; otherwise, false.</returns>
        bool SendCustomCommand(string command);

        /// <summary>
        /// Asynchronously sends all buffered commands to the Arduino if the buffer exceeds the specified threshold.
        /// </summary>
        /// <param name="threshold">The minimum buffer length required to trigger the flush.</param>
        /// <returns>True if commands were successfully sent; otherwise, false.</returns>
        Task<bool> FlushBufferedCommandsAsync(int threshold);

        /// <summary>
        /// Retrieves the Arduino information such as name, VID, and PID.
        /// </summary>
        /// <returns>A string containing the Arduino information.</returns>
        string GetArduinoInfo();

        /// <summary>
        /// Retrieves the SerialPort instance used for communication with the Arduino.
        /// </summary>
        /// <returns>The SerialPort instance associated with the Arduino connection.</returns>
        SerialPort GetSerialPort();

        /// <summary>
        /// Retrieves a list of all commands sent to the Arduino during the current session.
        /// </summary>
        /// <returns>A list of string commands.</returns>
        List<string> GetCommandHistory();

        /// <summary>
        /// Closes the connection to the serial port.
        /// </summary>
        /// <returns>True if the connection was successfully closed; otherwise, false.</returns>
        bool CloseConnection();
    }
}
