﻿using HakDuinoSerial.Enum;
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
        /// Sends a command to move the mouse to the specified X and Y coordinates.
        /// </summary>
        /// <param name="x">The target X coordinate for the mouse movement.</param>
        /// <param name="y">The target Y coordinate for the mouse movement.</param>
        /// <returns>True if the command was successfully buffered; otherwise, false.</returns>
        bool MoveMouse(int x, int y);

        /// <summary>
        /// Sends a command to click the mouse using the specified button.
        /// </summary>
        /// <param name="mouseButton">The mouse button to click (left, right, etc.).</param>
        /// <returns>True if the command was successfully buffered; otherwise, false.</returns>
        bool ClickMouse(HakDuinoEnumButton mouseButton);

        /// <summary>
        /// Sends a command to perform a right mouse click.
        /// </summary>
        /// <returns>True if the command was successfully buffered; otherwise, false.</returns>
        bool MouseRightClick();

        /// <summary>
        /// Sends a command to perform a left mouse click.
        /// </summary>
        /// <returns>True if the command was successfully buffered; otherwise, false.</returns>
        bool MouseLeftClick();

        /// <summary>
        /// Sends a command to scroll the mouse wheel.
        /// </summary>
        /// <param name="direction">The direction to scroll (up or down).</param>
        /// <param name="amount">The amount to scroll.</param>
        /// <returns>True if the command was successfully buffered; otherwise, false.</returns>
        bool ScrollWheel(HakDuinoEnumButton direction, int amount);

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
