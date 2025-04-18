using HakDuinoSerial.Enum;
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
        public HakDuinoHIDMouse(int VendorID, int ProductID) : base(VendorID, ProductID) { }

        public bool MoveMouse(int x, int y)
        {
            string command = $"{x},{y},0";
            return SendCustomCommand(command);
        }

        public bool ClickMouse(EHakDuinoMouseButton button)
        {
            string command = "0,0,1";
            return SendCustomCommand(command);
        }

        public bool MouseRightClick()
        {
            return ClickMouse(EHakDuinoMouseButton.RIGHT);
        }

        public bool MouseLeftClick()
        {
            return ClickMouse(EHakDuinoMouseButton.LEFT);
        }
    }
}
