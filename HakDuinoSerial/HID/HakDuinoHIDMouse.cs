using HakDuinoSerial.Enum;
using HakDuinoSerial.HID.Configuration;
using HakDuinoSerial.HID.Service;

namespace HakDuinoSerial.HID
{
    /// <summary>
    /// Represents a mouse controller for the HakDuino system, implementing the <see cref="IHakDuinoHIDMouse"/> interface.
    /// This class allows for various mouse operations, including moving the mouse and clicking buttons.
    /// It communicates with the underlying hardware through HID commands over a specified device path.
    /// </summary>
    public class HakDuinoHIDMouse : HakDuinoHID, IHakDuinoHIDMouse
    {
        // Store the config locally if needed, or access via protected base field if allowed
        private readonly HakDuinoHidConfig mouseConfig;

        /// <summary>
        /// Initializes a new instance of the HakDuinoHIDMouse class with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration settings for the HID device connection.</param>
        public HakDuinoHIDMouse(HakDuinoHidConfig configuration) : base(configuration)
        {
            // Store config specifically for mouse class if needed for quick access to command bytes
            this.mouseConfig = configuration;
        }

        public bool MoveMouse(int dx, int dy)
        {
            sbyte clampedDx = (sbyte)Math.Clamp(dx, -127, 127);
            sbyte clampedDy = (sbyte)Math.Clamp(dy, -127, 127);
            byte[] payload = new byte[] { (byte)clampedDx, (byte)clampedDy };

            // Use the command byte FROM THE CONFIG object
            return SendRawHidReport(mouseConfig.CommandMouseMove, payload);
        }

        public bool ClickMouse(EHakDuinoMouseButton button)
        {
            string command = "0,0,1";
            return SendCustomCommand(command);
        }

        public bool MouseRightClick()
        {
            // Use the command byte FROM THE CONFIG object
            return SendRawHidReport(mouseConfig.CommandRightClick, null);
        }

        public bool MouseLeftClick()
        {
            // Use the command byte FROM THE CONFIG object
            return SendRawHidReport(mouseConfig.CommandLeftClick, null);
        }
    }
}
