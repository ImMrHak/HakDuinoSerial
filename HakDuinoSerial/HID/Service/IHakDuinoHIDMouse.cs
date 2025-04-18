using HakDuinoSerial.Enum;

namespace HakDuinoSerial.HID.Service
{
    /// <summary>
    /// Defines an interface for controlling mouse operations via an HID connection to an Arduino device.
    /// This interface includes methods for simulating mouse movements, clicks, and scrolling actions.
    /// Implementations should provide the necessary logic to communicate with the HID hardware for mouse operations.
    /// </summary>
    internal interface IHakDuinoHIDMouse
    {
        /// <summary>
        /// Sends a command to move the mouse to the specified X and Y coordinates.
        /// </summary>
        /// <param name="x">The target X coordinate for the mouse movement.</param>
        /// <param name="y">The target Y coordinate for the mouse movement.</param>
        /// <returns>True if the command was successfully sent; otherwise, false.</returns>
        bool MoveMouse(int x, int y);

        /// <summary>
        /// Sends a command to click the mouse using the specified button.
        /// </summary>
        /// <param name="mouseButton">The mouse button to click (left, right, etc.).</param>
        /// <returns>True if the command was successfully sent; otherwise, false.</returns>
        bool ClickMouse(EHakDuinoMouseButton button);

        /// <summary>
        /// Sends a command to perform a right mouse click.
        /// </summary>
        /// <returns>True if the command was successfully sent; otherwise, false.</returns>
        bool MouseRightClick();

        /// <summary>
        /// Sends a command to perform a left mouse click.
        /// </summary>
        /// <returns>True if the command was successfully sent; otherwise, false.</returns>
        bool MouseLeftClick();
    }
}
