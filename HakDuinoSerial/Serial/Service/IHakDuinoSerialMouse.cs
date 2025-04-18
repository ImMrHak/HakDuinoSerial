using HakDuinoSerial.Enum;

namespace HakDuinoSerial.Serial.Service
{
    /// <summary>
    /// Defines the interface for controlling mouse operations in the HakDuino system.
    /// This interface provides methods for moving the mouse, clicking buttons, and scrolling the wheel.
    /// Implementations of this interface should provide the necessary logic to translate these commands
    /// into actions performed by the mouse hardware.
    /// </summary>
    internal interface IHakDuinoSerialMouse
    {
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
        bool ClickMouse(EHakDuinoMouseButton mouseButton);

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
        bool ScrollWheel(EHakDuinoMouseButton direction, int amount);
    }
}
