using HakDuinoSerial.Enum;
using HakDuinoSerial.Service;

namespace HakDuinoSerial
{
    /// <summary>
    /// Represents a mouse controller for the HakDuino system, implementing the <see cref="IHakDuinoMouse"/> interface.
    /// This class allows for various mouse operations, including moving the mouse, clicking buttons, and scrolling.
    /// It communicates with the underlying hardware through buffered commands over a specified COM port.
    /// </summary>
    internal class HakDuinoMouse : HakDuinoSerial, IHakDuinoMouse
    {
        public HakDuinoMouse(string COM_PORT, int BAUD_RATE = 250000) : base(COM_PORT, BAUD_RATE) { }

        public bool MoveMouse(int x, int y)
        {
            try
            {
                string mouseMovement = $"M,{x},{y}";
                BufferCommand(mouseMovement);
                addActionToList($"Mouse Mouvement X : {x} Y : {y}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool ClickMouse(EHakDuinoMouseButton mouseButton)
        {
            try
            {
                string mouseClick = $"C,{mouseButton.ToString()}";
                BufferCommand(mouseClick);
                addActionToList($"Mouse Click : {mouseButton.ToString()}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool MouseRightClick()
        {
            return ClickMouse(EHakDuinoMouseButton.RIGHT);
        }

        public bool MouseLeftClick()
        {
            return ClickMouse(EHakDuinoMouseButton.LEFT);
        }

        public bool ScrollWheel(EHakDuinoMouseButton direction, int amount)
        {
            try
            {
                string scrollCommand = direction switch
                {
                    EHakDuinoMouseButton.SCROLL_UP => $"S,UP,{amount}", // Send the command with proper format
                    EHakDuinoMouseButton.SCROLL_DOWN => $"S,DOWN,{amount}", // Send the command with proper format
                    _ => throw new ArgumentException("Invalid scroll direction")
                };
                BufferCommand(scrollCommand);
                addActionToList($"Mouse Scroll Direction : {direction} Amount : {amount}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
